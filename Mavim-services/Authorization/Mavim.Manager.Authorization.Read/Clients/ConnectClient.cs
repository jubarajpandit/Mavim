using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Authorization.Read.Clients.Interfaces;
using Mavim.Manager.Authorization.Read.Clients.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mavim.Manager.Authorization.Read.Clients
{

    public class ConnectClient : IConnectClient
    {
        private const string MediaTypeJson = "application/json";
        private const string AuthorizationScheme = "BEARER";
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly IJwtSecurityToken token;
        private readonly IOptionsSnapshot<ConnectClientSettings> azConnectClientSettings;

        public ConnectClient(
            HttpClient httpClient,
            ILogger<ConnectClient> logger,
            IJwtSecurityToken token,
            IOptionsSnapshot<ConnectClientSettings> azConnectClientSettings)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.token = token ?? throw new ArgumentNullException(nameof(token));
            this.azConnectClientSettings = azConnectClientSettings ?? throw new ArgumentNullException(nameof(azConnectClientSettings));
        }

        public async Task<User> GetConnectMeUser()
        {
            string connectRootPath = azConnectClientSettings.Value?.ApiEndpoint;
            if (string.IsNullOrEmpty(connectRootPath)) throw new Exception("No ApiEndpoint found from appconfiguration");

            string responseJson = await GetConnectMe(connectRootPath);
            if (string.IsNullOrEmpty(connectRootPath)) throw new ForbiddenRequestException("No user found based on this token.");

            var connectMeResponse = await ParseJson<ConnectMeResponse>(responseJson);

            return Map(connectMeResponse);
        }

        /// <summary>
        /// Gets the Authorization response.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Internal server error</exception>
        private async Task<string> GetConnectMe(string requestUri)
        {
            const string pathToMe = "v1/Users/me";
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeJson));
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationScheme, token.RawData);

            HttpResponseMessage response = await httpClient.GetAsync($"{requestUri}/{pathToMe}");
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private static User Map(ConnectMeResponse response)
        {
            var (id, _, companyId, groups) = response;
            return new User(id, companyId, groups);
        }

        private static async Task<T> ParseJson<T>(string json)
        {
            var byteArray = Encoding.UTF8.GetBytes(json);
            using var stream = new MemoryStream(byteArray);
            var Object = await JsonSerializer.DeserializeAsync<T>(stream);
            return Object;

        }

    }

}
