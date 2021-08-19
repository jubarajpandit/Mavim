using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Authorization.Extensions
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
            //This has to be fixed when we have the class for the AzAuthorizationAppConfigSettings that corresponds to the MAVIM_AUTH_SETTINGS
            //services.Configure<AzAuthorizationAppConfigSettings>(configuration.GetSection(AzAppConfigSettingsConstants.MAVIM_AUTH_SETTINGS));
        }
    }
}
