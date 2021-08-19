namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class ForbiddenRequestException : CustomException
    {
        /// <summary>
        /// Handle Forbidden request exception
        /// </summary>
        /// <param name="message"></param>
        public ForbiddenRequestException(string message) : base(message) { }
    }
}
