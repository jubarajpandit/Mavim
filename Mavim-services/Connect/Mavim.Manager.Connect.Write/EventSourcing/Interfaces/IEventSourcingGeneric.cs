using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.EventSourcing.Interfaces
{
    public interface IEventSourcingGeneric<T> : IEventSourcingBase where T : IEventModel
    {
        Task<bool> DoesEntitiesExists(IReadOnlyList<Guid> entityIds, Guid companyId,
            CancellationToken cancellationToken);

        Task<T> PlayEvents(IReadOnlyList<EventSourcingModel> events);
        Task<T> UpdateEnity(T payload, T entity);
        Task<T> UpdatePayload(T entity, T payload);
        Task<T> AddPartialPayload(T entity, T payload);
        Task<T> RemovePartialPayload(T entity, T payload);

        EventSourcingModel CreateNewEvent(Guid id, Guid companyId, T payload, int aggregateId = 0,
            int entityModelVersion = 1);

        EventSourcingModel CreateUpdateEvent(int aggregateid, Guid entityId, Guid companyId, T payload,
            int entityModelVersion = 1);

        EventSourcingModel CreateDeleteEvent(int aggregateid, Guid entityId, Guid companyId,
            int entityModelVersion = 1);

        EventSourcingModel CreateAddPartialEvent(int aggregateId, Guid entityId, Guid companyId, T payload,
            int entityModelVersion = 1);

        EventSourcingModel CreateRemovePartialEvent(int aggregateId, Guid entityId, Guid companyId, T payload,
            int entityModelVersion = 1);

        Task<T> ParseJson(string json);
    }
}