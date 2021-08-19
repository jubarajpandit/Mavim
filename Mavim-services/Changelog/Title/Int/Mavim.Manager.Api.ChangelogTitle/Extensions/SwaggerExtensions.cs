using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.ChangelogTitle.Extensions
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds the swagger generator.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddSwaggerGenerator(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Changelog Title api", Version = "v1" });
            });
        }

        /// <summary>
        /// Uses the swagger and swagger UI.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseSwaggerAndSwaggerUi(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Changelog api v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
