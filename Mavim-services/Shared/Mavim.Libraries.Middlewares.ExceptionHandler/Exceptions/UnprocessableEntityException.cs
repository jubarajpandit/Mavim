namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class UnprocessableEntityException : CustomException
    {
        /// <summary>
        /// Handle Bad request exception
        /// </summary>
        /// <param name="message"></param>
        public UnprocessableEntityException(string message) : base(message) { }

        /// <summary>
        /// Handle Bad request exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode">the inner error code</param>
        public UnprocessableEntityException(string message, int errorCode) : base(message, errorCode) { }
    }
}
