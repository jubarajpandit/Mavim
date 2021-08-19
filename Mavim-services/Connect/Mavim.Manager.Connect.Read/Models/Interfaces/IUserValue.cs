using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Read.Models.Interfaces
{
    public interface IUserValue
    {
        Guid Id { get; init; }
        string Email { get; init; }
        Guid CompanyId { get; init; }
        IReadOnlyList<Guid> Groups { get; init; }
    }
}
