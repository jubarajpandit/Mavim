using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.EventSourcing
{
    public class EventSourcingGeneric<T> : EventSourcingBase, IEventSourcingGeneric<T> where T : IEventModel
    {
        private readonly EntityType _entityType;
        private readonly JsonSerializerOptions JsonSerializerOptions = new() { IgnoreNullValues = true };

        public EventSourcingGeneric(ConnectDbContext dbContext, EntityType entityType) : base(dbContext)
        {
            _entityType = entityType;
        }

        public async Task<T> PlayEvents(IReadOnlyList<EventSourcingModel> events)
        {
            var entity = Activator.CreateInstance<T>();
            foreach (var @event in events)
            {
                var payload = await ParseJson(@event.Payload);
                entity = @event.EventType switch
                {
                    EventType.Create => await Create(payload, entity, @event.AggregateId > 0),
                    EventType.Update => await Update(payload, entity),
                    EventType.Delete => (T)await Delete(entity),
                    EventType.AddPartial => await UpdatePartial(payload, entity),
                    EventType.RemovePartial => await RemovePartial(payload, entity),
                    _ => throw new ArgumentOutOfRangeException($"Event type of {@event.EventType} is unknown")
                };
            }

            return entity;
        }

        public async Task<T> UpdateEnity(T payload, T entity)
        {
            return await Update(payload, entity);
        }

        public async Task<T> UpdatePayload(T entity, T payload)
        {
            var newPayload = Activator.CreateInstance<T>();
            return await Task.FromResult(Update(entity, payload, newPayload));
        }

        public async Task<T> AddPartialPayload(T entity, T payload)
        {
            return await Task.FromResult(AddPartialEntity(entity, payload, getPayload: true));
        }

        public async Task<T> RemovePartialPayload(T entity, T payload)
        {
            return await Task.FromResult(RemovePartialEntity(entity, payload, getPayload: true));
        }

        public EventSourcingModel CreateNewEvent(Guid id, Guid companyId, T payload, int aggregateId = 0,
            int entityModelVersion = 1)
        {
            return CreateEvent(EventType.Create, _entityType, aggregateId, id, companyId,
                aggregateId > 0
                    ? JsonSerializer.Serialize(new { IsActive = true }, JsonSerializerOptions)
                    : JsonSerializer.Serialize(payload, JsonSerializerOptions), entityModelVersion);
        }

        public EventSourcingModel CreateUpdateEvent(int aggregateId, Guid entityId, Guid companyId, T payload,
            int entityModelVersion = 1)
        {
            return CreateEvent(EventType.Update, _entityType, ++aggregateId, entityId, companyId,
                JsonSerializer.Serialize(payload, JsonSerializerOptions), entityModelVersion);
        }

        public EventSourcingModel CreateDeleteEvent(int aggregateId, Guid entityId, Guid companyId,
            int entityModelVersion = 1)
        {
            return CreateEvent(EventType.Delete, _entityType, ++aggregateId, entityId, companyId,
                JsonSerializer.Serialize(new { IsActive = false }, JsonSerializerOptions), entityModelVersion);
        }

        public EventSourcingModel CreateAddPartialEvent(int aggregateId, Guid entityId, Guid companyId, T payload,
            int entityModelVersion = 1)
        {
            return CreateEvent(EventType.AddPartial, _entityType, ++aggregateId, entityId, companyId,
                JsonSerializer.Serialize(payload, JsonSerializerOptions), entityModelVersion);
        }

        public EventSourcingModel CreateRemovePartialEvent(int aggregateId, Guid entityId, Guid companyId, T payload,
            int entityModelVersion = 1)
        {
            return CreateEvent(EventType.RemovePartial, _entityType, ++aggregateId, entityId, companyId,
                JsonSerializer.Serialize(payload, JsonSerializerOptions), entityModelVersion);
        }

        public async Task<bool> DoesEntitiesExists(IReadOnlyList<Guid> entityIds, Guid companyId,
            CancellationToken cancellationToken)
        {
            var events = await GetListOfEntityEvents(entityIds, companyId, cancellationToken);
            var listOfTasks = events.Where(e => e.EntityType == _entityType).GroupBy(e => e.EntityId)
                .Select(async e => await PlayEvents(e.OrderBy(b => b.AggregateId).ToList()));

            var existEntities = await Task.WhenAll(listOfTasks);

            return existEntities.Any() && existEntities.All(e => e.IsActive == true);
        }

        public async Task<T> ParseJson(string json)
        {
            var byteArray = Encoding.UTF8.GetBytes(json);
            using var stream = new MemoryStream(byteArray);
            var Object = await JsonSerializer.DeserializeAsync<T>(stream);
            return Object;

        }

        public override async Task<IReadOnlyList<EventSourcingModel>> GetEvents(Guid EntityId, Guid companyId, CancellationToken cancellationToken)
        {
            var result = await base.GetEvents(EntityId, companyId, cancellationToken);
            return result.Where(e => e.EntityType == _entityType).ToList();
        }

        public override async Task<IReadOnlyList<EventSourcingModel>> GetEvents(Guid entityId, Guid companyId, int endAggregateId, CancellationToken cancellationToken)
        {
            var result = await base.GetEvents(entityId, companyId, endAggregateId, cancellationToken);
            return result.Where(e => e.EntityType == _entityType).ToList();
        }

        public override async Task<IReadOnlyList<EventSourcingModel>> GetEvents(DateTime startDate, Guid companyId, CancellationToken cancellationToken, DateTime? endDate = null)
        {
            var result = await base.GetEvents(startDate, companyId, cancellationToken, endDate);
            return result.Where(e => e.EntityType == _entityType).ToList();
        }

        public override async Task<EventSourcingModel> GetLatestEvent(Guid EntityId, Guid companyId, CancellationToken cancellationToken)
        {
            var result = await base.GetLatestEvent(EntityId, companyId, cancellationToken);
            return result.EntityType == _entityType ? result : null;
        }

        public override async Task<IReadOnlyList<EventSourcingModel>> GetListOfEntityEvents(IReadOnlyList<Guid> entityIds, Guid companyId, CancellationToken cancellationToken)
        {
            var result = await base.GetListOfEntityEvents(entityIds, companyId, cancellationToken);
            return result.Where(e => e.EntityType == _entityType).ToList();
        }

        protected virtual T Update(T entity, T payload, T newPayload)
        {
            throw new NotImplementedException($"Update method is called with payload {payload} but is not implemented yet");
        }

        protected virtual T AddPartialEntity(T entity, T payload, bool getPayload)
        {
            throw new NotImplementedException($"AddPartial method is called with payload {payload} but is not implemented yet");
        }

        protected virtual T RemovePartialEntity(T entity, T payload, bool getPayload)
        {
            throw new NotImplementedException($"RemovePartial method is called with payload {payload} but is not implemented yet");
        }

        private static async Task<T> Create(T payload, T entity, bool activate)
        {
            return await Task.FromResult(activate ? (T)entity.Activate() : payload);
        }

        private async Task<T> Update(T payload, T entity)
        {
            return await Task.FromResult(Update(entity, payload, entity));
        }

        private static async Task<IEventModel> Delete(T entity)
        {
            return await Task.FromResult(entity.Deactivate());
        }

        private async Task<T> UpdatePartial(T payload, T entity)
        {
            return await Task.FromResult(AddPartialEntity(entity, payload, getPayload: false));
        }

        private async Task<T> RemovePartial(T payload, T entity)
        {
            return await Task.FromResult(RemovePartialEntity(entity, payload, getPayload: false));
        }
    }
}