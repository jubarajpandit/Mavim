using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Libraries.Wopi.Configuration;
using Mavim.Libraries.Wopi.Interfaces;
using Mavim.Libraries.Wopi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Clients
{
    public class WopiHostClient : IWopiHostClient
    {
        private readonly AzWopiHostAppConfigSettings _azWopiHostAppConfigSettings;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly IJwtSecurityToken _token;
        private readonly IDataLanguage _dataLanguage;
        private readonly string Dutch = "nl";
        private readonly string English = "en";

        public WopiHostClient(
            IOptionsSnapshot<AzWopiHostAppConfigSettings> azWopiHostAppConfigSettings,
            HttpClient httpClient,
            IDataLanguage dataLanguage,
            ILogger<WopiHostClient> logger,
            IJwtSecurityToken token)
        {
            _azWopiHostAppConfigSettings = azWopiHostAppConfigSettings?.Value ?? throw new ArgumentNullException(nameof(azWopiHostAppConfigSettings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _dataLanguage = dataLanguage ?? throw new ArgumentNullException(nameof(dataLanguage));
        }

        public async Task<IFileInfo> GetFileInfo(Guid dbId, string dcvId)
        {
            string dataLanguage = Map(_dataLanguage.Type);
            string requestUri = $"{_azWopiHostAppConfigSettings.ApiEndpoint}/v1/{dbId}/{dataLanguage}/description/wopi/files/{dcvId}";
            _logger.LogTrace($"WopiHost Client requesting GetFileInfo at uri: {requestUri}");

            string responseJson = await GetWopiResponse(requestUri);

            CheckFileInfo createAuthorizationResponse = JsonConvert.DeserializeObject<CheckFileInfo>(responseJson);

            IFileInfo fileInfo = Map(createAuthorizationResponse);

            return fileInfo;
        }

        /// <summary>
        /// Gets the Authorization response.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Internal server error</exception>
        private async Task<string> GetWopiResponse(string requestUri)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Manager.Api.Utils.Constants.Configuration.BEARER, _token.RawData);

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private static IFileInfo Map(CheckFileInfo response) =>
            new FileInfo
            {
                BaseFileName = response.BaseFileName,
                Size = response.Size
            };

        private string Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => Dutch,
                DataLanguageType.English => English,
                _ => throw new RequestNotFoundException("DataLanguage not supported")
            };
    }
}
