using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Catalog.Extensions
{
    public static class AuthorizationExtension
    {
        private const string ApplicationId = "Mavim:CatalogSettings:ApplicationId";
        private const string Tenant = "Mavim:CatalogSettings:TenantId";

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string tenant = configuration.GetSection(Tenant).Value;

            services.AddAuth(applicationId, tenant, isDevelopment);

            return services;
        }
    }
}