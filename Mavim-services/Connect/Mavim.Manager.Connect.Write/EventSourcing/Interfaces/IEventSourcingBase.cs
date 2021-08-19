using Mavim.Manager.Connect.Write.Database.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.EventSourcing.Interfaces
{
    public interface IEventSourcingBase
    {
        Task<IReadOnlyList<EventSourcingModel>> GetEvents(Guid EntityId, Guid companyId,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<EventSourcingModel>> GetEvents(Guid entityId, Guid companyId, int endAggregateId,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<EventSourcingModel>> GetEvents(DateTime startDate, Guid companyId,
            CancellationToken cancellationToken, DateTime? endDate = null);

        Task<EventSourcingModel> GetLatestEvent(Guid EntityId, Guid companyId, CancellationToken cancellationToken);

        Task<IReadOnlyList<EventSourcingModel>> GetListOfEntityEvents(IReadOnlyList<Guid> entityIds, Guid companyId,
            CancellationToken cancellationToken);

        Task AddEventToDatabase(EventSourcingModel @event, CancellationToken cancellationToken);

        Task<bool> SaveEvent(CancellationToken cancellationToken);
    }
}