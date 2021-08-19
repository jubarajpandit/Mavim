namespace Mavim.Manager.Api.Utils.Constants.AzureConfiguration
{
    public static class AzAppConfigSettingsConstants
    {
        //Mavim Settings to read from AZ App Configuration
        public static readonly string MAVIM_SENTINEL = "Mavim:Sentinel";
        public static readonly string MAVIM_DATABASEINFO_SETTINGS = "Mavim:DatabaseInfoSettings";
        public static readonly string MAVIM_CATALOG_SETTINGS = "Mavim:CatalogSettings";
        public static readonly string MAVIM_WOPI_SETTINGS = "Mavim:WopiSettings";
        public static readonly string MAVIM_CHANGELOG_SETTINGS = "Mavim:ChangelogSettings";

        //Configuration Name to read from user secrets
        public static readonly string DEV_AZ_APP_CONFIG = "AzAppConfig:ConnectionString:Dev";
        public static readonly string PROD_AZ_APP_CONFIG = "AzAppConfig:ConnectionString:Prod";
    }
}
