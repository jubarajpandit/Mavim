using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Commands
{
    public abstract class EventSourcingBaseCommand<T> where T : IEventSourcingBase
    {
        protected readonly T _eventSourcing;
        private readonly IQueueClient _queueClient;

        public EventSourcingBaseCommand(T eventSourcing, IQueueClient queueClient)
        {
            _eventSourcing = eventSourcing ?? throw new ArgumentNullException(nameof(eventSourcing));
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
        }

        protected async Task SendEventToServiceBus(EventSourcingModel @event)
        {
            var message = Map(@event);
            await _queueClient.SendAsync(message);
        }

        protected async Task SendEventToServiceBus(IReadOnlyList<EventSourcingModel> events)
        {
            var messages = events.Select(Map).ToList();
            foreach (var message in messages)
            {
                await Task.Delay(250);
                await _queueClient.SendAsync(message);
            }
        }

        protected async Task SaveEventToDatabase(EventSourcingModel @event, CancellationToken cancellationToken)
        {
            await _eventSourcing.AddEventToDatabase(@event, cancellationToken);

            if (!await _eventSourcing.SaveEvent(cancellationToken))
                throw new Exception("Database save was not successful.");
        }

        private Message Map(EventSourcingModel @event)
        {
            (EventType eventType, int aggregateId, Guid entityId,
             EntityType entityType, int entityModelVersion, string payload,
             DateTime _, Guid companyId) = @event;

            const string contenType = "application/json";
            var message = new Message(Encoding.UTF8.GetBytes(payload))
            {
                MessageId = $"{entityId}_{aggregateId}", // Unique combination to make  duplicate detection working propertly
                ContentType = contenType // we always using json
            };

            message.UserProperties.Add("eventType", (int)eventType);
            message.UserProperties.Add("aggregateId", aggregateId);
            message.UserProperties.Add("entityId", entityId);
            message.UserProperties.Add("entityType", (int)entityType);
            message.UserProperties.Add("entityModelVersion", entityModelVersion);
            message.UserProperties.Add("companyId", companyId);

            return message;
        }

    }
}
