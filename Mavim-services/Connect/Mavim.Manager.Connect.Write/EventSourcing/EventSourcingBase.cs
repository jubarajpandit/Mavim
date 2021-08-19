using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.EventSourcing
{
    public abstract class EventSourcingBase : IEventSourcingBase
    {
        private readonly ConnectDbContext _dbContext;

        public EventSourcingBase(ConnectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task<EventSourcingModel> GetLatestEvent(Guid entityId, Guid companyId,
                    CancellationToken cancellationToken)
        {
            return await _dbContext.EventSourcings.AsNoTracking()
                .Where(e => e.EntityId == entityId && e.CompanyId == companyId)
                .OrderByDescending(e => e.AggregateId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<IReadOnlyList<EventSourcingModel>> GetEvents(Guid entityId, Guid companyId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.EventSourcings.AsNoTracking()
                .Where(e => e.EntityId == entityId && e.CompanyId == companyId)
                .OrderBy(e => e.AggregateId)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<IReadOnlyList<EventSourcingModel>> GetEvents(Guid entityId, Guid companyId,
            int endAggregateId, CancellationToken cancellationToken)
        {
            var events = await GetEvents(entityId, companyId, cancellationToken);
            return events.Where(e => e.AggregateId <= endAggregateId).ToList();
        }

        public virtual async Task<IReadOnlyList<EventSourcingModel>> GetEvents(DateTime startDate, Guid companyId,
            CancellationToken cancellationToken, DateTime? endDate = null)
        {
            var events = await _dbContext.EventSourcings.AsNoTracking()
                .Where(e => e.TimeStamp >= startDate && e.TimeStamp <= (endDate ?? DateTime.Now) &&
                            e.CompanyId == companyId)
                .ToListAsync(cancellationToken);

            return events;
        }

        public virtual async Task<IReadOnlyList<EventSourcingModel>> GetListOfEntityEvents(IReadOnlyList<Guid> entityIds,
            Guid companyId, CancellationToken cancellationToken)
        {
            return await _dbContext.EventSourcings.AsNoTracking()
                .Where(e => entityIds.Contains(e.EntityId) && e.CompanyId == companyId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddEventToDatabase(EventSourcingModel @event, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(@event, cancellationToken);
        }

        public async Task<bool> SaveEvent(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        protected static EventSourcingModel CreateEvent(
            EventType eventType,
            EntityType entityType,
            int aggregateId,
            Guid entityId,
            Guid companyId,
            string payload,
            int entityModelVersion)
        {
            return new EventSourcingModel(
                eventType,
                aggregateId,
                entityId,
                entityType,
                entityModelVersion,
                payload,
                DateTime.Now.ToUniversalTime(),
                companyId);
        }

    }
}