using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mavim.Manager.Api.Topic.Swagger
{
    internal class PowerAppsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null) return;

            // Handle Metadata attribute
            foreach (OpenApiParameter parameter in operation.Parameters)
            {
                SetPowerAppsSummary(parameter, parameter.Description);
            }
        }

        /// <summary>
        /// Adds for each operation the specific for the Microsoft Power Platform Connector the summary
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="friendlyName"></param>
        private static void SetPowerAppsSummary(OpenApiParameter parameter, string friendlyName)
        {
            if (!parameter.Extensions.ContainsKey("x-ms-summary"))
            {
                parameter.Extensions.Add("x-ms-summary", new OpenApiString(friendlyName));
            }
        }
    }
}