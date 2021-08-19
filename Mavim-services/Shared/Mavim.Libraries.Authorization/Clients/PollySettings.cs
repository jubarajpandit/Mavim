using Polly;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;

namespace Mavim.Libraries.Authorization.Clients
{
    public static class PollyClient
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            const int retryCount = 5;
            const int exponend = 2;

            return HttpPolicyExtensions.HandleTransientHttpError().OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(retryCount, retryAttempts =>
                    TimeSpan.FromSeconds(Math.Pow(exponend, retryAttempts))
                    + TimeSpan.FromMilliseconds(new Random().Next(0, 100))
                );
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPatternPolicy()
        {
            const int handledEventsAllowedBeforeBreaking = 3;
            TimeSpan durationOfBreak = TimeSpan.FromSeconds(30);

            return HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, durationOfBreak);
        }
    }
}
