using System;

namespace Mavim.Manager.Connect.Write.DomainModel.Interfaces
{
    public interface IEventModel
    {
        Guid? Id { get; init; }
        bool? IsActive { get; init; }
        IEventModel Activate();
        IEventModel Deactivate();
    }
}