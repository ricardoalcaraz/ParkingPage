using Microsoft.AspNetCore.Http.Extensions;

namespace ParkingPageApi;

public class DdnsHostedService : BackgroundService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<DdnsHostedService> _logger;

    public DdnsHostedService(IHttpClientFactory clientFactory, ILogger<DdnsHostedService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
        }
    }

    private async Task UpdateDns(CancellationToken cancellationToken)
    {
        using var httpClient = _clientFactory.CreateClient();
        const string BASE_URL = "https://dynamicdns.park-your-domain.com/update";
        var queryBuilder = new QueryBuilder();
        queryBuilder.Add("host", "@");
        queryBuilder.Add("domain", "ricardoalcaraz.dev");
        queryBuilder.Add("password", "f460fadea4a04024bd9d43ce7ab66f1b");
        var ipAddress = "136.26.16.149";
        queryBuilder.Add("ip", ipAddress);
        var ddnsUrl = BASE_URL + queryBuilder.ToQueryString();
        var response = await httpClient.GetAsync(ddnsUrl, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successfully updated dns to {Ip}", ipAddress);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("Unable to update ip: \n{Body}", content);
        }
    }
}