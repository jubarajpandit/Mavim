using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace Mavim.Manager.Api.Connect.Read.Extensions
{
    /// <summary>
    /// This class allows adding the redis cache connection to the service
    /// </summary>
    public static class RedisCacheConnectionExtension
    {
        private const string _keyVaultKey = "Mavim:ConnectReadSettings:KeyVaultName";
        private const string _applicationSecretKey = "Mavim:ConnectReadSettings:keyVaultAppSecret";
        private const string _redisConnectionString = "Mavim:ConnectReadSettings:RedisConnectionString";

        /// <summary>
        /// Use this method in the startup to add the redis cache connection to the service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isDevelopment"></param>
        public static void AddRedisCacheConnection(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            if (isDevelopment)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    // for configuration options see:
                    // https://stackexchange.github.io/StackExchange.Redis/Configuration.html
                    options.ConfigurationOptions = ConfigurationOptions.Parse(configuration.GetSection(_redisConnectionString).Value);
                    options.InstanceName = "Connect_";
                });
            }
            else
            {
                string keyVaultName = configuration.GetSection(_keyVaultKey).Value;
                if (string.IsNullOrWhiteSpace(keyVaultName)) throw new InvalidOperationException("No keyvaultkey found in appconfig");
                string secretKey = configuration.GetSection(_applicationSecretKey).Value;
                if (string.IsNullOrWhiteSpace(secretKey)) throw new InvalidOperationException("No secretkey found in appconfig");

                string redisPassword = GetRedisPasswordFromKeyVault(keyVaultName, secretKey);

                if (string.IsNullOrWhiteSpace(redisPassword))
                    throw new InvalidOperationException($"No redis password found in KeyVault with key {_applicationSecretKey}");

                services.AddStackExchangeRedisCache(options =>
                {
                    // for configuration options see:
                    // https://stackexchange.github.io/StackExchange.Redis/Configuration.html
                    options.ConfigurationOptions = ConfigurationOptions.Parse(configuration.GetSection(_redisConnectionString).Value);
                    options.ConfigurationOptions.Password = redisPassword;
                    options.InstanceName = "Connect_";
                });
            }
        }

        private static string GetRedisPasswordFromKeyVault(string keyVaultName, string secretKey)
        {
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

            var client = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(secretKey);

            return secret?.Value ?? string.Empty;
        }
    }
}
