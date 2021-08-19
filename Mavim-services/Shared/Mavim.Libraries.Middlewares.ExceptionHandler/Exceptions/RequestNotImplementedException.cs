namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class RequestNotImplementedException : CustomException
    {
        /// <summary>
        /// Handle Authorization request exception
        /// </summary>
        /// <param name="message"></param>
        public RequestNotImplementedException(string message) : base(message) { }
    }
}
