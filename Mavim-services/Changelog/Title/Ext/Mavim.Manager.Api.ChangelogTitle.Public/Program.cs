using Azure.Identity;
using Mavim.Manager.Api.Utils.Constants.AzureConfiguration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Mavim.Manager.Api.ChangelogTitle.Public
{
    public class Program
    {

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Starting Changelog Title Public application....");
            host.Run();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    IConfiguration configuration = config.Build();
                    ILogger<Program> logger = GetLogger();
                    LogConfigurationSource(config, logger);

                    try
                    {
                        bool useLocalSettings = false;
                        IConfigurationSection useLocalSettingsConfig = configuration.GetSection("Mavim:UseLocalSettings");
                        if (useLocalSettingsConfig != null && useLocalSettingsConfig.Value?.Trim().Length > 0)
                            bool.TryParse(useLocalSettingsConfig.Value.ToLowerInvariant(), out useLocalSettings);

                        bool isLocal = hostingContext.HostingEnvironment.IsDevelopment();

                        if (!useLocalSettings)
                        {
                            config.AddAzureAppConfiguration(options =>
                            {
                                ConnectAppConfiguration(ref options, configuration, isLocal).ConfigureRefresh(refresh =>
                                {
                                    refresh.Register(key: AzAppConfigSettingsConstants.MAVIM_SENTINEL, refreshAll: true)
                                        .SetCacheExpiration(TimeSpan.FromSeconds(5));
                                }).UseFeatureFlags();
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error configuring Azure app configuration.");
                        throw;
                    }
                }).UseStartup<Startup>();
            });

        /// <summary>
        /// Connects the application configuration.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="isLocal">if set to <c>true</c> [is local].</param>
        /// <returns></returns>
        private static AzureAppConfigurationOptions ConnectAppConfiguration(ref AzureAppConfigurationOptions options, IConfiguration configuration, bool isLocal) => isLocal
            ? options.Connect(configuration[AzAppConfigSettingsConstants.DEV_AZ_APP_CONFIG])
            : options.Connect(
                new Uri(configuration[AzAppConfigSettingsConstants.PROD_AZ_APP_CONFIG]),
                new ManagedIdentityCredential()
            );


        /// <summary>
        /// Logs the configured json sources that are found in the configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="logger">The logger.</param>
        private static void LogConfigurationSource(IConfigurationBuilder config, ILogger<Program> logger)
        {
            foreach (IConfigurationSource source in config.Sources)
            {
                if (source is JsonConfigurationSource item)
                    logger.LogTrace($"Found Json config path: {item.Path}.");
            }
        }

        /// <summary>
        /// Gets a logger for the Program
        /// </summary>
        /// <returns></returns>
        private static ILogger<Program> GetLogger()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(c => c.ClearProviders().AddConsole())
                .BuildServiceProvider();
            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            return logger;
        }
    }
}
