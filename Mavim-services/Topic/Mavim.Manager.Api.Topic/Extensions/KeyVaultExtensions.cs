using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mavim.Manager.Api.Topic.Extensions
{
    /// <summary>
    /// KeyVaultExtensions
    /// </summary>
    public static class KeyVaultExtensions
    {
        private const string KeyVaultKey = "Mavim:TopicSettings:KeyVaultName";
        private const string AzureTenantIdKey = "Mavim:TopicSettings:TenantId";
        private const string ApplicationIdKey = "Mavim:TopicSettings:KeyVaultAppId";
        private const string ApplicationSecretKey = "Mavim:TopicSettings:KeyVaultAppSecret";

        /// <summary>
        /// AddKeyVaultSecrets
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isDevelopment"></param>
        /// <returns></returns>
        public static IServiceCollection AddKeyVaultSecrets(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string keyVaultName = configuration.GetSection(KeyVaultKey).Value;
            string azureTenantId = isDevelopment ? configuration.GetSection(AzureTenantIdKey).Value : string.Empty;
            string applicationId = isDevelopment ? configuration.GetSection(ApplicationIdKey).Value : string.Empty;
            string applicationSecret = isDevelopment ? configuration.GetSection(ApplicationSecretKey).Value : string.Empty;

            var options = new SecretClientOptions
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            services.AddSingleton(sc => new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"), new DefaultAzureCredential(), options));

            return services;
        }
    }
}
