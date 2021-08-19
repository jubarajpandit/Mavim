namespace Mavim.Manager.Authorization.Read.Constants
{
    public static class Logging
    {
        public const string NOT_ALLOWED = "You are not allowed to make this request.";
        public const string COMPANY_NOT_FOUND = "Could not find the company with your user.";
        public const string INCORRECT_AGGREGATEID = "Supplied object contains an invalid aggregateId: {0}, expected aggregateId: {1}";
        public const string INCORRECT_MODELVERSION = "Supplied object contains an invalid modelversion: {0}, expected modelversion: {1}";
        public const string INVALID_ROLEID = "Supplied roleId is invalid: {0}";
        public const string ROLE_NOT_FOUND = "Could not find the specified role with id: {0}.";
        public const string USER_NOT_FOUND = "Could not find user with id: {0}.";
        public const string COMPANY_ALREADY_EXISTS = "Company with guid {0} already exists.";
        public const string USER_ALREADY_EXISTS = "User with guid {0} already exists.";
        public const string ROLE_ALREADY_EXISTS = "Role with guid {0} already exists.";
    }
}
