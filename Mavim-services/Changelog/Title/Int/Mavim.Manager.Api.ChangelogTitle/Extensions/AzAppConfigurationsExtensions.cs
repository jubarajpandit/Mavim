﻿using Mavim.Manager.Api.Utils.Constants.AzureConfiguration;
using Mavim.Manager.ChangelogTitle.DbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.ChangelogTitle.Extensions
{
    public static class AzAppConfigurationsExtensions
    {
        /// <summary>
        /// Gets the azure application settings.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static void GetAzureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TitleConnectionSettings>(configuration.GetSection(AzAppConfigSettingsConstants.MAVIM_CHANGELOG_SETTINGS));
        }
    }
}