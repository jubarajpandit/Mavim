using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mavim.Manager.MavimDatabaseInfo.EFCore.Models
{
    public class DbConnectionInfo
    {
        /// <summary>
        /// Database ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Azure AD Tenant ID
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// Name that will show to select database
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Database connectionstring
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database schema
        /// </summary>
        [Required]
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
        [Required]
        public bool IsInternalDatabase { get; set; }
    }
}
