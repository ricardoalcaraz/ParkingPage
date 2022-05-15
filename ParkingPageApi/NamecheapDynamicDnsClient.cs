namespace ParkingPageApi;

public class NamecheapDynamicDnsClient : IDynamicDnsClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NamecheapDynamicDnsClient> _logger;
    private const string BASE_URL = "https://dynamicdns.park-your-domain.com";

    public NamecheapDynamicDnsClient(HttpClient httpClient, ILogger<NamecheapDynamicDnsClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<bool> TryUpdate(UpdateDynamicDns dynamicDn, CancellationToken ctx)
    {
        _logger.LogInformation("Starting update to NamecheapDns to {Dns}", dynamicDn);
        
        var url = BASE_URL
            .AppendPathSegment("update")
            .SetQueryParams(dynamicDn);
        
        var response = await _httpClient.GetAsync(url, ctx);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Successfully updated {Domain}", dynamicDn.Domain);
        return response.IsSuccessStatusCode;
    }
}