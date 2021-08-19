using Microsoft.AspNetCore.Builder;

namespace Mavim.Manager.Api.Topic.Extensions
{
    /// <summary>
    /// CorsExtensions
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Configures the cors
        /// </summary>
        /// <param name="app">The application.</param>
        public static void ConfigureCors(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
        }
    }
}
