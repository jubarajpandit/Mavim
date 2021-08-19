using System;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class JwtAuthenticationFailed : CustomException
    {
        /// <summary>
        /// Handle Jwt authentication failed request Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException">Pass the inner exception</param>
        public JwtAuthenticationFailed(string message, Exception innerException) : base(message, innerException) { }
    }
}
