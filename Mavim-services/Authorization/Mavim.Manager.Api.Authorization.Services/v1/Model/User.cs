using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using System;

namespace Mavim.Manager.Api.Authorization.Services.v1.Model
{
    public class User : IUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid TenantId { get; set; }
        public Role Role { get; set; }
    }
}
