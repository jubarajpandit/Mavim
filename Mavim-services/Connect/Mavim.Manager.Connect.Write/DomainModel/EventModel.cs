using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
using System;

namespace Mavim.Manager.Connect.Write.DomainModel
{
    public abstract record EventModel(Guid? Id, bool? IsActive) : IEventModel
    {
        public EventModel() : this(null, null)
        { }

        public IEventModel Activate()
        {
            return this with { IsActive = true };
        }

        public IEventModel Deactivate()
        {
            return this with { IsActive = false };
        }
    }
}