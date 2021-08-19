using System;

namespace Mavim.Manager.Connect.Read.Databases.Interfaces
{
    public interface IDiscoveryUser
    {
        Guid Id { get; init; }
        string Email { get; init; }
        Guid TenantId { get; init; }
        bool Disabled { get; init; }
    }
}
