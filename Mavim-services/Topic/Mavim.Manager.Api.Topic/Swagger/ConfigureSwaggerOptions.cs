using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace Mavim.Manager.Api.Topic.Swagger
{
    /// <summary>
    ///     Configures the Swagger generation options.
    /// </summary>
    /// <remarks>
    ///     This allows API versioning to define a Swagger document per API version after the
    ///     <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
    /// </remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
        /// </summary>
        /// <param name="provider">
        ///     The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
        ///     documents.
        /// </param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = AppContext.BaseDirectory;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        /// <summary>
        /// Configure Swagger options.
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            options.EnableAnnotations();

            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

            // add a custom operation filter which sets default values
            options.SchemaFilter<SwaggerSchemaFilter>();
            options.DocumentFilter<PowerAppsDocumentFilter>();
            options.OperationFilter<SwaggerOperationFilter>();
            options.OperationFilter<PowerAppsOperationFilter>();
            options.ParameterFilter<PowerAppsParameterFilter>();

            // integrate xml comments
            options.IncludeXmlComments(XmlCommentsFilePath);

            // custom coperation ids
            options.CustomOperationIds(apiDesc =>
            {
                return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
            });
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var title = "Mavim iMprove Api";

            var info = new OpenApiInfo
            {
                Title = title,
                Version = description.ApiVersion.ToString(),
                Description = $"With the {title} you are able to build a digital twin of your organization by visualizing the relationships among people, process and technology and creates a map of business operations to help you monitor change, measure impact, and make informed business decisions to help you reach your destination. Our cloud software makes use of open standards for integration with third party applications, enabling simple adoption into any company’s technology stack.",
                Contact = new OpenApiContact
                {
                    Name = "Mavim servicedesk",
                    Email = "servicedesk@mavim.com",
                    Url = new Uri("https://my.mavim.com"),
                },
            };

            if (description.IsDeprecated) info.Title += " (deprecated)";

            return info;
        }
    }
}