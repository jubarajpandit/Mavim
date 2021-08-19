using Mavim.Manager.Api.Utils.Constants.AzureConfiguration;
using Mavim.Manager.MavimDatabaseInfo.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.MavimDatabaseInfo.Extensions
{
    public static class AzAppConfigurationsExtensions
    {
        /// <summary>
        /// Gets the azure application settings.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection GetAzureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<MavimDatabaseInfoConnectionSettings>(configuration.GetSection(AzAppConfigSettingsConstants.MAVIM_DATABASEINFO_SETTINGS));
        }

    }
}
