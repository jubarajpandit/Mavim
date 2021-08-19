using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mavim.Manager.Api.ChangelogTitle.Extensions
{
    public static class NewtonSoftJsonSerializerSettingsExtensions
    {
        /// <summary>
        /// Configures the newtonsoft json serializer settings.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void ConfigureNewtonsoftJsonSerializerSettings(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Formatting = Formatting.None;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                        options.SerializerSettings.Converters.Add(new StringEnumConverter
                        {
                            AllowIntegerValues = false,
                            NamingStrategy = new CamelCaseNamingStrategy()
                        });
                    });
        }
    }
}
