using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Middleware
{

    public abstract class ExceptionHandler
    {
        private readonly JsonSerializerOptions JsonSerializerOptions = new() { IgnoreNullValues = true };
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger(GetType().Name) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                if (!await SetKnownExceptionTypeResponse(context, ex))
                    throw;
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                if (!await SetKnownDefaultExceptionTypeResponse(context, ex))
                    throw;
            }
        }

        /// <summary>
        /// Returns the exception response asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public abstract Task<bool> SetKnownExceptionTypeResponse(HttpContext context, CustomException ex);

        /// <summary>
        /// Returns the exception response asynchronous.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public virtual async Task<bool> SetKnownDefaultExceptionTypeResponse(HttpContext context, Exception ex)
        {
            return await Task.FromResult(false);
        }

        /// <summary>
        /// Sets the response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="logLevel">The log level.</param>
        protected async Task SetResponse(HttpContext context, Exception ex, HttpStatusCode statusCode, LogLevel logLevel = LogLevel.Information, int? errorCode = null)
        {
            _logger.Log(logLevel, ex, ex.Message);
            string responseMessage = logLevel > LogLevel.Information ? string.Empty : JsonSerializer.Serialize(new { error = ex.Message, errorCode }, JsonSerializerOptions);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(responseMessage, System.Text.Encoding.UTF8);
        }
    }
}
