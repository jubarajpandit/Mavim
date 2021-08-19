using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Mavim.Admin.Api.Import.Catalog.Extensions
{
    public static class AuthorizationExtension
    {
        private const string ApplicationId = "Mavim:AdminImportCatalogSettings:ApplicationId";
        private const string Tenant = "Mavim:AdminImportCatalogSettings:TenantId";

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string tenant = configuration.GetSection(Tenant).Value;
            TokenValidationParameters validationParms = new TokenValidationParameters
            {
                ValidIssuer = $"https://login.microsoftonline.com/{tenant}/v2.0",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            services.AddAuth(applicationId, tenant, isDevelopment, validationParms: validationParms);

            return services;
        }
    }
}