using System;
namespace Mavim.Admin.Api.Import.Catalog.Model
{
    public class DbConnectionInfo
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string DisplayName { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public Guid? ApplicationTenantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public string ApplicationSecretKey { get; set; }
        public bool IsInternalDatabase { get; set; }
    }
}
