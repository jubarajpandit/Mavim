using Azure.Messaging.ServiceBus;
using Mavim.Manager.Connect.Read.Functions.Clients;
using Mavim.Manager.Connect.Read.Functions.Handlers.Interfaces;
using Mavim.Manager.Connect.Read.Functions.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enums = Mavim.Manager.Connect.Read.Functions.Constants.Enums;

namespace Mavim.Manager.Connect.Read.Functions.Handlers
{
    public abstract class BaseMessageHandler : IMessageHandler
    {
        protected ServiceBusReceivedMessage Message { get; private set; }
        protected ILogger Logger { get; private set; }
        protected IConnectHttpClient HttpClient { get; private set; }

        public BaseMessageHandler(ServiceBusReceivedMessage message, ILogger logger, IConnectHttpClient client)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            HttpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<HttpResponseMessage> ExecuteAsync() =>
            MapToEventType(Message.GetIntProperty("eventType")) switch
            {
                Enums.EventType.Create => await SendCreateEvent(),
                Enums.EventType.Update => await SendUpdateEvent(),
                Enums.EventType.Delete => await SendDeleteEvent(),
                Enums.EventType.AddPartial => await SendAddPartialEvent(),
                Enums.EventType.RemovePartial => await SendRemovePartialEvent(),
                _ => null
            };

        protected virtual Task<HttpResponseMessage> SendCreateEvent()
        {
            throw new NotImplementedException();
        }

        protected virtual Task<HttpResponseMessage> SendUpdateEvent()
        {
            throw new NotImplementedException();
        }

        protected virtual Task<HttpResponseMessage> SendDeleteEvent()
        {
            throw new NotImplementedException();
        }

        protected virtual Task<HttpResponseMessage> SendAddPartialEvent()
        {
            throw new NotImplementedException();
        }

        protected virtual Task<HttpResponseMessage> SendRemovePartialEvent()
        {
            throw new NotImplementedException();
        }

        protected T GetMessageBody<T>()
        {
            return JsonSerializer.Deserialize<T>(Message.Body);
        }

        protected Enums.EventType MapToEventType(int entityType) =>
            entityType switch
            {
                0 => Enums.EventType.Create,
                1 => Enums.EventType.Update,
                2 => Enums.EventType.Delete,
                3 => Enums.EventType.AddPartial,
                4 => Enums.EventType.RemovePartial,
                _ => Enums.EventType.Unknown
            };
    }
}
