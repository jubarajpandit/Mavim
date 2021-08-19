using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Middleware
{
    public class DefaultExceptionHandler : ExceptionHandler
    {
        public DefaultExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory) : base(next, loggerFactory) { }

        public override async Task<bool> SetKnownDefaultExceptionTypeResponse(HttpContext context, Exception ex)
        {
            bool isKnownType = true;
            switch (ex)
            {
                case NotImplementedException _:
                    await SetResponse(context, ex, HttpStatusCode.NotImplemented);
                    break;
                case SecurityTokenExpiredException _:
                    await SetResponse(context, ex, HttpStatusCode.Unauthorized);
                    break;
                default:
                    isKnownType = false;
                    break;
            }

            return await Task.FromResult(isKnownType);
        }

        public override async Task<bool> SetKnownExceptionTypeResponse(HttpContext context, CustomException ex)
        {
            return await Task.FromResult(false);
        }
    }
}
