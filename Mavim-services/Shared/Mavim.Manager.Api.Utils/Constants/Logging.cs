namespace Mavim.Manager.Api.Utils.Constants
{
    public static class Logging
    {
        public const string MISSING_HEADER_FORMAT = "Missing {0} Header.";
        public const string UNAUTHORIZED = "User {0} from tenant {1} is unauthorized for this action";
        public const string RETRIEVE_AUTHORIZATION_FAILED_FORMAT = "Retrieving authorization failed, reason: {0}";
        public const string POLLY_RETRY_MESSAGE_FORMAT = "Delaying for {0}ms, then making retry {1}.";
        public const string JWT_TOKEN_EXPIRED = "JWT Token expired";
        public const string NO_DATABASE_FOUND = "No Database information found with guid {0}";
    }
}
