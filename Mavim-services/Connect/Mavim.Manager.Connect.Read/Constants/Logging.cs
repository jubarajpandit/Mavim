namespace Mavim.Manager.Connect.Read.Constants
{
    public static class Logging
    {
        public const string NOT_ALLOWED = "You are not allowed to make this request.";
        public const string COMPANY_NOT_FOUND = "Could not find the company with your user.";
        public const string INCORRECT_AGGREGATEID = "Supplied object contains an invalid aggregateId: {0}, expected aggregateId: {1}";
        public const string INCORRECT_MODELVERSION = "Supplied object contains an invalid modelversion: {0}, expected modelversion: {1}";
        public const string INVALID_GROUPID = "Supplied groupId is invalid: {0}";
        public const string GROUP_NOT_FOUND = "Could not find the specified group with id: {0}.";
        public const string USER_NOT_FOUND = "Could not find user with id: {0}.";
        public const string COMPANY_ALREADY_EXISTS = "Company with guid {0} already exists.";
        public const string USER_ALREADY_EXISTS = "User with guid {0} already exists.";
        public const string GROUP_ALREADY_EXISTS = "Group with guid {0} already exists.";
    }
}
