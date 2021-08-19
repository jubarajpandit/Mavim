using Microsoft.AspNetCore.Builder;

namespace Mavim.Manager.Api.Connect.Write.Extensions
{
    /// <summary>
    /// Corse Extension
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Configures the cors.
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
