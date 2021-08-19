using System;

namespace Mavim.Manager.Api.Authorization.Services.Interfaces.v1
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Email { get; set; }
        Guid TenantId { get; set; }
        Role Role { get; set; }
    }
}
