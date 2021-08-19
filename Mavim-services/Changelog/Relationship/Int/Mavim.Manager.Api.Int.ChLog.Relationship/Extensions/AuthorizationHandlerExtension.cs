using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Extensions
{
    public static class AuthorizationHandlerExtension
    {
        private const string ApplicationId = "Mavim:ChangelogRelationSettings:ApplicationId";
        private const string Tenant = "Mavim:ChangelogRelationSettings:TenantId";
        private const string AuthorizationApiEndPointKey = "Mavim:ChangelogRelationSettings:AuthApiEndPoint";

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