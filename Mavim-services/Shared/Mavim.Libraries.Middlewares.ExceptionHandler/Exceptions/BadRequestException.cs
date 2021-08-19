namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class BadRequestException : CustomException
    {
        /// <summary>
        /// Handle Bad request exception
        /// </summary>
        /// <param name="message"></param>
        public BadRequestException(string message) : base(message) { }
    }
}
