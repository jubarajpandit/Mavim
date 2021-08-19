using Mavim.Libraries.Authorization.Interfaces;
using System;

namespace Mavim.Libraries.Authorization.Models
{
    public class MavimDatabaseInfo : IMavimDatabaseInfo
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public Guid? ApplicationTenantId { get; set; }
        public Guid TenantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? ApplicationSecretKey { get; set; }
        public bool IsInternalDatabase { get; set; }
    }
}
