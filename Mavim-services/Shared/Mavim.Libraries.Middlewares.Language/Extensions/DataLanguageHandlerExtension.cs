using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Libraries.Middlewares.Language.Middleware;
using Mavim.Libraries.Middlewares.Language.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mavim.Libraries.Middlewares.Language.Extensions
{

    public static class DataLanguageHandlerExtension
    {

        public static IServiceCollection AddDataLanguage(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddScoped<IDataLanguage, DataLanguage>();

            return serviceCollection;
        }

        public static IApplicationBuilder UseDataLanguage(this IApplicationBuilder app)
        {
            app.UseMiddleware<DataLanguageMiddleware>();

            return app;
        }

    }

}