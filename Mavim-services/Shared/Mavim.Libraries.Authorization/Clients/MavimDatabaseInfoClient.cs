using Mavim.Libraries.Authorization.Configuration;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Authorization.Models;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Utils.Constants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Clients
{
    public class MavimDatabaseInfoClient : IMavimDatabaseInfoClient
    {
        private readonly AzDatabaseInfoAppConfigSettings _azDatabaseInfoAppConfigSettings;
        private readonly HttpClient _httpClient;

        private readonly IJwtSecurityToken _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogClient" /> class.
        /// </summary>
        /// <param name="azDatabaseInfoAppConfigSettings">The az catalog application configuration settings.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// /// <param name="token">The access token.</param>
        /// <exception cref="ArgumentNullException">
        /// azCatalogAppConfigSettings
        /// or
        /// httpClient
        /// or
        /// token
        /// </exception>
        public MavimDatabaseInfoClient(
            IOptionsSnapshot<AzDatabaseInfoAppConfigSettings> azDatabaseInfoAppConfigSettings,
            HttpClient httpClient,
            IJwtSecurityToken token)
        {
            _azDatabaseInfoAppConfigSettings = azDatabaseInfoAppConfigSettings?.Value ?? throw new ArgumentNullException(nameof(azDatabaseInfoAppConfigSettings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// Gets all valid databases of the user.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IMavimDatabaseInfo>> GetMavimDatabaseInfoList()
        {
            string requestUri = $"{_azDatabaseInfoAppConfigSettings.ApiEndpoint}";
            string responseJson = await GetDatabaseInfoResponse(requestUri);

            if (string.IsNullOrWhiteSpace(responseJson))
                throw new Exception(string.Format(Logging.NO_DATABASE_FOUND, "List"));

            List<MavimDatabaseInfoResponse> mavimDatabaseInfoListResponse =
                JsonConvert.DeserializeObject<List<MavimDatabaseInfoResponse>>(responseJson);

            IEnumerable<IMavimDatabaseInfo> database = mavimDatabaseInfoListResponse.Select(Map);

            return database;
        }

        /// <summary>
        /// Gets the mavim database information.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        public async Task<IMavimDatabaseInfo> GetMavimDatabaseInfo(Guid dbId)
        {
            string requestUri = $"{_azDatabaseInfoAppConfigSettings.ApiEndpoint}/{dbId}";
            string responseJson = await GetDatabaseInfoResponse(requestUri);

            if (string.IsNullOrWhiteSpace(responseJson))
                throw new Exception(string.Format(Logging.NO_DATABASE_FOUND, dbId));

            MavimDatabaseInfoResponse mavimDatabaseInfoResponse =
                JsonConvert.DeserializeObject<MavimDatabaseInfoResponse>(responseJson);

            return Map(mavimDatabaseInfoResponse);
        }

        /// <summary>
        /// Gets the database information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Internal server error</exception>
        private async Task<string> GetDatabaseInfoResponse(string requestUri)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Manager.Api.Utils.Constants.Configuration.BEARER, _token.RawData);

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new RequestNotFoundException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenRequestException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            return response.IsSuccessStatusCode
                        ? await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                        : throw new Exception($"Faild statuscode {response.StatusCode} by retrieving database at resource {requestUri}. Response: {response.RequestMessage}. Header: {response.Headers}");
        }

        private static IMavimDatabaseInfo Map(MavimDatabaseInfoResponse response) =>
            new MavimDatabaseInfo
            {
                Id = response.Id,
                DisplayName = response.DisplayName,
                ConnectionString = response.ConnectionString,
                Schema = response.Schema,
                TenantId = response.TenantId,
                ApplicationTenantId = response.ApplicationTenantId,
                ApplicationId = response.ApplicationId,
                ApplicationSecretKey = response.ApplicationSecretKey,
                IsInternalDatabase = response.IsInternalDatabase
            };
    }
}
