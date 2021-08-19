using Mavim.Libraries.Authorization.Clients;
using Mavim.Libraries.Authorization.Configuration;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Authorization.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Libraries.Authorization.Extensions
{
    public static class AuthorizationChangelogHandlerExtension
    {
        /// <summary>
        /// Adds the authorization Changelog.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="authorizationApiEndPointKey">The authorization API end point key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        public static IServiceCollection AddAuthorizationChangelog(this IServiceCollection services, IConfiguration configuration, string authorizationApiEndPointKey)
        {
            if (configuration is null) throw new System.ArgumentNullException(nameof(configuration));

            string authApiEndPoint = configuration.GetSection(authorizationApiEndPointKey).Value;

            services.Configure<AzAuthorizationAppConfigSettings>(options =>
            {
                options.ApiEndpoint = authApiEndPoint;
            });

            services.AddHttpClient<IAuthorizationClient, AuthorizationClient>()
                .AddPolicyHandler(PollyClient.GetRetryPolicy())
                .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());

            return services;
        }

        public static IApplicationBuilder UseAuthorizationChangelog(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationChangelogMiddleware>();

            return app;
        }

    }
}