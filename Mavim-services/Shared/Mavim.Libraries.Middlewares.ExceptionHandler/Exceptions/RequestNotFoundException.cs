namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class RequestNotFoundException : CustomException
    {
        /// <summary>
        /// Handle request not found Exception
        /// </summary>
        /// <param name="message"></param>
        public RequestNotFoundException(string message) : base(message) { }
    }
}
