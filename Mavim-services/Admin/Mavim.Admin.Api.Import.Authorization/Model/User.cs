using System;

namespace Mavim.Admin.Api.Import.Authorization.Model
{
    public class User
    {
        public Guid ObjectId { get; set; }
        public Guid TenantId { get; set; }
    }
}