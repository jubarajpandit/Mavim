using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using Mavim.Manager.Connect.Write.ServiceBus.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public static class ResendEventSourcing
    {
        // Command
        public record Command(int AggregateId, Guid EntityId) : IRequest<Unit>;

        // Handler
        public class Handler : EventSourcingBaseCommand<ICommonEventSourcing>, IRequestHandler<Command, Unit>
        {
            private readonly IIdentityService _identity;

            public Handler(
                ICommonEventSourcing resendEventSourcing,
                IIdentityService identity,
                IBatchQueueClient queueClient) : base(resendEventSourcing, queueClient)
            {
                _identity = identity ?? throw new ArgumentNullException(nameof(identity));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await CanHandle())
                    throw new ForbiddenRequestException("You are not allowed to undo the database");

                var (AggregateId, EntityId) = request;

                // Warning: Using CommonEventSourcing bypass type checking. this will get all results of any entity type
                var events = (await _eventSourcing.GetEvents(EntityId, _identity.CompanyId, cancellationToken))
                    .Where(e => e.AggregateId >= AggregateId)
                    .OrderBy(d => d.AggregateId)
                    .ToList();

                await SendEventToServiceBus(events);


                return await Task.FromResult(Unit.Value);
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}
