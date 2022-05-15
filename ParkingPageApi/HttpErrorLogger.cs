namespace ParkingPageApi;

public class HttpErrorLogger : DelegatingHandler
{
    private readonly ILogger<HttpErrorLogger> _logger;

    public HttpErrorLogger(ILogger<HttpErrorLogger> logger)
    {
        _logger = logger;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ctx)
    {
        var response = await base.SendAsync(request, ctx);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ctx);
            _logger.LogWarning("Failed to update received:\n{Response}", response);
            _logger.LogWarning("Received the following in the body: \n{Body}", body);
        }

        return response;
    }
}