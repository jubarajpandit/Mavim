using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Middleware
{
    public class CatchAllExceptionHandler : ExceptionHandler
    {
        public CatchAllExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory) : base(next, loggerFactory) { }

        public override async Task<bool> SetKnownDefaultExceptionTypeResponse(HttpContext context, Exception ex)
        {
            await SetResponse(context, ex, HttpStatusCode.InternalServerError, LogLevel.Error);

            return await Task.FromResult(true);
        }

        public override async Task<bool> SetKnownExceptionTypeResponse(HttpContext context, CustomException ex)
        {
            await SetResponse(context, ex, HttpStatusCode.InternalServerError, LogLevel.Error);

            return await Task.FromResult(true);
        }
    }
}
