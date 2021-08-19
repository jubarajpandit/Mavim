using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using MediatR;
using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public static class AddCompany
    {

        // Command
        public record Command(string Name, string Domain, Guid TenantId) : IRequest<Guid>;

        // Handler
        public class Handler : EventSourcingBaseCommand<IEventSourcingGeneric<CompanyV1>>, IRequestHandler<Command, Guid>
        {
            public Handler(IEventSourcingGeneric<CompanyV1> eventSourcing, IQueueClient queueClient) : base(eventSourcing, queueClient)
            { }

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
                var companyId = Guid.NewGuid();
                var company = Map(companyId, request);

                return _eventSourcing.CreateNewEvent(companyId, companyId, company);
            }

            private static CompanyV1 Map(Guid companyId, Command request)
            {
                return new(companyId, request.Name, request.Domain.ToLower(), request.TenantId, true);
            }

            private static async Task<bool> CanHandle()
            {
                return await Task.FromResult(true);
            }
        }
    }
}