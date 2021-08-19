using Azure.Messaging.ServiceBus;
using Mavim.Manager.Connect.Read.Functions.Clients;
using Mavim.Manager.Connect.Read.Functions.Constants;
using Mavim.Manager.Connect.Read.Functions.Models;
using Mavim.Manager.Connect.Read.Functions.Utils;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Functions.Handlers
{
    public class GroupMessageHandler : BaseMessageHandler
    {
        public GroupMessageHandler(ServiceBusReceivedMessage message,
            ILogger log, IConnectHttpClient httpClient) : base(message, log, httpClient)
        {
        }

        protected override async Task<HttpResponseMessage> SendCreateEvent()
        {
            Logger.LogInformation("HandleGroup Create Event processing message start.");
            if (Message.GetIntProperty("aggregateId") > 0)
                return await HttpClient.SendRequestAsync(string.Format(Endpoints.EnableGroup, Message.GetGuidProperty("entityId")), HttpMethod.Patch, Map(GetMessageBody<ReceivedGroup>()), Logger);
            else
                return await HttpClient.SendRequestAsync(Endpoints.AddGroup, HttpMethod.Post, Map(GetMessageBody<ReceivedGroup>()), Logger);
        }

        protected override async Task<HttpResponseMessage> SendUpdateEvent()
        {
            Logger.LogInformation("HandleGroup Update Event processing message start.");
            var updateEndpoint = string.Format(Endpoints.UpdateGroup, Message.GetGuidProperty("entityId"));
            return await HttpClient.SendRequestAsync(updateEndpoint, HttpMethod.Patch, Map(GetMessageBody<ReceivedGroup>()), Logger);
        }

        protected override async Task<HttpResponseMessage> SendDeleteEvent()
        {
            Logger.LogInformation("HandleGroup Delete Event processing message start.");
            var disableEndpoint = string.Format(Endpoints.DisableGroup, Message.GetGuidProperty("entityId"));
            return await HttpClient.SendRequestAsync(disableEndpoint, HttpMethod.Delete, Map(GetMessageBody<ReceivedGroup>()), Logger);
        }

        protected override async Task<HttpResponseMessage> SendAddPartialEvent()
        {
            Logger.LogInformation("HandleGroup Add Partial Event processing message start.");
            var addEndpoint = string.Format(Endpoints.AddOrDeleteUsersToGroup, Message.GetGuidProperty("entityId"));
            return await HttpClient.SendRequestAsync(addEndpoint, HttpMethod.Patch, Map(GetMessageBody<ReceivedGroup>()), Logger);
        }

        protected override async Task<HttpResponseMessage> SendRemovePartialEvent()
        {
            Logger.LogInformation("HandleGroup Remove Partial Event processing message start.");
            var removeEndpoint = string.Format(Endpoints.AddOrDeleteUsersToGroup, Message.GetGuidProperty("entityId"));
            return await HttpClient.SendRequestAsync(removeEndpoint, HttpMethod.Delete, Map(GetMessageBody<ReceivedGroup>()), Logger);
        }

        private Group Map(ReceivedGroup group) => new Group
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            CompanyId = group.CompanyId,
            Ids = group.UserIds,
            IsActive = group.IsActive,
            ModelVersion = Message.GetIntProperty("entityModelVersion"),
            AggregateId = Message.GetIntProperty("aggregateId")
        };
    }
}
