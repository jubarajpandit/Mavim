using Mavim.Libraries.Authorization.Clients;
using Mavim.Libraries.Wopi.Configuration;
using Mavim.Libraries.Wopi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Libraries.Wopi.Extensions
{
    public static class WopiHandlerExtension
    {
        private const string WopiHostApiEndPointKey = "Mavim:WopiSettings:WopiHostApiEndPoint";

        /// <summary>
        /// Adds the WopiHost Client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        public static IServiceCollection AddWopiHostClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null) throw new System.ArgumentNullException(nameof(configuration));

            string wopiHostApiEndPoint = configuration.GetSection(WopiHostApiEndPointKey).Value;

            services.Configure<AzWopiHostAppConfigSettings>(options =>
            {
                options.ApiEndpoint = wopiHostApiEndPoint;
            });

            services.AddHttpClient<IWopiHostClient, WopiHostClient>()
                .AddPolicyHandler(PollyClient.GetRetryPolicy())
                .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());

            return services;
        }
    }
}