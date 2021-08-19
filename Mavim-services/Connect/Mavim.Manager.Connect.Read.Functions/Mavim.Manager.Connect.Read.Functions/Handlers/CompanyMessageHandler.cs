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
    public class CompanyMessageHandler : BaseMessageHandler
    {
        public CompanyMessageHandler(ServiceBusReceivedMessage message,
            ILogger log, IConnectHttpClient httpClient) : base(message, log, httpClient) { }

        protected override async Task<HttpResponseMessage> SendCreateEvent()
        {
            Logger.LogInformation("HandleCompany Create Event processing message start.");
            return await HttpClient.SendRequestAsync(Endpoints.AddCompany, HttpMethod.Post, Map(GetMessageBody<ReceivedCompany>()), Logger);
        }

        private Company Map(ReceivedCompany company) => new Company
        {
            Id = company.Id,
            Name = company.Name,
            Domain = company.Domain,
            TenantId = company.TenantId,
            IsActive = company.IsActive,
            ModelVersion = Message.GetIntProperty("entityModelVersion"),
            AggregateId = Message.GetIntProperty("aggregateId")
        };
    }
}
