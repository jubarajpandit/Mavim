using Mavim.Manager.Api.Utils.AzAppConfiguration;
using Mavim.Manager.Api.Utils.Constants.AzureConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Libraries.Middlewares.WopiValidator.Extensions
{
    public static class AzAppConfigurationsExtensions
    {
        /// <summary>
        /// Loads the wopi azure application configuration settings.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void LoadWopiAzureAppConfigSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WopiSettings>(configuration.GetSection(AzAppConfigSettingsConstants.MAVIM_WOPI_SETTINGS));
        }
    }
}
