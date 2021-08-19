using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
using System;

namespace Mavim.Manager.Connect.Write.DomainModel
{
    public record UserV1(Guid? Id, string Email, Guid? CompanyId, bool? IsActive) : EventModel(Id, IsActive), IEventModel
    {
        public UserV1() : this(null, null, null, null)
        { }
    }
}