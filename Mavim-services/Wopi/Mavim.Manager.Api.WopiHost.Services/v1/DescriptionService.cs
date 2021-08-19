using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Utils.AzAppConfiguration;
using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;
using Mavim.Manager.Api.WopiHost.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;
using RepositoryInterfaces = Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.WopiHost.Services.v1
{
    public class DescriptionService : IDescriptionService
    {
        //OwnerId has to be Unique and consistent for WOPI and since there is no Owner maintained in the Mavim Manager, we hard code it as of now.
        private const string _ownerId = "administrator@wopihost.com";
        private const string JwtUserIdKey = "oid"; // the object id in the jwt token is unique per AD user.
        private const string JwtNameKey = "name"; // the name in the jwt token is unique per AD user.
        private readonly ILogger<DescriptionService> _logger;
        private readonly RepositoryInterfaces.IDescriptionRepository _descriptionRepository;
        private readonly WopiSettings _wopiConfigSettings;
        private readonly string _dataLanguage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionService"/> class.
        /// </summary>
        /// <param name="onBehalfOfTokenProvider">The on behalf of token provider.</param>
        /// <param name="logger">The logger.</param>
        public DescriptionService(RepositoryInterfaces.IDescriptionRepository repository,
            IOptionsSnapshot<WopiSettings> wopiConfigSettings, ILogger<DescriptionService> logger, IDataLanguage dataLanguage)
        {
            _wopiConfigSettings = wopiConfigSettings.Value ?? throw new ArgumentNullException(nameof(wopiConfigSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _descriptionRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _dataLanguage = Map(dataLanguage.Type);
        }

        /// <summary>
        /// Gets the content of the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Stream> GetFileContent(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Id is null or empty.", nameof(id));

            return await _descriptionRepository.GetFileContent(id);
        }

        /// <summary>
        /// Updates the description.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="descriptionStream">The description stream.</param>
        /// <returns></returns>
        public async Task UpdateDescription(string id, Stream descriptionStream)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Id is null or empty.", nameof(id));

            if (descriptionStream is null) throw new ArgumentNullException(nameof(descriptionStream));

            await _descriptionRepository.UpdateDescription(id, descriptionStream);
        }

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ICheckFileInfo> GetFileInfo(string id, string token)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Id is null or empty.", nameof(id));
            if (string.IsNullOrEmpty(token)) throw new ArgumentException("Token is null or empty.", nameof(token));

            ICheckFileInfo fileInfo = await GetFIleInfo(id, token);

            return fileInfo;
        }

        private async Task<ICheckFileInfo> GetFIleInfo(string id, string token)
        {
            RepositoryInterfaces.ICheckFileInfo fileInfo = await _descriptionRepository.CheckFileInfo(id);

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token)) throw new Exception("Could not read Jwt token from the string.");

            JwtSecurityToken jwtSecurityToken = jwtHandler.ReadJwtToken(token);

            if (!jwtSecurityToken.Payload.ContainsKey(JwtUserIdKey)) throw new Exception("Oid not found in the token.");

            if (!jwtSecurityToken.Payload.ContainsKey(JwtNameKey)) throw new Exception("Name key not found in the token.");

            if (string.IsNullOrWhiteSpace(_wopiConfigSettings.HostEditUrl)) throw new Exception("HostEditUrl is not found or empty.");

            string hostEditUrl = _wopiConfigSettings.HostEditUrl;
            hostEditUrl = hostEditUrl.TrimEnd('/');
            hostEditUrl = hostEditUrl.Replace("{DataLanguage}", _dataLanguage, StringComparison.InvariantCultureIgnoreCase);

            _logger.LogDebug($"Host edit URL: {hostEditUrl}");

            ICheckFileInfo checkFileInfo = new CheckFileInfo
            {
                BaseFileName = $"{id}.docx",
                SHA256 = fileInfo.ComputedFileHash,
                Version = fileInfo.ComputedFileHash,
                Size = fileInfo.FileSize,
                SupportsUpdate = true,
                // UserCanNotWriteRelative to be set to true always as we don't support put relative file yet (https://wopi.readthedocs.io/projects/wopirest/en/latest/files/PutRelativeFile.html)
                UserCanNotWriteRelative = true,
                SupportsLocks = true,
                SupportsGetLock = true,
                IsAnonymousUser = false,
                LicenseCheckForEditIsEnabled = true,
                HostEditUrl = $"{hostEditUrl}/{id}",
                ReadOnly = false,
                UserCanWrite = true,
                UserId = jwtSecurityToken.Payload[JwtUserIdKey].ToString(),
                OwnerId = _ownerId,
                UserFriendlyName = jwtSecurityToken.Payload[JwtNameKey].ToString()
            };

            return checkFileInfo;
        }

        private static string Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => "nl",
                DataLanguageType.English => "en",
                _ => throw new ArgumentException(String.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };
    }
}
