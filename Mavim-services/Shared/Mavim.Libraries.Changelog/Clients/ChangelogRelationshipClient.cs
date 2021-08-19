using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Changelog.Configuration;
using Mavim.Libraries.Changelog.Enums;
using Mavim.Libraries.Changelog.Interfaces;
using Mavim.Libraries.Changelog.Models;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
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
    public class ChangelogRelationshipClient : IChangelogRelationshipClient
    {
        private readonly AzChangelogRelationshipAppConfigSettings _azChangelogRelationshipAppConfigSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChangelogRelationshipClient> _logger;
        private readonly string _dataLanguage;
        private readonly IJwtSecurityToken _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogRelationshipClient" /> class.
        /// </summary>
        /// <param name="azChangelogRelationshipAppConfigSettings">The az changelog relationship application configuration settings.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="token">The access token.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="dataLanguage">The data language.</param>
        /// <exception cref="ArgumentNullException">azCatalogAppConfigSettings
        /// or
        /// httpClient
        /// or
        /// token</exception>
        public ChangelogRelationshipClient(
            IOptionsSnapshot<AzChangelogRelationshipAppConfigSettings> azChangelogRelationshipAppConfigSettings,
            HttpClient httpClient,
            IJwtSecurityToken token,
            ILogger<ChangelogRelationshipClient> logger,
            IDataLanguage dataLanguage)
        {
            _azChangelogRelationshipAppConfigSettings = azChangelogRelationshipAppConfigSettings?.Value ?? throw new ArgumentNullException(nameof(azChangelogRelationshipAppConfigSettings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _dataLanguage = dataLanguage != null ? Map(dataLanguage.Type) : throw new ArgumentNullException(nameof(dataLanguage));
        }

        /// <summary>
        /// Gets all changed relationships of the topic.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="topicId">The dcvId.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogRelationship>> GetRelations(Guid dbId, string topicId)
        {
            CheckInputParameters(dbId, topicId);

            string requestUri = $"{_azChangelogRelationshipAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/relationships/topics/{topicId}";
            string responseJson = await GetChangelogRelationshipResponse(requestUri);

            List<ChangelogRelationshipResponse> changelogRelationshipListResponse =
                JsonConvert.DeserializeObject<List<ChangelogRelationshipResponse>>(responseJson);

            IEnumerable<IChangelogRelationship> relationships = changelogRelationshipListResponse.Select(Map);

            return relationships;
        }

        /// <summary>
        /// Gets the pending relationship changes of the topic if available.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="topicId">The dcvId.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogRelationship>> GetPendingRelations(Guid dbId, string topicId)
        {
            CheckInputParameters(dbId, topicId);

            string requestUri = $"{_azChangelogRelationshipAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/relationships/topics/{topicId}/pending";
            string responseJson = await GetChangelogRelationshipResponse(requestUri);

            List<ChangelogRelationshipResponse> changelogRelationshipListResponse =
                JsonConvert.DeserializeObject<List<ChangelogRelationshipResponse>>(responseJson);

            IEnumerable<IChangelogRelationship> relationships = changelogRelationshipListResponse.Select(Map);

            return relationships;
        }

        /// <summary>
        /// Gets all pending relationship changes of the database.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogRelationship>> GetAllPendingRelations(Guid dbId)
        {
            CheckInputParameters(dbId);

            string requestUri = $"{_azChangelogRelationshipAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/relationships/pending";
            string responseJson = await GetChangelogRelationshipResponse(requestUri);

            List<ChangelogRelationshipResponse> changelogRelationshipListResponse =
                JsonConvert.DeserializeObject<List<ChangelogRelationshipResponse>>(responseJson);

            IEnumerable<IChangelogRelationship> relationships = changelogRelationshipListResponse.Select(Map);

            return relationships;
        }

        /// <summary>
        /// Gets the relationship status of a topic.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="topicId">The dcvId.</param>
        /// <returns></returns>
        public async Task<ChangeStatus> GetRelationStatus(Guid dbId, string topicId)
        {
            CheckInputParameters(dbId, topicId);

            string requestUri = $"{_azChangelogRelationshipAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/relationships/topics/{topicId}/status";
            string responseJson = await GetChangelogRelationshipResponse(requestUri);

            ChangeStatus relationshipStatus =
                JsonConvert.DeserializeObject<ChangeStatus>(responseJson);

            return relationshipStatus;
        }

        /// <summary>
        /// Approves the pending title change of a topic.
        /// </summary>
        /// <returns></returns>
        public async Task<IChangelogRelationship> ApproveRelation(Guid dbId, Guid changelogId)
        {
            CheckInputParameters(dbId, changelogId);

            string requestUri = $"{_azChangelogRelationshipAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/relationships/{changelogId}/approve";
            string responseJson = await PatchChangelogRelationshipResponse(requestUri);

            ChangelogRelationshipResponse changelogRelationshipResponse =
                JsonConvert.DeserializeObject<ChangelogRelationshipResponse>(responseJson);

            IChangelogRelationship relationship = Map(changelogRelationshipResponse);

            return relationship;
        }

        /// <summary>
        /// Rejects the pending title change of a topic.
        /// </summary>
        /// <returns></returns>
        public async Task<IChangelogRelationship> RejectRelation(Guid dbId, Guid changelogId)
        {
            CheckInputParameters(dbId, changelogId);

            string requestUri = $"{_azChangelogRelationshipAppConfigSettings.ApiEndpoint}/v1/{dbId}/{_dataLanguage}/changelog/relationships/{changelogId}/reject";
            string responseJson = await PatchChangelogRelationshipResponse(requestUri);

            ChangelogRelationshipResponse changelogRelationshipResponse =
                JsonConvert.DeserializeObject<ChangelogRelationshipResponse>(responseJson);

            IChangelogRelationship relationship = Map(changelogRelationshipResponse);

            return relationship;
        }

        #region Private Methods
        private void CheckInputParameters(Guid dbId)
        {
            if (dbId != Guid.Empty) return;
            _logger.LogError("dbId argument empty");
            throw new ArgumentNullException(nameof(dbId));
        }

        private void CheckInputParameters(Guid dbId, string dcvId)
        {
            CheckInputParameters(dbId);

            if (!string.IsNullOrEmpty(dcvId)) return;
            _logger.LogError("dcvId is null or empty");
            throw new ArgumentNullException(nameof(dcvId));
        }

        private void CheckInputParameters(Guid dbId, Guid changelogId)
        {
            CheckInputParameters(dbId);
            if (changelogId != Guid.Empty) return;
            _logger.LogError("changelogId is null or empty");
            throw new ArgumentNullException(nameof(changelogId));
        }

        /// <summary>
        /// Gets the requested changelog title resource and returns the response.
        /// </summary>
        /// <param name="requestUri">The requested resource.</param>
        /// <returns></returns>
        /// <exception cref="RequestNotFoundException">Request not found</exception>
        /// <exception cref="ForbiddenRequestException">User is not authorized to make the request</exception>
        /// <exception cref="Exception">Server error when requesting resource</exception>
        private async Task<string> GetChangelogRelationshipResponse(string requestUri)
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

        /// <summary>
        /// Patches the requested changelog title resource and returns the response.
        /// </summary>
        /// <param name="requestUri">The requested resource.</param>
        /// <returns></returns>
        /// <exception cref="RequestNotFoundException">Request not found</exception>
        /// <exception cref="ForbiddenRequestException">User is not authorized to make the request</exception>
        /// <exception cref="Exception">Server error when requesting resource</exception>
        private async Task<string> PatchChangelogRelationshipResponse(string requestUri)
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

        private static IChangelogRelationship Map(ChangelogRelationshipResponse response) =>
            response == null ? null :
            new ChangelogRelationship
            {
                ChangelogId = response.ChangelogId,
                InitiatorUserEmail = response.InitiatorUserEmail,
                ReviewerUserEmail = response.ReviewerUserEmail,
                TimestampChanged = response.TimestampChanged,
                TimestampApproved = response.TimestampApproved,
                TopicDcv = response.TopicDcv,
                Status = response.Status,
                RelationDcv = response.RelationDcv,
                FromCategory = response.FromCategory,
                FromTopicDcv = response.FromTopicDcv,
                ToCategory = response.ToCategory,
                ToTopicDcv = response.ToTopicDcv
            };

        private static string Map(Middlewares.Language.Enums.DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                Middlewares.Language.Enums.DataLanguageType.Dutch => "nl",
                Middlewares.Language.Enums.DataLanguageType.English => "en",
                _ => throw new ArgumentException($"unsupported DataLanguage: {dataLanguage.ToString()}")
            };
    }
    #endregion
}
