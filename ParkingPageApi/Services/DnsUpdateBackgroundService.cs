namespace ParkingPageApi.Services;

public class DnsUpdateBackgroundService : BackgroundService
{
    private readonly IDynamicDnsClient _dynamicDnsClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DnsUpdateBackgroundService> _logger;
    private readonly IEnumerable<DynamicDnsSetting> _ddnsSettings;

    public DnsUpdateBackgroundService(IDynamicDnsClient dynamicDnsClient, 
        IOptions<IEnumerable<DynamicDnsSetting>> ddnsSettings,
        IHttpClientFactory httpClientFactory,
        ILogger<DnsUpdateBackgroundService> logger)
    {
        _dynamicDnsClient = dynamicDnsClient;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _ddnsSettings = ddnsSettings.Value ?? throw new ArgumentNullException(nameof(ddnsSettings));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTime = TimeSpan.FromSeconds(2);
        try
        {
            stoppingToken.ThrowIfCancellationRequested();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delayTime, stoppingToken);
                _logger.LogInformation("Background Update {Time}", DateTime.UtcNow);
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("Cancellation requested terminating background service");
        }
    }
    
    
}
