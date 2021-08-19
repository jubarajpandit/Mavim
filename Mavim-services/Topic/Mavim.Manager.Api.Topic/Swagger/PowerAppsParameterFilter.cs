using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Mavim.Manager.Api.Topic.Swagger
{
    internal class PowerAppsParameterFilter : IParameterFilter
    {
        readonly IServiceScopeFactory _serviceScopeFactory;

        public PowerAppsParameterFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter == null) return;

            ApplyParameterUrlEncodingDefinition(parameter);
            ApplyParameterEnumDefinition(parameter, context);
        }

        /// <summary>
        /// Adds to all parameters the Microsoft Power Platform Url single encoding to the definition
        /// </summary>
        /// <param name="parameter"></param>
        private static void ApplyParameterUrlEncodingDefinition(OpenApiParameter parameter)
        {
            parameter.Extensions.Add("x-ms-url-encoding", new OpenApiString("single"));
        }

        /// <summary>
        /// Adds for every enum the OpenAPI enum definition to the API method parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="context"></param>
        private void ApplyParameterEnumDefinition(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (context.ApiParameterDescription.Type.IsEnum)
            {
                parameter.Schema.Format = null;
                parameter.Schema.Type = typeof(String).Name.ToLower();
                parameter.Schema.Enum.Clear();

                Enum.GetNames(context.ApiParameterDescription.Type)
                    .ToList()
                    .ForEach(n => parameter.Schema.Enum.Add(new OpenApiString(n)));
            }
        }
    }
}