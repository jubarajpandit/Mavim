using System;

namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    /// <summary>
    /// CustomException class
    /// </summary>
    public abstract class CustomException : Exception
    {
        public int ErrorCode { get; set; }

        /// <summary>
        /// CustomException constructor
        /// </summary>
        /// <param name="message"></param>
        public CustomException(string message) : base(message) { }

        /// <summary>
        /// CustomException constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CustomException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// CustomException constructor
        /// </summary>
        /// <param name="message">the error message</param>
        /// <param name="errorCode">the internal error code</param>
        public CustomException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
