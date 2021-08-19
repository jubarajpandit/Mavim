using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Changelog.Configuration;
using Mavim.Libraries.Changelog.Enums;
using Mavim.Libraries.Changelog.Interfaces;
using Mavim.Libraries.Changelog.Models;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mavim.Libraries.Changelog.Clients
{
    public class ChangelogTitleClient : IChangelogTitleClient
    {
        private readonly AzChangelogTitleAppConfigSettings _azChangelogTitleAppConfigSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChangelogTitleClient> _logger;
        private readonly string _dataLanguage;

        private readonly IJwtSecurityToken _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogTitleClient" /> class.
        /// </summary>
        /// <param name="azChangelogTitleAppConfigSettings">The az changelogtitle application configuration settings.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// /// <param name="token">The access token.</param>
        /// <exception cref="ArgumentNullException">
        /// azCatalogAppConfigSettings
        /// or
        /// httpClient
        /// or
        /// token
        /// </exception>
        public ChangelogTitleClient(
            IOptionsSnapshot<AzChangelogTitleAppConfigSettings> azChangelogTitleAppConfigSettings,
            HttpClient httpClient,
            IJwtSecurityToken token,
            ILogger<ChangelogTitleClient> logger,
            Middlewares.Language.Interfaces.IDataLanguage dataLanguage)
        {
            _azChangelogTitleAppConfigSettings = azChangelogTitleAppConfigSettings?.Value ?? throw new ArgumentNullException(nameof(azChangelogTitleAppConfigSettings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _dataLanguage = dataLanguage != null ? Map(dataLanguage.Type) : throw new ArgumentNullException(nameof(dataLanguage));
        }

        /// <summary>
        /// Gets all changed titles of the topic.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogTitle>> GetTitles(Guid dbId, string dcvId)
        {
            CheckInputParameters(dbId, dcvId);

            string requestUri = $"{_azChangelogTitleAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/titles/topic/{dcvId}";
            string responseJson = await GetChangelogTitleResponse(requestUri);

            List<ChangelogTitleResponse> changelogTitleListResponse =
                JsonConvert.DeserializeObject<List<ChangelogTitleResponse>>(responseJson);

            IEnumerable<IChangelogTitle> titles = changelogTitleListResponse.Select(Map);

            return titles;
        }

        /// <summary>
        /// Gets the pending title changes of the topic if available.
        /// </summary>
        /// <returns></returns>
        public async Task<IChangelogTitle> GetPendingTitle(Guid dbId, string dcvId)
        {
            CheckInputParameters(dbId, dcvId);

            string requestUri = $"{_azChangelogTitleAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/titles/topic/{dcvId}/pending";
            string responseJson = await GetChangelogTitleResponse(requestUri);

            ChangelogTitleResponse changelogTitleListResponse =
                JsonConvert.DeserializeObject<ChangelogTitleResponse>(responseJson);

            IChangelogTitle title = Map(changelogTitleListResponse);

            return title;
        }

        /// <summary>
        /// Gets all pending title changes of the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogTitle>> GetAllPendingTitles(Guid dbId)
        {
            CheckInputParameters(dbId);

            string requestUri = $"{_azChangelogTitleAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/titles/pending";
            string responseJson = await GetChangelogTitleResponse(requestUri);

            List<ChangelogTitleResponse> changelogTitleListResponse =
                JsonConvert.DeserializeObject<List<ChangelogTitleResponse>>(responseJson);

            IEnumerable<IChangelogTitle> titles = changelogTitleListResponse.Select(Map);

            return titles;
        }

        /// <summary>
        /// Gets the title status.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<ChangeStatus> GetTitleStatus(Guid dbId, string dcvId)
        {
            CheckInputParameters(dbId, dcvId);

            string requestUri = $"{_azChangelogTitleAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/titles/topic/{dcvId}/status";
            string responseJson = await GetChangelogTitleResponse(requestUri);

            ChangeStatus titleStatus =
                JsonConvert.DeserializeObject<ChangeStatus>(responseJson);

            return titleStatus;
        }

        /// <summary>
        /// Approves the pending title change of a topic.
        /// </summary>
        /// <returns></returns>
        public async Task<IChangelogTitle> ApproveTitle(Guid dbId, string dcvId)
        {
            CheckInputParameters(dbId, dcvId);

            string requestUri = $"{_azChangelogTitleAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/titles/topic/{dcvId}/approve";
            string responseJson = await PatchChangelogTitleResponse(requestUri);

            ChangelogTitleResponse changelogTitleListResponse =
                JsonConvert.DeserializeObject<ChangelogTitleResponse>(responseJson);

            IChangelogTitle title = Map(changelogTitleListResponse);

            return title;
        }

        /// <summary>
        /// Rejects the pending title change of a topic.
        /// </summary>
        /// <returns></returns>
        public async Task<IChangelogTitle> RejectTitle(Guid dbId, string dcvId)
        {
            CheckInputParameters(dbId, dcvId);

            string requestUri = $"{_azChangelogTitleAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/titles/topic/{dcvId}/reject";
            string responseJson = await PatchChangelogTitleResponse(requestUri);

            ChangelogTitleResponse changelogTitleListResponse =
                JsonConvert.DeserializeObject<ChangelogTitleResponse>(responseJson);

            IChangelogTitle title = Map(changelogTitleListResponse);

            return title;
        }

        #region Private Methods
        private void CheckInputParameters(Guid dbid)
        {
            if (dbid != Guid.Empty) return;
            _logger.LogError("dbid argument empty");
            throw new ArgumentNullException(nameof(dbid));
        }

        private void CheckInputParameters(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid);

            if (!string.IsNullOrEmpty(dcvid)) return;
            _logger.LogError("dcvid is null or empty");
            throw new ArgumentNullException(nameof(dcvid));
        }

        /// <summary>
        /// Gets the requested changelog title resource and returns the response.
        /// </summary>
        /// <param name="requestUri">The requested resource.</param>
        /// <returns></returns>
        /// <exception cref="RequestNotFoundException">Request not found</exception>
        /// <exception cref="ForbiddenRequestException">User is not authorized to make the request</exception>
        /// <exception cref="Exception">Server error when requesting resource</exception>
        private async Task<string> GetChangelogTitleResponse(string requestUri)
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
                        : throw new Exception($"Failed statuscode {response.StatusCode} by retrieving resource {requestUri}. Response: {response.RequestMessage}. Header: {response.Headers}");
        }

        private async Task<string> PatchChangelogTitleResponse(string requestUri)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Manager.Api.Utils.Constants.Configuration.BEARER, _token.RawData);

            HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, null);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new RequestNotFoundException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenRequestException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            return response.IsSuccessStatusCode
                        ? await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                        : throw new Exception($"Failed statuscode {response.StatusCode} by patching resource {requestUri}. Response: {response.RequestMessage}. Header: {response.Headers}");
        }

        private static IChangelogTitle Map(ChangelogTitleResponse response) =>
            response == null ? null :
            new ChangelogTitle
            {
                ChangelogId = response.ChangelogId,
                InitiatorUserEmail = response.InitiatorUserEmail,
                ReviewerUserEmail = response.ReviewerUserEmail,
                TimestampChanged = response.TimestampChanged,
                TimestampApproved = response.TimestampApproved,
                TopicDcv = response.TopicDcv,
                Status = response.Status,
                FromTitleValue = response.FromTitleValue,
                ToTitleValue = response.ToTitleValue
            };

        private static string Map(Middlewares.Language.Enums.DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                Middlewares.Language.Enums.DataLanguageType.Dutch => "nl",
                Middlewares.Language.Enums.DataLanguageType.English => "en",
                _ => throw new ArgumentException(string.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };
    }
    #endregion
}
