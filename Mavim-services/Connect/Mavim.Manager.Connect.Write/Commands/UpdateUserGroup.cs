using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using MediatR;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public static class UpdateUserGroup
    {
        // Command
        public record Command(Guid GroupId, IReadOnlyList<Guid> UserIds) : IRequest<Unit>;

        // Handler
        public class Handler : EventSourcingBaseCommand<IEventSourcingGeneric<GroupV1>>, IRequestHandler<Command, Unit>
        {
            private readonly IIdentityService _identity;
            private readonly IEventSourcingGeneric<UserV1> _userEventSourcing;

            public Handler(IEventSourcingGeneric<GroupV1> eventSourcing, IEventSourcingGeneric<UserV1> userEventSourcing,
                IIdentityService identity, IQueueClient queueClient) : base(eventSourcing, queueClient)
            {
                _userEventSourcing = userEventSourcing ?? throw new ArgumentNullException(nameof(userEventSourcing));
                _identity = identity ?? throw new ArgumentNullException(nameof(identity));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var @event = await GetEventAsync(request, cancellationToken);

                await SaveEventToDatabase(@event, cancellationToken);

                await SendEventToServiceBus(@event);

                return await Task.FromResult(Unit.Value);
            }

            private async Task<EventSourcingModel> GetEventAsync(Command request, CancellationToken cancellationToken)
            {
                if (!await CanHandle())
                    throw new ForbiddenRequestException("You are not allowed to create a user");

                var companyId = _identity.CompanyId;

                var (GroupId, UserIds) = request ?? throw new ArgumentNullException(nameof(request));

                var events = await _eventSourcing.GetEvents(GroupId, _identity.CompanyId, cancellationToken);
                var entity = await _eventSourcing.PlayEvents(events);
                var lastEvent = events?.OrderBy(e => e.AggregateId).LastOrDefault();

                if (entity == null || entity?.IsActive == false || lastEvent == null)
                    throw new RequestNotFoundException($"Group with ID {GroupId} does not exists");

                if (!await _userEventSourcing.DoesEntitiesExists(UserIds, companyId, cancellationToken))
                    throw new RequestNotFoundException("Unable to add an non-existing userId");

                if (UserIds.Count != UserIds.Distinct().Count())
                    throw new UnprocessableEntityException("Request contains duplicate items");

                var payload = Map(UserIds);

                var newPayload = await _eventSourcing.AddPartialPayload(entity, payload);

                if (!Enumerable.SequenceEqual(newPayload.UserIds.OrderBy(t => t), payload.UserIds.OrderBy(t => t)))
                    throw new UnprocessableEntityException("Properties to update contains the same results");

                return _eventSourcing.CreateAddPartialEvent(lastEvent.AggregateId, GroupId, lastEvent.CompanyId,
                        newPayload);
            }

            private static GroupV1 Map(IReadOnlyList<Guid> GroupIds)
            {
                return new(null, null, null, null, GroupIds, null);
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}