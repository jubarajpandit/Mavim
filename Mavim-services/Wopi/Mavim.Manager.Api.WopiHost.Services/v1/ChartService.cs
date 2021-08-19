using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;
using Mavim.Manager.Api.WopiHost.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;
using RepositoryInterfaces = Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.WopiHost.Services.v1
{
    public class ChartService : IChartService
    {
        //OwnerId has to be Unique and consistent for WOPI and since there is no Owner maintained in the Mavim Manager, we hard code it as of now.
        private const string _ownerId = "administrator@wopihost.com";
        private const string JwtUserIdKey = "oid"; // the object id in the jwt token is unique per AD user.
        private const string JwtNameKey = "name"; // the name in the jwt token is unique per AD user.
        private readonly RepositoryInterfaces.IChartRepository _chartRepository;
        private readonly IFeatureManager _featureManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartService"/> class.
        /// </summary>
        /// <param name="onBehalfOfTokenProvider">The on behalf of token provider.</param>
        /// <param name="logger">The logger.</param>
        public ChartService(RepositoryInterfaces.IChartRepository repository, ILogger<ChartService> logger, IFeatureManager featureManager)
        {
            _chartRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// Gets the content of the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="access_token">The access token.</param>
        /// <returns></returns>
        public Task<Stream> GetFileContent(string id, string access_token)
        {
            return _chartRepository.GetFileContent(id);
        }

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="token">The access token.</param>
        /// <returns></returns>
        public async Task<ICheckFileInfo> GetFileInfo(string id, string token, string embeddingPageOrigin, string embeddingPageSessionInfo)
        {
            RepositoryInterfaces.ICheckFileInfo repositoryFileInfo = await _chartRepository.CheckFileInfo(id);

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token)) throw new Exception("Could not read Jwt token from the string.");

            JwtSecurityToken jwtSecurityToken = jwtHandler.ReadJwtToken(token);

            if (!jwtSecurityToken.Payload.ContainsKey(JwtUserIdKey)) throw new Exception("Oid not found in the token.");

            if (!jwtSecurityToken.Payload.ContainsKey(JwtNameKey)) throw new Exception("Name key not found in the token.");

            ICheckFileInfo fileInfo = await _featureManager.IsEnabledAsync(nameof(ChartNavigationFeature.ChartNavigation))
                ? new CheckFileInfo
                {
                    BaseFileName = $"{id}.vsd",
                    SHA256 = repositoryFileInfo.ComputedFileHash,
                    //// Check to see if we can somehow return a meaningful file Version, for now returning a tick because it represents a unique value of the current time
                    //// This unique tick is now sent because the Word Viewer uses cache to server the doc files and checks the version of the file to serve from the cache.
                    //// If the description is updated in Mavim, it would not result in being fetched from API as there is no versioning of the description as such.
                    //// Hence returning a tick would make sure that the version is always unique and hence the description will not be fetched from cache and instead would result in api call (WI:18310)
                    Version = repositoryFileInfo.ComputedFileHash,
                    Size = repositoryFileInfo.FileSize,
                    UserId = jwtSecurityToken.Payload[JwtUserIdKey].ToString(),
                    OwnerId = _ownerId,
                    UserFriendlyName = jwtSecurityToken.Payload[JwtNameKey].ToString(),
                    EmbeddingPageOrigin = embeddingPageOrigin,
                    EmbeddingPageSessionInfo = embeddingPageSessionInfo
                }
                : new CheckFileInfo
                {
                    BaseFileName = $"{id}.vsd",
                    SHA256 = repositoryFileInfo.ComputedFileHash,
                    Version = repositoryFileInfo.ComputedFileHash,
                    Size = repositoryFileInfo.FileSize,
                    UserId = jwtSecurityToken.Payload[JwtUserIdKey].ToString(),
                    OwnerId = _ownerId,
                    UserFriendlyName = jwtSecurityToken.Payload[JwtNameKey].ToString()
                };

            return fileInfo;
        }
    }
}
