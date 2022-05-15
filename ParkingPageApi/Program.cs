var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddHttpClient<IDynamicDnsClient, NamecheapDynamicDnsClient>()
    .WithJitterRetryPolicy()
    .AddHttpMessageHandler<HttpErrorBodyLogger>();

builder.Services.AddDbContext<ParkingPageContext>(opt => opt.UseSqlite());
builder.Services.AddHttpContextAccessor();

builder.Configuration.AddEnvironmentVariables("ARC_");
builder.Configuration.AddJsonFile("secrets.json", optional: true);

builder.Services
    .AddOptions<List<DynamicDnsSetting>>()
    .BindConfiguration("DynamicDnsUrls");

builder.Logging.AddConsole();

var app = builder.Build();

var dynamicDnsSettings = app.Services.GetRequiredService<IOptions<List<DynamicDnsSetting>>>().Value;

app.MapGet("/getIp", (string? name, [FromServices] IHttpContextAccessor accessor) =>
{
    if (string.IsNullOrWhiteSpace(name))
    {
        app.Logger.LogInformation("No server name given");
        return Results.BadRequest("ServerName is required");
    }
    
    var remoteIp = accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    app.Logger.LogInformation("Received request from {Ip}", remoteIp);
    return Results.Ok(remoteIp);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();