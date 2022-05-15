namespace Ralcaraz.Microservices.Common;

public class HttpErrorBodyLogger : DelegatingHandler
{
    private readonly ILogger<HttpErrorBodyLogger> _logger;

    public HttpErrorBodyLogger(ILogger<HttpErrorBodyLogger> logger)
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