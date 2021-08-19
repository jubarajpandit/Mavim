using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using MediatR;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public static class AddGroup
    {
        // Command
        public record Command(string Name, string Description) : IRequest<Guid>;

        // Handler
        public class Handler : EventSourcingBaseCommand<IEventSourcingGeneric<GroupV1>>, IRequestHandler<Command, Guid>
        {
            private readonly IIdentityService _identity;

            public Handler(IEventSourcingGeneric<GroupV1> eventSourcing, IIdentityService identity, IQueueClient queueClient) : base(eventSourcing, queueClient)
            {
                _identity = identity ?? throw new ArgumentNullException(nameof(identity));
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));

                if (!await CanHandle())
                    throw new ForbiddenRequestException("You are not allowed to create a user");

                var @event = GetEvent(request);

                await SaveEventToDatabase(@event, cancellationToken);

                await SendEventToServiceBus(@event);

                return @event.EntityId;
            }

            private EventSourcingModel GetEvent(Command request)
            {
                var groupId = Guid.NewGuid();
                var companyId = _identity.CompanyId;
                var group = Map(groupId, companyId, request);

                return _eventSourcing.CreateNewEvent(groupId, companyId, group);
            }

            private static GroupV1 Map(Guid groupId, Guid companyId, Command request)
            {
                return new(groupId, request.Name, request.Description, companyId, new List<Guid>(), true);
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}