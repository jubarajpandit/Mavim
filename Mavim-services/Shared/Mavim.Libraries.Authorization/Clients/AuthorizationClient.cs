using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Authorization.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Mavim.Libraries.Authorization.Clients
{
    public class AuthorizationClient : IAuthorizationClient
    {
        private const string MediaTypeJson = "application/json";
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly IJwtSecurityToken _token;

        public AuthorizationClient(
            HttpClient httpClient,
            ILogger<AuthorizationClient> logger,
            IJwtSecurityToken token)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public async Task<IAuthorization> GetAuthorization(string requestUri)
        {
            string responseJson = await GetAuthorizationResponse(requestUri);

            TopicAuthorizationResponse createAuthorizationResponse = JsonConvert.DeserializeObject<TopicAuthorizationResponse>(responseJson);

            IAuthorization authorization = Map(createAuthorizationResponse);

            return authorization;
        }

        /// <summary>
        /// Gets the Authorization response.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Internal server error</exception>
        private async Task<string> GetAuthorizationResponse(string requestUri)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeJson));
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Manager.Api.Utils.Constants.Configuration.BEARER, _token.RawData);

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private static IAuthorization Map(TopicAuthorizationResponse response) =>
            response == null ? null :
                new TopicAuthorization
                {
                    Readonly = response.Role == Role.Subscriber,
                    IsAdmin = response.Role == Role.Administrator
                };
    }
}
