using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Connect.Write.Extensions
{
    /// <summary>
    /// Extension Api Versioning
    /// </summary>
    public static class ApiVersioning
    {
        /// <summary>
        /// Adds api versioning.
        /// </summary>
        /// <param name="services"></param>
        public static void AddNetApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}
