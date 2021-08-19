using Mavim.Libraries.Authorization.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Topic.Extensions
{
    /// <summary>
    /// AuthorizationExtensions
    /// </summary>
    public static class AuthorizationExtensions
    {
        private const string ApplicationIdKey = "Mavim:TopicSettings:ApplicationId";
        private const string TenantKey = "Mavim:TopicSettings:TenantId";
        private const string AuthorizationApiEndPointKey = "Mavim:TopicSettings:AuthApiEndPoint";

        /// <summary>
        /// AddAuthorization
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isDevelopment"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string applicationId = configuration.GetSection(ApplicationIdKey).Value;
            string tenant = configuration.GetSection(TenantKey).Value;

            services.AddAuth(applicationId, tenant, isDevelopment)
                    .AddMavimDatabase(configuration)
                    .AddAuthorizationTopic(configuration, AuthorizationApiEndPointKey);

            return services;
        }
    }
}
