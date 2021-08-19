using Mavim.Libraries.Middlewares.WopiValidator.Helpers;
using Mavim.Libraries.Middlewares.WopiValidator.Models;
using Mavim.Libraries.Middlewares.WopiValidator.Enums;
using Mavim.Manager.Api.Utils.AzAppConfiguration;
using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Services
{
    public class WopiActionUrlMetadataService : IWopiActionUrlMetadataService
    {
        private readonly ILogger<WopiActionUrlMetadataService> _logger;
        private readonly WopiSettings _wopiConfigSettings;
        private readonly IWopiDiscoveryCache _wopiDiscoveryCache;
        private readonly IFeatureManager _featureManager;
        public WopiActionUrlMetadataService(IWopiDiscoveryCache wopiDiscoveryCache,
            IOptionsSnapshot<WopiSettings> wopiConfigSettings, ILogger<WopiActionUrlMetadataService> logger, IFeatureManager featureManager)
        {
            _wopiDiscoveryCache = wopiDiscoveryCache ?? throw new ArgumentNullException(nameof(wopiDiscoveryCache));
            _wopiConfigSettings = wopiConfigSettings.Value ?? throw new ArgumentNullException(nameof(wopiConfigSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// Gets the wopi source url information.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Id is null or empty. - id
        /// or
        /// Token is null or empty. - token</exception>
        public async Task<IWopiActionUrlMetaData> GetWopiSourceMetadata()
        {
            IWopiActionUrlMetaData wopiActionUrlMetadata = await _wopiDiscoveryCache.GetOrAddWopiSrcFromCache(_wopiConfigSettings.DiscoveryUrl,
                _wopiConfigSettings.ProofKeyInfoCacheExpirationInMinutes, _wopiConfigSettings.SupportedWopiQueryParameterValues);

            IWopiActionUrlMetaData wopiSourceMetaData = await _featureManager.IsEnabledAsync(nameof(WopiFeatureFlags.WopiApiEditNew))
                ?
                new WopiActionUrlMetadata
                {
                    WopiTestViewerActionUrl = wopiActionUrlMetadata.WopiTestViewerActionUrl,
                    WordViewerActionUrl = wopiActionUrlMetadata.WordViewerActionUrl,
                    WordEditorActionUrl = wopiActionUrlMetadata.WordEditorActionUrl,
                    WordNewEditorActionUrl = wopiActionUrlMetadata.WordNewEditorActionUrl,
                    VisioViewerActionUrl = wopiActionUrlMetadata.VisioViewerActionUrl
                } 
                :
                new WopiActionUrlMetadata
                {
                    WopiTestViewerActionUrl = wopiActionUrlMetadata.WopiTestViewerActionUrl,
                    WordViewerActionUrl = wopiActionUrlMetadata.WordViewerActionUrl,
                    WordEditorActionUrl = wopiActionUrlMetadata.WordEditorActionUrl,                    
                    VisioViewerActionUrl = wopiActionUrlMetadata.VisioViewerActionUrl
                };
            return wopiSourceMetaData;
        }
    }
}
