using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.ChangelogTitle.Public.Extensions
{
    public static class AuthorizationExtension
    {
        private const string ApplicationId = "Mavim:ChangelogTitlePublicSettings:ApplicationId";
        private const string Tenant = "Mavim:ChangelogTitlePublicSettings:TenantId";
        private const string AuthorizationApiEndPointKey = "Mavim:ChangelogTitlePublicSettings:AuthApiEndPoint";

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string tenant = configuration.GetSection(Tenant).Value;

            services.AddAuth(applicationId, tenant, isDevelopment)
                .AddAuthorizationChangelog(configuration, AuthorizationApiEndPointKey); ;

            return services;
        }
    }
}