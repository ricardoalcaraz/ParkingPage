using System.Net;
using MediatR;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(AzureDevPush));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddHttpClient<IDynamicDnsClient, NamecheapDynamicDnsClient>()
    .WithJitterRetryPolicy()
    .AddHttpMessageHandler<HttpErrorBodyLogger>();

builder.Services.AddDbContext<ParkingPageContext>(opt => opt.UseSqlite());
builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<DdnsHostedService>();

builder.Configuration.AddEnvironmentVariables("ARC_");
builder.Configuration.AddJsonFile("secrets.json", optional: true);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("172.19.0.0"), 24));
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:172.19.0.4"), 128));
    options.KnownProxies.Add(IPAddress.Parse("::ffff:172.19.0.4"));
    options.AllowedHosts.Add("api.ricardoalcaraz.dev");
});
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
    options.RequestHeaders.Add("X-Forwarded-For");
    options.RequestHeaders.Add("X-Forwarded-Host");
    options.RequestHeaders.Add("X-Forwarded-Proto");
    
});
builder.Services
    .AddOptions<List<DynamicDnsSetting>>()
    .BindConfiguration("DynamicDnsUrls");

builder.Logging
    .AddConsole()
    .AddSystemdConsole()
    .AddDebug();

var app = builder.Build();

var dynamicDnsSettings = app.Services.GetRequiredService<IOptions<List<DynamicDnsSetting>>>().Value;


app.UseForwardedHeaders();
app.UseHttpLogging();
app.Use(async (context, next) =>
{
    // Connection: RemoteIp
    app.Logger.LogInformation("Request RemoteIp: {RemoteIpAddress}",
        context.Connection.RemoteIpAddress);

    await next(context);
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapGet("/getIp", (string? name, [FromServices] IHttpContextAccessor accessor) =>
{
    if (string.IsNullOrWhiteSpace(name))
    {
        app.Logger.LogInformation("No server name given");
        return Results.BadRequest("ServerName is required");
    }
    
    var remoteIp = accessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    
    if (IPAddress.TryParse(remoteIp, out var ipAddress))
    {
        var ipv4Ip = ipAddress?.MapToIPv4().ToString();
        var ipv6Ip = ipAddress?.MapToIPv6().ToString();
        app.Logger.LogInformation("Received request from {Ipv4}, {Ipv6}", ipv4Ip, ipv6Ip);
    
        return Results.Ok(new {ipv4Ip, ipv6Ip});
    }
    else
    {
        var ip = accessor.HttpContext!.Connection.RemoteIpAddress;
        var ipv4Ip = ip?.MapToIPv4().ToString();
        var ipv6Ip = ip?.MapToIPv6().ToString();
        app.Logger.LogInformation("Received request from {Ipv4}, {Ipv6}", ipv4Ip, ipv6Ip);

        return Results.Ok(new {ipv4Ip, ipv6Ip});
    }
});

app.MapGet("/getIpOther", ([FromServices] IHttpContextAccessor accessor) =>
{
    var forwardedIp = accessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var httpContextIp = accessor.HttpContext!.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    return Results.Ok(new {forwardedIp, httpContextIp});
});

app.Run();