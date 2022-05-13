using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingPageApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ParkingPageContext>(opt => opt.UseSqlite());
builder.Services.AddHttpContextAccessor();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapGet("/ddns",  async (string host, string ip, [FromServices] HttpClient httpClient, 
    [FromServices] IHttpContextAccessor ctx) =>
{
    var remoteIp = ctx.HttpContext?.Connection.RemoteIpAddress;
    app.Logger.LogDebug("Remote Ip is {Ip}", remoteIp);
    const string BASE_URL = "https://dynamicdns.park-your-domain.com/update";
    var list = new List<int>();
    var queryBuilder = new QueryBuilder
    {
        { "host", host },
        { "domain", "ricardoalcaraz.dev" },
        { "password", "f460fadea4a04024bd9d43ce7ab66f1b" },
        { "ip", ip }
    };
    var ddnsUrl = BASE_URL + queryBuilder.ToQueryString();
    app.Logger.LogDebug("Making request with params {Query}", queryBuilder);
    app.Logger.LogDebug("Making request to {Url}", ddnsUrl);
    var response = await httpClient.GetAsync(ddnsUrl);
    if (response.IsSuccessStatusCode)
    {
        app.Logger.LogInformation("Successfully updated dns to {Ip}", ip);
    }
    else
    {
        var content = await response.Content.ReadAsStringAsync();
        app.Logger.LogWarning("Unable to update ip: \n{Body}", content);
        return Results.Problem(detail: content, statusCode: (int)response.StatusCode);
    }

    return Results.Ok();
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