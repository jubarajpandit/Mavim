using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Middleware
{
    public class CommonExceptionHandler : ExceptionHandler
    {
        public CommonExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory) : base(next, loggerFactory) { }

        /// <summary>
        /// Returns the exception response asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public override async Task<bool> SetKnownExceptionTypeResponse(HttpContext context, CustomException ex)
        {
            bool isKnownType = true;
            switch (ex)
            {
                case BadRequestException _:
                    await SetResponse(context, ex, HttpStatusCode.BadRequest, errorCode: ex.ErrorCode);
                    break;
                case RequestNotFoundException _:
                    await SetResponse(context, ex, HttpStatusCode.NotFound, errorCode: ex.ErrorCode);
                    break;
                case ForbiddenRequestException _:
                    await SetResponse(context, ex, HttpStatusCode.Forbidden, errorCode: ex.ErrorCode);
                    break;
                case UnprocessableEntityException _:
                    await SetResponse(context, ex, HttpStatusCode.UnprocessableEntity, errorCode: ex.ErrorCode);
                    break;
                case ConflictException _:
                    await SetResponse(context, ex, HttpStatusCode.Conflict, errorCode: ex.ErrorCode);
                    break;
                case RequestNotImplementedException _:
                    await SetResponse(context, ex, HttpStatusCode.NotImplemented, errorCode: ex.ErrorCode);
                    break;
                default:
                    isKnownType = false;
                    break;
            }

            return await Task.FromResult(isKnownType);
        }
    }
}
