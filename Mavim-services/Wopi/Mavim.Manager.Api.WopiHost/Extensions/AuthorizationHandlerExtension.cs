using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.WopiHost.Extensions
{
    public static class AuthorizationHandlerExtension
    {
        private const string ApplicationId = "Mavim:WopiSettings:ApplicationId";
        private const string Tenant = "Mavim:WopiSettings:TenantId";
        private const string AzConfigConnectionString = "Mavim:WopiSettings:ConnectionString";
        private const string AuthorizationApiEndPointKey = "Mavim:WopiSettings:AuthApiEndPoint";

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string tenant = configuration.GetSection(Tenant).Value;

            services.AddAuth(applicationId, tenant, isDevelopment, useQueryString: true)
                    .AddMavimDatabase(configuration)
                    .AddAuthorizationTopic(configuration, AuthorizationApiEndPointKey);

            return services;
        }
    }
}