using Mavim.Libraries.Authorization.Clients;
using Mavim.Manager.Authorization.Read.Clients;
using Mavim.Manager.Authorization.Read.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Authorization.Read.Extensions
{
    /// <summary>
    /// Connect Http Rest Client Extension
    /// </summary>
    public static class ConnectHttpClientExtension
    {
        private readonly static string ConnectClientAuthKey = "Mavim:AuthReadSettings:ConnectApiEndPoint";
        /// <summary>
        /// Adds the Connect http rest client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        public static IServiceCollection AddConnectHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null) throw new System.ArgumentNullException(nameof(configuration));

            string connectApiEndPoint = configuration.GetSection(ConnectClientAuthKey).Value;

            services.Configure<ConnectClientSettings>(options =>
            {
                options.ApiEndpoint = connectApiEndPoint;
            });

            services.AddHttpClient<IConnectClient, ConnectClient>()
                .AddPolicyHandler(PollyClient.GetRetryPolicy())
                .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());

            return services;
        }

    }
}