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
    public class UserMessageHandler : BaseMessageHandler
    {
        public UserMessageHandler(ServiceBusReceivedMessage message,
            ILogger log,
            IConnectHttpClient httpClient) : base(message, log, httpClient) { }

        protected override async Task<HttpResponseMessage> SendCreateEvent()
        {
            Logger.LogInformation("HandleUser: Create Event.");
            if (Message.GetIntProperty("aggregateId") > 0)
                return await HttpClient.SendRequestAsync(string.Format(Endpoints.EnableUser, Message.GetGuidProperty("entityId")), HttpMethod.Patch, Map(GetMessageBody<ReceivedUser>()), Logger);
            else
                return await HttpClient.SendRequestAsync(Endpoints.AddUser, HttpMethod.Post, Map(GetMessageBody<ReceivedUser>()), Logger);
        }

        protected override async Task<HttpResponseMessage> SendDeleteEvent()
        {
            Logger.LogInformation("HandleUser: Delete Event.");
            var endpoint = string.Format(Endpoints.DisableUser, Message.GetGuidProperty("entityId"));
            return await HttpClient.SendRequestAsync(endpoint, HttpMethod.Delete, Map(GetMessageBody<ReceivedUser>()), Logger);
        }

        private User Map(ReceivedUser user) => new User
        {
            Id = user.Id,
            Email = user.Email,
            CompanyId = user.CompanyId,
            IsActive = user.IsActive,
            ModelVersion = Message.GetIntProperty("entityModelVersion"),
            AggregateId = Message.GetIntProperty("aggregateId")
        };
    }
}
