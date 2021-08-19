using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mavim.Admin.Api.Import.Catalog.Extensions
{
    public static class KeyVaultExtensions
    {
        private const string KeyVaultKey = "Mavim:AdminImportCatalogSettings:KeyVaultName";
        private const string AzureTenantIdKey = "Mavim:AdminImportCatalogSettings:TenantId";
        private const string ApplicationIdKey = "Mavim:AdminImportCatalogSettings:KeyVaultAppId";
        private const string ApplicationSecretKey = "Mavim:AdminImportCatalogSettings:KeyVaultAppSecret";

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

            services.AddSingleton(sc => isDevelopment
                ? new SecretClient(
                    new Uri($"https://{keyVaultName}.vault.azure.net/"),
                    new ClientSecretCredential(azureTenantId, applicationId, applicationSecret),
                    options)
                : new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"), new DefaultAzureCredential(), options));

            return services;
        }
    }
}
