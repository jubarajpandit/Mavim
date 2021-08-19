using Mavim.Libraries.Authorization.Configuration;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Utils.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Middleware
{
    public class AuthorizationTopicMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _versionPath = "/v1/version";
        private const string _swaggerPath = "/swagger/v1/swagger.json";
        private const string _rootPath = "/";
        private ILogger<AuthorizationTopicMiddleware> _logger;

        public AuthorizationTopicMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(
            HttpContext context,
            IAuthorizationClient client,
            IJwtSecurityToken token,
            IOptionsSnapshot<AzAuthorizationAppConfigSettings> options,
            ILogger<AuthorizationTopicMiddleware> logger)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (options is null) throw new ArgumentNullException(nameof(options));

            _logger = logger;

            if (!SkipRoutes(context))
            {
                string requestUri = options.Value.ApiEndpoint;
                IAuthorization authorization = await client.GetAuthorization(requestUri);

                if (authorization == null)
                    throw new ForbiddenRequestException(string.Format(Logging.UNAUTHORIZED, token.Email, token.TenantId));

                if (IsMutationMethod(context) && authorization.Readonly)
                    throw new ForbiddenRequestException(string.Format(Logging.UNAUTHORIZED, token.Email, token.TenantId));
            }

            await _next(context);
        }

        /// <summary>
        /// Determines whether [is mutation method] [the specified context].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if [is mutation method] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMutationMethod(HttpContext context)
        {
            return context.Request.Method != WebRequestMethods.Http.Get;
        }

        private bool SkipRoutes(HttpContext context)
        {
            _logger.LogTrace($"Request Path: {context.Request.Path} : Method : {context.Request.Method}");
            return context.Request.Path.ToString().EndsWith(_rootPath) ||
                context.Request.Path.ToString().EndsWith(_versionPath) ||
                context.Request.Path.ToString().EndsWith(_swaggerPath);
        }
    }
}
