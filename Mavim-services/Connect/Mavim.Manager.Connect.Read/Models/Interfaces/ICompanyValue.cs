using System;

namespace Mavim.Manager.Connect.Read.Models.Interfaces
{
    public interface ICompanyValue
    {
        Guid Id { get; init; }
        string Name { get; init; }
        string Domain { get; init; }
        Guid TenantId { get; init; }
    }
}
