using LazyCache;
using Mavim.Libraries.Authorization.Clients;
using Mavim.Libraries.Middlewares.WopiValidator.Helpers;
using Mavim.Libraries.Middlewares.WopiValidator.WopiProofKeyValidator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Libraries.Middlewares.WopiValidator.Extensions
{
    public static class WopiValidatorExtension
    {
        public static void AddWopiValidationMiddleware(this IServiceCollection services, IConfiguration configuration)
        {
            services.LoadWopiAzureAppConfigSettings(configuration);
            services.AddScoped<IAppCache, CachingService>();
            services.AddHttpClient<IProofKeyValidator, ProofKeyValidator>()
                .AddPolicyHandler(PollyClient.GetRetryPolicy())
                .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());

            services.AddHttpClient<IWopiDiscoveryCache, WopiDiscoveryCache>()
                .AddPolicyHandler(PollyClient.GetRetryPolicy())
                .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());
        }

        public static void UseWopiValidationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<WopiValidationMiddleware>();
        }
    }
}
