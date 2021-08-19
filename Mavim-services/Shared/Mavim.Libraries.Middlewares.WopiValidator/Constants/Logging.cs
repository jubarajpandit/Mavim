namespace Mavim.Libraries.Middlewares.WopiValidator.Constants
{
    static class Logging
    {
        public const string CANNOT_ACCESS_TOKEN_RESPONSE = "Cannot get access token from response";
        public const string NO_VALID_JWT_TOKEN_FORMAT = "argument '{0}' must be a valid JWT security token";
        public const string MISSING_HEADER_FORMAT = "Missing '{0}' Header.";
        public const string MISSING_QUERY_STRING = "Missing '{0}' query string.";
        public const string RESPONSE_FORMAT = "Response status: '{0}'. Response message: '{1}'";
        public const string JWT_EXPIRED = "JWT Token expired";
        public const string AUTH_TOKEN_FAILED = "Authentication failed for the specified token.";
        public const string POLLY_RETRY_MESSAGE_FORMAT = "Delaying for {0}ms, then making retry {1}.";
        public const string RETRIEVE_TOKEN_FAILED_FORMAT = "Retrieving token failed, reason: {0}";
    }
}
