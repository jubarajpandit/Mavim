namespace Mavim.Manager.Connect.Read.Functions.Constants
{
    public static class Endpoints
    {
        public const string AddOrDeleteUsersToGroup = "internal/v1/Groups/{0}/users";

        public const string UpdateGroup = "internal/v1/Groups/{0}";
        public const string DisableGroup = "internal/v1/Groups/{0}/disable";
        public const string EnableGroup = "internal/v1/Groups/{0}/enable";
        public const string AddGroup = "internal/v1/Groups";

        public const string DisableUser = "internal/v1/Users/{0}/disable";
        public const string EnableUser = "internal/v1/Users/{0}/enable";
        public const string AddUser = "internal/v1/Users";

        public const string AddCompany = "internal/v1/Company";
    }
}
