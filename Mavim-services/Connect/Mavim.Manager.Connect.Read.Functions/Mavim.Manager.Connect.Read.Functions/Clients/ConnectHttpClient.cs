using Mavim.Manager.Connect.Read.Functions.Constants;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Functions.Clients
{
    public interface IConnectHttpClient
    {
        Task<HttpResponseMessage> SendRequestAsync<T>(string route, HttpMethod httpMethod, T payload, ILogger logger = null);
    }

    internal class ConnectHttpClient : IConnectHttpClient
    {
        private ILogger _logger;
        private readonly HttpClient _client;

        public ConnectHttpClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<HttpResponseMessage> SendRequestAsync<T>(string route, HttpMethod httpMethod, T payload, ILogger logger)
        {
            if (_logger == null) _logger = logger;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());

            _logger?.LogInformation($" {_client.DefaultRequestHeaders.Authorization}");

            var baseUrl = Environment.GetEnvironmentVariable(ConfigurationKeys.ConnectReadClientBaseUrl);
            var requestUri = $"{baseUrl}{route}";

            _logger?.LogInformation($"Sending event to uri {requestUri}");

            var request = new HttpRequestMessage(httpMethod, requestUri) { Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json") };

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            _logger?.LogInformation($"By sending an event we receive response status code {response} with message {content}");

            return response;
        }

        private async Task<string> GetTokenAsync()
        {
            var requestBody = await SendRequestAsync<LoginRequestBody>(GetTokenFormContent(), GetAuthUrl(), HttpMethod.Post);

            return requestBody?.access_token;
        }

        private string GetAuthUrl()
        {
            var tenantId = Environment.GetEnvironmentVariable(ConfigurationKeys.AzureTenantId);
            var authurl = string.Format(ConfigurationKeys.AuthUrl, tenantId);

            return authurl;
        }

        private Dictionary<string, string> GetTokenFormContent()
        {
            var client_id = Environment.GetEnvironmentVariable(ConfigurationKeys.ConnectReadClientApplicationId);
            var client_secret = Environment.GetEnvironmentVariable(ConfigurationKeys.ConnectReadClientApplicationSecret);
            var scope = Environment.GetEnvironmentVariable(ConfigurationKeys.ConnectReadClientScope);
            var grant_type = ConfigurationKeys.GrantType;

            var formContent = new Dictionary<string, string>
            {
                { "client_id", client_id },
                { "scope", scope },
                { "client_secret", client_secret },
                { "grant_type", grant_type }
            };

            _logger?.LogInformation($"Output all x-www-form-urlencoded data by requesting authorization token.");
            _logger?.LogInformation(string.Join(Environment.NewLine, formContent));

            return formContent;
        }

        private async Task<T> SendRequestAsync<T>(Dictionary<string, string> formUrlEncodedContent, string uri, HttpMethod method)
        {
            _logger?.LogInformation($"Get authorization header from azure ad with method: {method} and uri: {uri} and EncodeType: {formUrlEncodedContent}");
            var req = new HttpRequestMessage(method, uri) { Content = new FormUrlEncodedContent(formUrlEncodedContent) };

            using var client = new HttpClient();
            var response = await client.SendAsync(req);
            var content = await response.Content.ReadAsStringAsync();
            _logger?.LogInformation($"By requesting authorization header we receive response status code {response} with message {content}");

            var responsebody = JsonSerializer.Deserialize<T>(content);

            return responsebody;
        }

        private class LoginRequestBody
        {
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public int ext_expires_in { get; set; }
            public string access_token { get; set; }
        }
    }
}
