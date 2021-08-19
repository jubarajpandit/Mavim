using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mavim.Manager.Api.Topic.Swagger
{
    internal class PowerAppsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            AddConnectorMetadata(swaggerDoc);
        }

        /// <summary>
        /// Adds specific for Microsoft Power Platform the connector metadata
        /// </summary>
        /// <param name="swaggerDoc"></param>
        private static void AddConnectorMetadata(OpenApiDocument swaggerDoc)
        {
            swaggerDoc.Extensions.Add("x-ms-connector-metadata", new OpenApiArray {
                new OpenApiObject
                {
                    [ "propertyName" ] = new OpenApiString("Website"),
                    [ "propertyValue" ] = new OpenApiString("https://www.mavim.com/"),
                },
                new OpenApiObject
                {
                    [ "propertyName" ] = new OpenApiString("Privacy policy"),
                    [ "propertyValue" ] = new OpenApiString("https://trustcenter.mavim.com/"),
                },
                new OpenApiObject
                {
                    [ "propertyName" ] = new OpenApiString("Categories"),
                    [ "propertyValue" ] = new OpenApiString("Business Management;Collaboration"),
                },
            });
        }
    }
}