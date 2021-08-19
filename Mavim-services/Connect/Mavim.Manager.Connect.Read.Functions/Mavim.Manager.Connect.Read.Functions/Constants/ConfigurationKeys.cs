namespace Mavim.Manager.Connect.Read.Functions.Constants
{
    public static class ConfigurationKeys
    {
        public const string AzureTenantId = "AZURE_TENANT_ID";

        public const string ConnectReadClientApplicationId = "ConnectReadClientApplicationId";
        public const string ConnectReadClientApplicationSecret = "ConnectReadClientApplicationSecret";
        public const string ConnectReadClientScope = "ConnectReadClientScope";
        public const string ConnectReadClientBaseUrl = "ConnectReadClientBaseUrl";
        public const string GrantType = "client_credentials";
        public const string AuthUrl = "https://login.microsoftonline.com/{0}/oauth2/v2.0/token";
    }
}
