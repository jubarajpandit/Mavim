using Mavim.Libraries.Middlewares.ExceptionHandler.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Extensions
{
    public static class ExceptionHandlerExtension
    {
        public static IApplicationBuilder AddExceptionHandler(this IApplicationBuilder app, bool isDevelopment)
        {
            // API Error handling
            if (!isDevelopment)
            {
                // Always start with CatchAllExceptionHandler (desc order)
                app.UseMiddleware<CatchAllExceptionHandler>();
            }
            app.UseMiddleware<AuthExceptionHandler>();
            app.UseMiddleware<WopiExceptionHandler>();
            app.UseMiddleware<CommonExceptionHandler>();
            app.UseMiddleware<DefaultExceptionHandler>();

            return app;
        }
    }
}
