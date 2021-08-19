using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Authorization.Models;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Clients
{
    public class AzureSqlAccessTokenClient : IAzureSqlAccessTokenClient
    {
        private const string Resource = "https://database.windows.net/";

        public async Task<string> GetTokenAsync(Guid tenantId, Guid applicationId, string applicationSecret)
        {
            string connectionString = new AzureAdAppConnectionString(tenantId, applicationId, applicationSecret);

            AzureServiceTokenProvider provider = new AzureServiceTokenProvider(connectionString);
            return await provider.GetAccessTokenAsync(Resource, tenantId.ToString());
        }
    }
}
