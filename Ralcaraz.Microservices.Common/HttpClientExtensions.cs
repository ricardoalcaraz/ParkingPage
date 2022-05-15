using Microsoft.Extensions.DependencyInjection;

namespace Ralcaraz.Microservices.Common;

public static class HttpClientExtensions
{
    public static IHttpClientBuilder WithJitterRetryPolicy(this IHttpClientBuilder builder) => builder.AddPolicyHandler(GetRetryPolicy());
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(delay);
    }
}