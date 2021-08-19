using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Enum;
using System;

namespace Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Interface
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Email { get; set; }
        Guid TenantId { get; set; }
        UserRole Role { get; set; }
    }
}