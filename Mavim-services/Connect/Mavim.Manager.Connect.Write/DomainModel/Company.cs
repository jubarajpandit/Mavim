using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
using System;

namespace Mavim.Manager.Connect.Write.DomainModel
{
    public record CompanyV1(Guid? Id, string Name, string Domain, Guid? TenantId, bool? IsActive) : EventModel(Id, IsActive), IEventModel
    {
        public CompanyV1() : this(null, null, null, null, null)
        {
        }
    }
}