using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Read.Models.Interfaces
{
    public interface IGroupValue
    {
        Guid Id { get; init; }
        string Name { get; init; }
        string Description { get; init; }
        Guid CompanyId { get; init; }
        IReadOnlyList<Guid> Users { get; init; }
    }
}
