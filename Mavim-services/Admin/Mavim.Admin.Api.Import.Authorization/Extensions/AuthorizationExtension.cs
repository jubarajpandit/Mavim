using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Mavim.Admin.Api.Import.Authorization.Extensions
{
    public static class AuthorizationExtension
    {
        private const string ApplicationId = "Mavim:AdminImportAuthSettings:ApplicationId";
        private const string Tenant = "Mavim:AdminImportAuthSettings:TenantId";

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string tenant = configuration.GetSection(Tenant).Value;
            TokenValidationParameters parms = new TokenValidationParameters
            {
                ValidIssuer = $"https://login.microsoftonline.com/{tenant}/v2.0",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            services.AddAuth(applicationId, tenant, isDevelopment, validationParms: parms);

            return services;
        }
    }
}