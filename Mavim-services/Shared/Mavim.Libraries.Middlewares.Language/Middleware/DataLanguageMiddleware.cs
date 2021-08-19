using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.Language.Middleware
{
    public class DataLanguageMiddleware
    {
        private const string PathLanguageKey = "dataLanguage";
        private const string Dutch = "nl";
        private const string English = "en";
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="MavimDatabaseMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="secretClient">The secret client.</param>
        /// <exception cref="ArgumentNullException">
        /// next
        /// or
        /// secretClient
        /// </exception>
        public DataLanguageMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">
        /// context
        /// or
        /// dataAccess
        /// </exception>
        /// <exception cref="RequestNotFoundException">Database not found</exception>
        public async Task Invoke(HttpContext context, IDataLanguage dataLanguage)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            object language = context.GetRouteValue(PathLanguageKey);
            if (language != null)
                dataLanguage.Type = Map(language.ToString().ToLower());

            await _next.Invoke(context);
        }

        private DataLanguageType Map(string value) =>
            value switch
            {
                Dutch => DataLanguageType.Dutch,
                English => DataLanguageType.English,
                _ => throw new RequestNotFoundException("DataLanguage not supported")
            };
    }
}
