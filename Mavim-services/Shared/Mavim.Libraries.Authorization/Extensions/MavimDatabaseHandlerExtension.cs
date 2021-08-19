using Mavim.Libraries.Authorization.Clients;
using Mavim.Libraries.Authorization.Configuration;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Authorization.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Libraries.Authorization.Extensions
{
    public static class AuthorizationDatabaseExtension
    {
        private const string DatabaseInfoApiEndPoint = "Mavim:DatabaseInfoSettings:ApiEndPoint";

        public static IServiceCollection AddMavimDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
                throw new System.ArgumentNullException(nameof(services));

            string apiEndPoint = configuration.GetSection(DatabaseInfoApiEndPoint).Value;

            services.Configure<AzDatabaseInfoAppConfigSettings>(options =>
            {
                options.ApiEndpoint = apiEndPoint;
            });

            services.AddHttpClient<IMavimDatabaseInfoClient, MavimDatabaseInfoClient>()
                    .AddPolicyHandler(PollyClient.GetRetryPolicy())
                    .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());

            services.AddScoped<IAzureSqlAccessTokenClient, AzureSqlAccessTokenClient>();

            return services;
        }

        public static IApplicationBuilder UseMavimDatabaseMiddleware(this IApplicationBuilder app, bool isDevelopment)
        {
            app.UseMiddleware<MavimDatabaseMiddleware>(isDevelopment);

            return app;
        }
    }
}
