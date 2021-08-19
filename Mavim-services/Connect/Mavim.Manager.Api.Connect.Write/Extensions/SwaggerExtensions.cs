using Mavim.Manager.Api.Connect.Write.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Mavim.Manager.Api.Connect.Write.Extensions
{
    /// <summary>
    ///     Swagger Extension
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        ///     Adds the swagger generator.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddSwaggerGenerator(this IServiceCollection services)
        {
            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
        }

        /// <summary>
        ///     Uses the swagger and swagger UI.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider"></param>
        public static void UseSwaggerAndSwaggerUi(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options => AddSwaggerUiOptions(options, provider));
        }

        private static void AddSwaggerUiOptions(SwaggerUIOptions options, IApiVersionDescriptionProvider provider)
        {
            // build a swagger endpoint for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            options.RoutePrefix = string.Empty;
        }
    }
}