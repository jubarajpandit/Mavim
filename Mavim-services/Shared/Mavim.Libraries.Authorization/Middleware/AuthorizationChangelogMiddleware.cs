using Mavim.Libraries.Authorization.Configuration;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Utils.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Middleware
{
    class AuthorizationChangelogMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationChangelogMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(
            HttpContext context,
            IAuthorizationClient client,
            IOptionsSnapshot<AzAuthorizationAppConfigSettings> options
        )
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (options is null) throw new ArgumentNullException(nameof(options));

            if (IsUpdateMethod(context))
            {
                string requestUri = options.Value.ApiEndpoint;

                IAuthorization authorization = await client.GetAuthorization(requestUri);

                if (!authorization.IsAdmin)
                    throw new AuthorizationException(Logging.UNAUTHORIZED);
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
        private bool IsUpdateMethod(HttpContext context)
        {
            return context.Request.Method == "PATCH"; // WebRequestMethods.Http.Patch missing
        }
    }
}
