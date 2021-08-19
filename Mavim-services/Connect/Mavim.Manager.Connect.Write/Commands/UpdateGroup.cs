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
    public static class UpdateGroup
    {
        // Command
        public record Command(Guid GroupId, string Name, string Description) : IRequest<Unit>;

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
                    throw new ForbiddenRequestException("You are not allowed to create a user");

                var @event = await GetEventAsync(request, cancellationToken);

                await SaveEventToDatabase(@event, cancellationToken);

                await SendEventToServiceBus(@event);

                return await Task.FromResult(Unit.Value);
            }

            private async Task<EventSourcingModel> GetEventAsync(Command request, CancellationToken cancellationToken)
            {
                var (GroupId, Name, Description) = request;

                if (Name?.Trim() == string.Empty)
                    throw new BadRequestException("Empty name property is not allowed");

                if (string.IsNullOrWhiteSpace(Name) && Description == null)
                    throw new BadRequestException("Please update at least one property");

                var events = await _eventSourcing.GetEvents(GroupId, _identity.CompanyId, cancellationToken);
                var entity = await _eventSourcing.PlayEvents(events);
                var lastEvent = events?.OrderBy(e => e.AggregateId).LastOrDefault();

                if (entity == null || entity?.IsActive == false || lastEvent == null)
                    throw new RequestNotFoundException($"Group with ID {GroupId} does not exists");

                var payload = Map(request);

                var newPayload = await _eventSourcing.UpdatePayload(entity, payload);

                if (newPayload != payload)
                    throw new UnprocessableEntityException("One or more properties to update contains the same results");

                return _eventSourcing.CreateUpdateEvent(lastEvent.AggregateId, GroupId, lastEvent.CompanyId, newPayload);
            }

            private static GroupV1 Map(Command request)
            {
                return new(null, request.Name, request.Description, null, null, null);
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}