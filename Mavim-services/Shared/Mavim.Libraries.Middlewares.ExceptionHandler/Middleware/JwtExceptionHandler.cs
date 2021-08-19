using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Middleware
{
    public class AuthExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthExceptionHandler"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AuthExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory) : base(next, loggerFactory) { }

        /// <summary>
        /// Returns the exception response asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public override async Task<bool> SetKnownExceptionTypeResponse(HttpContext context, CustomException ex)
        {
            bool isKnowType = true;
            switch (ex)
            {
                case JwtAuthenticationFailed _:
                    await SetResponse(context, ex, HttpStatusCode.Unauthorized, LogLevel.Error);
                    break;
                default:
                    isKnowType = false;
                    break;
            }
            return await Task.FromResult(isKnowType);
        }
    }
}
