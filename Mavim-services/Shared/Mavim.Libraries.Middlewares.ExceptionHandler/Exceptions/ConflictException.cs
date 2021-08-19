namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class ConflictException : CustomException
    {
        /// <summary>
        /// Handle Conflic Exception
        /// </summary>
        /// <param name="message"></param>
        public ConflictException(string message) : base(message) { }
    }
}
