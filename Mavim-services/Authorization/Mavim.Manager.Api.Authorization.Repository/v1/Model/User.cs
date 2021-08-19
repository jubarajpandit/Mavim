using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Interface;
using System;

namespace Mavim.Manager.Api.Authorization.Repository.v1.Model
{
    public class User : IUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid TenantId { get; set; }
        public UserRole Role { get; set; }
    }
}