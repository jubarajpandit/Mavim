using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using MediatR;
using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public static class AddUser
    {
        // Command
        public record Command(string Email) : IRequest<Guid>;

        // Handler
        public class Handler : EventSourcingBaseCommand<IEventSourcingGeneric<UserV1>>, IRequestHandler<Command, Guid>
        {
            protected readonly IIdentityService _identity;

            public Handler(IEventSourcingGeneric<UserV1> eventSourcing, IIdentityService identity, IQueueClient queueClient) : base(eventSourcing, queueClient)
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
                var userId = Guid.NewGuid();
                var companyId = _identity.CompanyId;
                var user = Map(userId, companyId, request);

                return _eventSourcing.CreateNewEvent(userId, companyId, user);
            }

            private static UserV1 Map(Guid userId, Guid companyId, Command request)
            {
                return new(userId, request.Email.ToLower(), companyId, true);
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}