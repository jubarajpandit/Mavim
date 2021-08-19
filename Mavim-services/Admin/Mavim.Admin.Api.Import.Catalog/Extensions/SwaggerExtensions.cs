using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Mavim.Admin.Api.Import.Catalog.Extensions
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
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Admin Import Catalog api", Version = "v1" });
            });
        }

        /// <summary>
        /// Uses the swagger and swagger UI.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseSwaggerAndSwaggerUi(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                // Only for development testing to get v2 for PowerApps.
                c.SerializeAsV2 = false;
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/" } };
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin Import Catalog api v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
