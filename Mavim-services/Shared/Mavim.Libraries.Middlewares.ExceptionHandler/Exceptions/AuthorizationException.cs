namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class AuthorizationException : CustomException
    {
        /// <summary>
        /// Handle Authorization request exception
        /// </summary>
        /// <param name="message"></param>
        public AuthorizationException(string message) : base(message) { }
    }
}
