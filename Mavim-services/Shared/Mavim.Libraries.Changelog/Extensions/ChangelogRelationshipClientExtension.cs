using Mavim.Libraries.Authorization.Clients;
using Mavim.Libraries.Changelog.Clients;
using Mavim.Libraries.Changelog.Configuration;
using Mavim.Libraries.Changelog.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Libraries.Changelog.Extensions
{
    public static class ChangelogRelationshipClientExtension
    {
        private const string ChangelogRelationshipApiEndPoint = "Mavim:ChangelogRelationshipPublicSettings:InternalApiEndPoint";

        public static IServiceCollection AddChangelogRelationshipClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
                throw new System.ArgumentNullException(nameof(services));

            string apiEndPoint = configuration.GetSection(ChangelogRelationshipApiEndPoint).Value;

            if (string.IsNullOrWhiteSpace(apiEndPoint))
                throw new System.ArgumentException("internal apiEndpoint empty in supplied services");

            services.Configure<AzChangelogRelationshipAppConfigSettings>(options =>
            {
                options.ApiEndpoint = apiEndPoint;
            });

            services.AddHttpClient<IChangelogRelationshipClient, ChangelogRelationshipClient>()
                    .AddPolicyHandler(PollyClient.GetRetryPolicy())
                    .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());

            return services;
        }
    }
}
