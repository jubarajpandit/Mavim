using Mavim.Manager.Connect.Read.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Read.Models
{
    public record UserValue(Guid Id, string Email, Guid CompanyId, IReadOnlyList<Guid> Groups) : IUserValue;
}
