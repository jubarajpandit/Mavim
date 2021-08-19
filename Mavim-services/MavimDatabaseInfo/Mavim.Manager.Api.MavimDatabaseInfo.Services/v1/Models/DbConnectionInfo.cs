using Mavim.Manager.Api.MavimDatabaseInfo.Services.Interfaces.v1;
using System;

namespace Mavim.Manager.Api.MavimDatabaseInfo.Services.v1.Models
{
    public class DbConnectionInfo : IDbConnectionInfo
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Azure AD Tenant ID
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Name that will show to select database
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Database connectionstring
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database schema
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Tenant id to connect external database
        /// This tenant contains the applicationID
        /// </summary>
        public Guid? ApplicationTenantId { get; set; }

        /// <summary>
        /// Azure Ad App id to connect external databases
        /// </summary>
        public Guid? ApplicationId { get; set; }

        /// <summary>
        /// Azure Ad App Secret to connect external databases
        /// Use this key to get the app secret from our KeyVault
        /// </summary>
        public Guid? ApplicationSecretKey { get; set; }

        /// <summary>
        /// This boolean is to define if a database is internal of external
        /// If "true" the database is internal and we use management identity
        /// If "false" the database is external and we use service principal
        /// </summary>
        public bool IsInternalDatabase { get; set; }

    }
}
