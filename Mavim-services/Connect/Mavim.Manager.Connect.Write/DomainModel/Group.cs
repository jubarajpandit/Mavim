using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Write.DomainModel
{
    public record GroupV1(Guid? Id, string Name, string Description, Guid? CompanyId, IReadOnlyList<Guid> UserIds, bool? IsActive) : EventModel(Id, IsActive), IEventModel
    {
        public GroupV1() : this(null, null, null, null, null, null)
        { }
    }
}