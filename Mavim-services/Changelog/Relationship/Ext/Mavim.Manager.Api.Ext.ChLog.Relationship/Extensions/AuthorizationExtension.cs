using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Ext.ChLog.Relationship.Extensions
{
    public static class AuthorizationExtension
    {
        private const string ApplicationId = "Mavim:ChangelogRelationshipPublicSettings:ApplicationId";
        private const string Tenant = "Mavim:ChangelogRelationshipPublicSettings:TenantId";
        private const string AuthorizationApiEndPointKey = "Mavim:TopicSettings:AuthApiEndPoint";

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