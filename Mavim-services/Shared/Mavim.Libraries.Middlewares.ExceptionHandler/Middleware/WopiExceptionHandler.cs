using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Middleware
{
    public class WopiExceptionHandler : ExceptionHandler
    {
        public WopiExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory) : base(next, loggerFactory) { }

        public override async Task<bool> SetKnownExceptionTypeResponse(HttpContext context, CustomException ex)
        {
            bool isKnowType = true;
            switch (ex)
            {
                case WopiProofValidationFailed _:
                    // When proof validation fails, we need to return 500 internal server error with not much exception details in it.
                    // See url https://wopi.readthedocs.io/en/latest/scenarios/proofkeys.html#proof-keys for more information
                    await SetResponse(context, ex, HttpStatusCode.InternalServerError, LogLevel.Error);
                    break;
                default:
                    isKnowType = false;
                    break;
            }

            return await Task.FromResult(isKnowType);
        }
    }
}
