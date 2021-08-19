using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.ChangelogField.Extensions
{
    public static class AuthorizationHandlerExtension
    {
        private const string ApplicationId = "Mavim:ChangelogFieldSettings:ApplicationId";
        private const string Tenant = "Mavim:ChangelogFieldSettings:TenantId";
        private const string AuthorizationApiEndPointKey = "Mavim:ChangelogFieldSettings:AuthApiEndPoint";

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string tenant = configuration.GetSection(Tenant).Value;

            services.AddAuth(applicationId, tenant, isDevelopment)
                .AddAuthorizationChangelog(configuration, AuthorizationApiEndPointKey);

            return services;
        }
    }
}