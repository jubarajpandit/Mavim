using System;

namespace Mavim.Libraries.Authorization.Interfaces
{
    public interface IMavimDatabaseInfo
    {
        /// <summary>
        /// Database ID
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Azure AD Tenant ID
        /// </summary>
        Guid TenantId { get; set; }

        /// <summary>
        /// Name that will show to select database
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Database connectionstring
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Database schema
        /// </summary>
        string Schema { get; set; }

        /// <summary>
        /// Tenant id to connect external database
        /// This tenant contains the applicationID
        /// </summary>
        public Guid? ApplicationTenantId { get; set; }


        /// <summary>
        /// Azure Ad App id to connect external databases
        /// </summary>
        Guid? ApplicationId { get; set; }

        /// <summary>
        /// Azure Ad App Secret to connect external databases
        /// Use this key to get the app secret from our KeyVault
        /// </summary>
        Guid? ApplicationSecretKey { get; set; }

        /// <summary>
        /// This boolean is to define if a database is internal of external
        /// If "true" the database is internal and we use management identity
        /// If "false" the database is external and we use service principal
        /// </summary>
        bool IsInternalDatabase { get; set; }
    }
}
