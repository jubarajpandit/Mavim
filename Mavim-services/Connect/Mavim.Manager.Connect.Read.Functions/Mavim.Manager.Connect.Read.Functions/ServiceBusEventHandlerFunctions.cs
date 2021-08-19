using Azure.Messaging.ServiceBus;
using Mavim.Manager.Connect.Read.Functions.Clients;
using Mavim.Manager.Connect.Read.Functions.Constants;
using Mavim.Manager.Connect.Read.Functions.Constants.Enums;
using Mavim.Manager.Connect.Read.Functions.Handlers;
using Mavim.Manager.Connect.Read.Functions.Handlers.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Functions
{
    public static class ServiceBusEventHandlerFunctions
    {
        private static readonly IConnectHttpClient client = new ConnectHttpClient(new HttpClient());

        [FunctionName("ConnectUserManagementQueueHandlerV1")]
        public static async Task ProcessQueue([ServiceBusTrigger("connectqueue", Connection = "ASBConnectionStringConnect")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions,
            ILogger log)
        {
            log.LogInformation($"ProcessQueue ServiceBusTrigger function processing message: {message.MessageId}.");

            var statuscode = await HandleMessage(message, log, client);

            await FinalizeMessage(statuscode, message, messageActions, log);
        }

        [FunctionName("ConnectUserManagementBatchQueueHandlerV1")]
        public static async Task ProcessBatch([ServiceBusTrigger("connectbatchqueue", Connection = "ASBConnectionStringConnect")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions,
            ILogger log)
        {
            log.LogInformation($"ProcessQueue ServiceBusTrigger function processing message: {message.MessageId}.");

            var action = await HandleMessage(message, log, client);

            await FinalizeMessage(action, message, messageActions, log);
        }

        private static async Task FinalizeMessage(MessageActions status, ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions, ILogger log)
        {
            switch (status)
            {
                case MessageActions.Complete:
                    log.LogInformation($"Event Success!: Completing message: {message.MessageId}.");
                    await messageActions.CompleteMessageAsync(message);
                    break;
                case MessageActions.Abandon:
                    log.LogDebug($"Retry #{message.DeliveryCount}.");
                    await messageActions.AbandonMessageAsync(message);
                    break;
                default:
                    log.LogError($"Deadlettering message: {message.MessageId}.");
                    await messageActions.DeadLetterMessageAsync(message);
                    break;
            }
        }

        public static async Task<MessageActions> HandleMessage(ServiceBusReceivedMessage message, ILogger log, IConnectHttpClient client)
        {
            IMessageHandler messageHandler = MessageHandlerFactory.GetMessageHandler(message, log, client);

            HttpResponseMessage response = await messageHandler.ExecuteAsync();

            return await GetMessageActionByResponseAsync(log, response);
        }

        private static async Task<MessageActions> GetMessageActionByResponseAsync(ILogger log, HttpResponseMessage response)
        {
            if (response is null)
            {
                log.LogTrace($"Response is null.");
                return MessageActions.DeadLetter;
            }

            if (response.IsSuccessStatusCode)
                return MessageActions.Complete;
            else
            {
                var errorBody = await response.Content.ReadAsAsync<ErrorBody>();
                log.LogTrace($"Event Failed error: {errorBody.Error}");
                return errorBody?.ErrorCode == ErrorCodes.AggregateIdTooLow
                    ? MessageActions.DeadLetter
                    : MessageActions.Abandon;
            }
        }

        private class ErrorBody
        {
            public string Error { get; set; }
            public int ErrorCode { get; set; }
        }
    }
}
