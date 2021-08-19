using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.FeatureFlag.Extensions
{
    public static class AuthorizationHandlerExtension
    {
        private const string ApplicationId = "Mavim:FeatureFlagSettings:ApplicationId";
        private const string Tenant = "Mavim:FeatureFlagSettings:TenantId";

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string tenant = configuration.GetSection(Tenant).Value;

            services.AddAuth(applicationId, tenant, isDevelopment);

            return services;
        }
    }
}