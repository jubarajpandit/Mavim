using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using Mavim.Manager.Connect.Write.ServiceBus.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public static class UndoEventSourcing
    {
        // Command
        public record Command(DateTime StartDate) : IRequest<Unit>;

        // Handler
        public class Handler : EventSourcingBaseCommand<ICommonEventSourcing>, IRequestHandler<Command, Unit>
        {
            private readonly IEventSourcingGeneric<CompanyV1> _companyEventSourcing;
            private readonly IEventSourcingGeneric<GroupV1> _groupEventSourcing;
            private readonly IIdentityService _identity;
            private readonly Dictionary<Guid, int> _latestAggregateId = new();
            private readonly IEventSourcingGeneric<UserV1> _userEventSourcing;
            private readonly List<EventSourcingModel> _events = new();

            public Handler(
                IEventSourcingGeneric<UserV1> userEventSourcing,
                IEventSourcingGeneric<GroupV1> groupEventSourcing,
                IEventSourcingGeneric<CompanyV1> companyEventSourcing,
                ICommonEventSourcing undoEventSourcing,
                IIdentityService identity,
                IBatchQueueClient queueClient) : base(undoEventSourcing, queueClient)
            {
                _userEventSourcing = userEventSourcing ?? throw new ArgumentNullException(nameof(userEventSourcing));
                _groupEventSourcing = groupEventSourcing ?? throw new ArgumentNullException(nameof(groupEventSourcing));
                _companyEventSourcing = companyEventSourcing ?? throw new ArgumentNullException(nameof(companyEventSourcing));
                _identity = identity ?? throw new ArgumentNullException(nameof(identity));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await CanHandle())
                    throw new ForbiddenRequestException("You are not allowed to undo the database");

                // Warning: Using CommonEventSourcing bypass type checking. this will get all results of any entity type
                var events = (await _eventSourcing.GetEvents(request.StartDate, _identity.CompanyId, cancellationToken))
                    .OrderByDescending(e => e.TimeStamp)
                    .ToList();

                foreach (var @event in events)
                {
                    var (eventType, aggregateId, entityId, entityType, entityModelVersion, _, _, companyId) = @event;

                    Task taskToExecute = eventType switch
                    {
                        EventType.Create => UndoCreate(GetEventSourcing(entityType), entityId, entityModelVersion,
                            companyId, cancellationToken),
                        EventType.Update => UndoUpdate(GetEventSourcing(entityType), aggregateId, entityId,
                            entityModelVersion, companyId, cancellationToken),
                        EventType.Delete => UndoDelete(GetEventSourcing(entityType), aggregateId, entityId,
                            entityModelVersion, companyId, cancellationToken),
                        EventType.AddPartial => UndoAddPartial(GetEventSourcing(entityType), aggregateId, entityId,
                            entityModelVersion, companyId, cancellationToken),
                        EventType.RemovePartial => UndoRemovePartial(GetEventSourcing(entityType), aggregateId,
                            entityId, entityModelVersion, companyId, cancellationToken),
                        _ => throw new ArgumentOutOfRangeException($"Type {eventType} is not recognised.")
                    };

                    taskToExecute.Wait(cancellationToken);

                }

                if (!await _companyEventSourcing.SaveEvent(cancellationToken))
                    throw new Exception("Database save was not successful.");

                await SendEventToServiceBus(_events);

                return await Task.FromResult(Unit.Value);
            }

            // Dynamic is needed because to use generic type.
            private dynamic GetEventSourcing(EntityType entityType)
            {
                return entityType switch
                {
                    EntityType.User => _userEventSourcing,
                    EntityType.Group => _groupEventSourcing,
                    EntityType.Company => _companyEventSourcing,
                    _ => throw new NotImplementedException($"Entity type of {entityType} is unknown")
                };
            }

            private async Task UndoUpdate<T>(IEventSourcingGeneric<T> eventSourcing, int aggregateId, Guid entityId,
                int entityModelVersion, Guid companyId, CancellationToken cancellationToken) where T : IEventModel
            {
                var events = await eventSourcing.GetEvents(entityId, companyId, aggregateId, cancellationToken);
                var currentEntity = await eventSourcing.PlayEvents(events);
                var previousEvents = events
                    .Where(e => e.AggregateId < aggregateId)
                    .OrderBy(b => b.AggregateId)
                    .ToList();

                var previousEntity = await eventSourcing.PlayEvents(previousEvents);
                var newPayload = await eventSourcing.UpdatePayload(currentEntity, previousEntity);
                var latestAggregateId =
                    await GetLatestAggregateId(eventSourcing, entityId, companyId, cancellationToken);

                var @event = eventSourcing.CreateUpdateEvent(latestAggregateId, entityId, companyId, newPayload,
                    entityModelVersion);
                await eventSourcing.AddEventToDatabase(@event, cancellationToken);
                _events.Add(@event);
            }

            private async Task UndoAddPartial<T>(IEventSourcingGeneric<T> eventSourcing, int aggregateId, Guid entityId,
                int entityModelVersion, Guid companyId, CancellationToken cancellationToken) where T : IEventModel
            {
                var latestAggregateId =
                    await GetLatestAggregateId(eventSourcing, entityId, companyId, cancellationToken);
                var events = await eventSourcing.GetEvents(entityId, companyId, aggregateId, cancellationToken);
                var lastEvent = events.OrderBy(e => e.AggregateId).Last();
                var payload = await eventSourcing.ParseJson(lastEvent.Payload);
                var @event = eventSourcing.CreateRemovePartialEvent(latestAggregateId, entityId, companyId, payload,
                    entityModelVersion);
                await eventSourcing.AddEventToDatabase(@event, cancellationToken);
                _events.Add(@event);
            }

            private async Task UndoRemovePartial<T>(IEventSourcingGeneric<T> eventSourcing, int aggregateId,
                Guid entityId, int entityModelVersion, Guid companyId, CancellationToken cancellationToken)
                where T : IEventModel
            {
                var latestAggregateId =
                    await GetLatestAggregateId(eventSourcing, entityId, companyId, cancellationToken);
                var events = await eventSourcing.GetEvents(entityId, companyId, aggregateId, cancellationToken);
                var lastEvent = events.OrderBy(e => e.AggregateId).Last();
                var payload = await eventSourcing.ParseJson(lastEvent.Payload);
                var @event = eventSourcing.CreateAddPartialEvent(latestAggregateId, entityId, companyId, payload,
                    entityModelVersion);
                await eventSourcing.AddEventToDatabase(@event, cancellationToken);
                _events.Add(@event);
            }

            private async Task UndoCreate<T>(IEventSourcingGeneric<T> eventSourcing, Guid entityId,
                int entityModelVersion, Guid companyId, CancellationToken cancellationToken) where T : IEventModel
            {
                var latestAggregateId =
                    await GetLatestAggregateId(eventSourcing, entityId, companyId, cancellationToken);
                var @event =
                    eventSourcing.CreateDeleteEvent(latestAggregateId, entityId, companyId, entityModelVersion);
                await eventSourcing.AddEventToDatabase(@event, cancellationToken);
                _events.Add(@event);
            }

            private async Task UndoDelete<T>(IEventSourcingGeneric<T> eventSourcing, int aggregateId, Guid entityId,
                int entityModelVersion, Guid companyId, CancellationToken cancellationToken) where T : IEventModel
            {
                var events = await eventSourcing.GetEvents(entityId, companyId, --aggregateId, cancellationToken);
                var entity = await eventSourcing.PlayEvents(events);
                entity = (T)entity.Activate();

                var latestAggregateId =
                    await GetLatestAggregateId(eventSourcing, entityId, companyId, cancellationToken);
                var @event = eventSourcing.CreateNewEvent(entityId, companyId, entity, ++latestAggregateId,
                    entityModelVersion);
                await eventSourcing.AddEventToDatabase(@event, cancellationToken);
                _events.Add(@event);
            }

            private async Task<int> GetLatestAggregateId<T>(IEventSourcingGeneric<T> eventSourcing, Guid entityId,
                Guid companyId, CancellationToken cancellationToken) where T : IEventModel
            {
                int latestAggregateId;
                if (_latestAggregateId.TryGetValue(entityId, out var existingValue))
                {
                    latestAggregateId = existingValue;
                    _latestAggregateId.Remove(entityId);
                }
                else
                {
                    latestAggregateId = (await eventSourcing.GetLatestEvent(entityId, companyId, cancellationToken))
                        .AggregateId;
                }

                _latestAggregateId.TryAdd(entityId, ++latestAggregateId);

                return --latestAggregateId;
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}