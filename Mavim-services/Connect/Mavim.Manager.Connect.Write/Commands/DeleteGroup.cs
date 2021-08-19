using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using MediatR;
using Microsoft.Azure.ServiceBus;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public static class DeleteGroup
    {
        // Command
        public record Command(Guid GroupId) : IRequest<Unit>;

        // Handler
        public class Handler : EventSourcingBaseCommand<IEventSourcingGeneric<GroupV1>>, IRequestHandler<Command, Unit>
        {
            private readonly IIdentityService _identity;

            public Handler(IEventSourcingGeneric<GroupV1> eventSourcing, IIdentityService identity, IQueueClient queueClient) : base(eventSourcing, queueClient)
            {
                _identity = identity ?? throw new ArgumentNullException(nameof(identity));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));

                if (!await CanHandle())
                    throw new ForbiddenRequestException($"You are not allowed to update group {request.GroupId}");

                var @event = await GetEventAsync(request, cancellationToken);

                await SaveEventToDatabase(@event, cancellationToken);

                await SendEventToServiceBus(@event);

                return await Task.FromResult(Unit.Value);
            }

            private async Task<EventSourcingModel> GetEventAsync(Command request, CancellationToken cancellationToken)
            {
                var GroupId = request.GroupId;
                var events = await _eventSourcing.GetEvents(GroupId, _identity.CompanyId, cancellationToken);
                var entity = await _eventSourcing.PlayEvents(events);
                var lastEvent = events.OrderBy(e => e.AggregateId).LastOrDefault();

                if (entity == null || entity?.IsActive == false || lastEvent == null)
                    throw new RequestNotFoundException($"Group {request.GroupId} not found");

                return _eventSourcing.CreateDeleteEvent(lastEvent.AggregateId, GroupId, lastEvent.CompanyId);
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}