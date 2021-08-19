using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Mavim.Manager.Connect.Write.ServiceBus.Client;
using Mavim.Manager.Connect.Write.ServiceBus.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mavim.Manager.Api.Connect.Write.Extensions
{
    /// <summary>
    /// Extension ServiceBus
    /// </summary>
    public static class ServiceBusExtensions
    {
        private const string ServiceBusConnectionKey = "Mavim:ConnectWriteSettings:ServiceBusConnectionKey";
        private const string ServiceBusQueueName = "Mavim:ConnectWriteSettings:ServiceBusQueueName";

        private const string ServiceBusBatchQueueName = "Mavim:ConnectWriteSettings:ServiceBusBatchQueueName";
        private const string ServiceBusBatchConnectionKey = "Mavim:ConnectWriteSettings:ServiceBusBatchConnectionKey";

        private const string KeyVaultKey = "Mavim:ConnectWriteSettings:KeyVaultName";

        /// <summary>
        /// Add Service Bus Queue Client
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isDevelopment"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBusAsync(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string secret = GetServiceBusConnectionString(configuration, ServiceBusConnectionKey, isDevelopment);
            string serviceBusQueueName = configuration.GetSection(ServiceBusQueueName).Value;

            string secretBatch = GetServiceBusConnectionString(configuration, ServiceBusBatchConnectionKey, isDevelopment);
            string serviceBusBatchQueueName = configuration.GetSection(ServiceBusBatchQueueName).Value;

            services.AddSingleton<IQueueClient>(e => new QueueClient(secret, serviceBusQueueName));
            services.AddSingleton<IBatchQueueClient>(e => new BatchQueueClient(secretBatch, serviceBusBatchQueueName));


            return services;
        }

        private static string GetServiceBusConnectionString(IConfiguration configuration, string key, bool isDevelopment)
        {
            if (isDevelopment)
                return configuration.GetSection(key).Value;
            else
            {
                string keyVaultName = configuration.GetSection(KeyVaultKey).Value;

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
                string serviceBusConnection = configuration.GetSection(key).Value;
                var keyVaultSecret = client.GetSecretAsync(serviceBusConnection).Result;
                return keyVaultSecret.Value.Value;
            }
        }
    }
}
