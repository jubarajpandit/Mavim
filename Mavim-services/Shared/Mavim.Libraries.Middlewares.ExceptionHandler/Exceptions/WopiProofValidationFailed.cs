namespace Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions
{
    public class WopiProofValidationFailed : CustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WopiProofValidationFailed"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WopiProofValidationFailed(string message) : base(message) { }
    }
}
