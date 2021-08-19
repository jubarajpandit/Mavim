using Mavim.Libraries.Authorization.Clients;
using Mavim.Libraries.Changelog.Clients;
using Mavim.Libraries.Changelog.Configuration;
using Mavim.Libraries.Changelog.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Libraries.Changelog.Extensions
{
    public static class ChangelogTitleClientExtension
    {
        private const string ChangelogApiEndPoint = "Mavim:ChangelogTitlePublicSettings:InternalApiEndPoint";

        public static IServiceCollection AddChangelogTitleClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
                throw new System.ArgumentNullException(nameof(services));

            string apiEndPoint = configuration.GetSection(ChangelogApiEndPoint).Value;

            if (string.IsNullOrWhiteSpace(apiEndPoint))
                throw new System.ArgumentException("internal apiEndpoint empty in supplied services");

            services.Configure<AzChangelogTitleAppConfigSettings>(options =>
            {
                options.ApiEndpoint = apiEndPoint;
            });

            services.AddHttpClient<IChangelogTitleClient, ChangelogTitleClient>()
                    .AddPolicyHandler(PollyClient.GetRetryPolicy())
                    .AddPolicyHandler(PollyClient.GetCircuitBreakerPatternPolicy());

            return services;
        }
    }
}
