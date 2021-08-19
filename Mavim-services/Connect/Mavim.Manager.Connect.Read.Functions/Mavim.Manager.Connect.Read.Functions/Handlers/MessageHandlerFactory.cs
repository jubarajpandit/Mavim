using Azure.Messaging.ServiceBus;
using Mavim.Manager.Connect.Read.Functions.Clients;
using Mavim.Manager.Connect.Read.Functions.Handlers.Interfaces;
using Mavim.Manager.Connect.Read.Functions.Utils;
using Microsoft.Extensions.Logging;
using Enums = Mavim.Manager.Connect.Read.Functions.Constants.Enums;

namespace Mavim.Manager.Connect.Read.Functions.Handlers
{
    public static class MessageHandlerFactory
    {
        public static IMessageHandler GetMessageHandler(ServiceBusReceivedMessage message, ILogger log, IConnectHttpClient client) =>
            MapToEntityType(message.GetIntProperty("entityType")) switch
            {
                Enums.EntityType.User => new UserMessageHandler(message, log, client),
                Enums.EntityType.Group => new GroupMessageHandler(message, log, client),
                Enums.EntityType.Company => new CompanyMessageHandler(message, log, client),
                _ => null
            };

        private static Enums.EntityType MapToEntityType(int entityType) =>
            entityType switch
            {
                0 => Enums.EntityType.User,
                1 => Enums.EntityType.Group,
                2 => Enums.EntityType.Company,
                _ => Enums.EntityType.Unknown
            };
    }
}
