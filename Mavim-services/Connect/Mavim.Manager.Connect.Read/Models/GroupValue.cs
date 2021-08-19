using Mavim.Manager.Connect.Read.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Read.Models
{
    public record GroupValue(Guid Id, string Name, string Description, Guid CompanyId,
        IReadOnlyList<Guid> Users) : IGroupValue;
}
