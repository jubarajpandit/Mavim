using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.WopiValidator.WopiProofKeyValidator
{
    public class WopiValidationMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>Initializes a new instance of the <see cref="WopiValidationMiddleware"/> class.</summary>
        /// <param name="next">The next.</param>
        /// <exception cref="ArgumentNullException">next</exception>
        public WopiValidationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context, IProofKeyValidator ProofKeyValidator)
        {
            await ProofKeyValidator.ValidateWopiProofKey(context);

            await _next(context);
        }
    }
}
