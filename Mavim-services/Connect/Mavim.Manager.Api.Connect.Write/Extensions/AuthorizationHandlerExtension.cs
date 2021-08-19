using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Authorization.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Connect.Write.Extensions
{
    /// <summary>
    /// Extension Authorization Handler
    /// </summary>
    public static class AuthorizationHandlerExtension
    {
        private const string ApplicationId = "Mavim:ConnectWriteSettings:ApplicationId";
        private const string ApplicationIdInternal = "Mavim:ConnectWriteSettings:ApplicationIdInternal";
        private const string TenantId = "Mavim:ConnectWriteSettings:TenantId";

        /// <summary>
        /// Internal Schema Name
        /// </summary>
        public const string InternalSchemaName = "Internal";

        /// <summary>
        /// Add Authorization by OAuth 2
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isDevelopment"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationId).Value;
            string applicationIdInternal = configuration.GetSection(ApplicationIdInternal).Value;
            string tenantId = configuration.GetSection(TenantId).Value;
            TokenValidationParameters validationParms = new()
            {
                ValidIssuer = $"https://login.microsoftonline.com/{tenantId}/v2.0",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            services.AddAuth(new List<AuthSchema>
            {
                new DefaultAuthSchema {
                    Audience = applicationId,
                    TenantId = tenantId
                },
                new AuthSchema
                {
                    Name = InternalSchemaName,
                    Audience = applicationIdInternal,
                    TenantId = tenantId,
                    ValidationParms = validationParms
                }
            }, isDevelopment);

            return services;
        }
    }
}