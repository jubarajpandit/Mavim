using LazyCache;
using Mavim.Libraries.Features.Enums;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.WopiValidator.Constants;
using Mavim.Libraries.Middlewares.WopiValidator.Models;
using Mavim.Libraries.Middlewares.WopiValidator.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mavim.Libraries.Middlewares.WopiValidator.Helpers
{
    public class WopiDiscoveryCache : IWopiDiscoveryCache
    {
        private readonly string WOPI_PROOF_KEY_INFO_CACHE = "WOPIPROOFKEYINFOCACHE";
        private readonly ILogger<WopiDiscoveryCache> _wopiDiscoveryCacheLogger;
        private readonly IAppCache _wopiDiscoveryCache = new CachingService();
        private IFeatureManager _featureManager { get; }
        private HttpClient _httpClient;

        public WopiDiscoveryCache(IAppCache wopiDiscoveryCache, ILogger<WopiDiscoveryCache> wopiDiscoveryCacheLogger, HttpClient httpClient, IFeatureManager featureManager)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _wopiDiscoveryCache = wopiDiscoveryCache ?? throw new ArgumentNullException(nameof(wopiDiscoveryCache));
            _wopiDiscoveryCacheLogger = wopiDiscoveryCacheLogger ?? throw new ArgumentNullException(nameof(wopiDiscoveryCacheLogger));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// Gets or adds wopi proof from cache.
        /// </summary>
        /// <param name="discoveryUrl">The discovery URL.</param>
        /// <param name="proofKeyInfoCacheExpirationInMinutes">The proof key information cache expiration in minutes.</param>
        /// <returns></returns>
        public async Task<WopiProof> GetOrAddWopiProofFromCache(string discoveryUrl, int proofKeyInfoCacheExpirationInMinutes)
        {
            WopiProof wopiProofCacheTask = await _wopiDiscoveryCache.GetOrAdd(WOPI_PROOF_KEY_INFO_CACHE, async cacheEntry =>
            {
                _wopiDiscoveryCacheLogger.LogDebug($"Fetching proof values from discovery url: {discoveryUrl}");

                return await GetOrAddWopiProofFromCache(cacheEntry, discoveryUrl, proofKeyInfoCacheExpirationInMinutes);
            });

            return wopiProofCacheTask;
        }

        /// <summary>
        /// Gets the or add wopi source from cache.
        /// </summary>
        /// <param name="viewMode">The view mode.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="discoveryUrl">The discovery URL.</param>
        /// <param name="proofKeyInfoCacheExpirationInMinutes">The proof key information cache expiration in minutes.</param>
        /// <param name="supportedWopiQueryParameterValues">The supported wopi query parameter values.</param>
        /// <returns></returns>
        public async Task<IWopiActionUrlMetaData> GetOrAddWopiSrcFromCache(string discoveryUrl
                                , int proofKeyInfoCacheExpirationInMinutes
                                , string supportedWopiQueryParameterValues)
        {
            if (string.IsNullOrWhiteSpace(discoveryUrl))
                throw new ArgumentException("DiscoveryUrl was empty or not found.", nameof(discoveryUrl));

            if (string.IsNullOrWhiteSpace(supportedWopiQueryParameterValues))
                _wopiDiscoveryCacheLogger.LogDebug("No supported Wopi query parameters found from the app configuration.");

            IWopiActionUrlMetaData wopiActionUrls = await _wopiDiscoveryCache.GetOrAdd(WOPI_PROOF_KEY_INFO_CACHE, async cacheEntry =>
            {
                _wopiDiscoveryCacheLogger.LogDebug($"Fetching wopisrc from discovery url: {discoveryUrl}");

                return await GetWopiSrc(cacheEntry, discoveryUrl, proofKeyInfoCacheExpirationInMinutes, supportedWopiQueryParameterValues);
            });

            return wopiActionUrls;
        }

        /// <summary>
        /// Gets the or add wopi proof from cache.
        /// </summary>
        /// <param name="cacheEntry">The cache entry.</param>
        /// <param name="discoveryUrl">The discovery URL.</param>
        /// <param name="proofKeyInfoCacheExpirationInMinutes">The proof key information cache expiration in minutes.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">httpClient</exception>
        private async Task<WopiProof> GetOrAddWopiProofFromCache(ICacheEntry cacheEntry, string discoveryUrl, int proofKeyInfoCacheExpirationInMinutes)
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync(discoveryUrl))
            {
                if (!response.IsSuccessStatusCode) return null;

                cacheEntry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(proofKeyInfoCacheExpirationInMinutes);
                // Need to see if we need sliding expiration later on.
                //cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(wopiConfigSettings.ProofKeyInfoCacheExpirationInMinutes);

                string discoveryXmlString = await response.Content.ReadAsStringAsync();
                XDocument discoveryXml = XDocument.Parse(discoveryXmlString);
                XElement proof = discoveryXml.Descendants(WopiDiscoveryXmlNodes.PROOF_KEY).FirstOrDefault();

                string currentCspBlobValue = proof.Attribute(WopiDiscoveryXmlNodes.CURRENT_PROOF_KEY_VALUE).Value;
                string currentModulusValue = proof.Attribute(WopiDiscoveryXmlNodes.CURRENT_PROOF_KEY_MODULUS).Value;
                string currentExponentValue = proof.Attribute(WopiDiscoveryXmlNodes.CURRENT_PROOF_KEY_EXPONENT).Value;
                string oldCspBlobValue = proof.Attribute(WopiDiscoveryXmlNodes.CURRENT_PROOF_KEY_OLD_VALUE).Value;
                string oldModulusValue = proof.Attribute(WopiDiscoveryXmlNodes.CURRENT_PROOF_KEY_OLD_MODULUS).Value;
                string oldExponentValue = proof.Attribute(WopiDiscoveryXmlNodes.CURRENT_PROOF_KEY_OLD_EXPONENT).Value;

                WopiProofKeyInfo currentWopiProofKeyInfo = new WopiProofKeyInfo(currentCspBlobValue, currentExponentValue, currentModulusValue);
                WopiProofKeyInfo oldWopiProofKeyInfo = new WopiProofKeyInfo(oldCspBlobValue, oldExponentValue, oldModulusValue);

                return new WopiProof(currentWopiProofKeyInfo, oldWopiProofKeyInfo);
            };
        }

        /// <summary>
        /// Gets the wopi source.
        /// </summary>
        /// <param name="cacheEntry">The cache entry.</param>
        /// <param name="discoveryUrl">The discovery URL.</param>
        /// <param name="proofKeyInfoCacheExpirationInMinutes">The proof key information cache expiration in minutes.</param>
        /// <param name="supportedWopiQueryParameterValues">The supported wopi query parameter values.</param>
        /// <returns></returns>
        private async Task<WopiActionUrlMetadata> GetWopiSrc(ICacheEntry cacheEntry, string discoveryUrl, int proofKeyInfoCacheExpirationInMinutes, string supportedWopiQueryParameterValues)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(discoveryUrl);

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException($"Invalid response code ({response.StatusCode}) from discovery URL ({discoveryUrl}).");

            cacheEntry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(proofKeyInfoCacheExpirationInMinutes);
            // Need to see if we need sliding expiration later on.
            // cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(wopiConfigSettings.ProofKeyInfoCacheExpirationInMinutes);

            string discoveryXmlString = await response.Content.ReadAsStringAsync();
            XDocument discoveryXml = XDocument.Parse(discoveryXmlString);
            WopiActionUrlMetadata wopiActionUrls = await GetWopiActionUrls(supportedWopiQueryParameterValues, discoveryXml);

            return wopiActionUrls;
        }

        /// <summary>
        /// Gets the wopi action urls.
        /// </summary>
        /// <param name="supportedWopiQueryParameterValues">The supported wopi query parameter values.</param>
        /// <param name="discoveryXml">The discovery XML.</param>
        /// <returns></returns>
        private async Task<WopiActionUrlMetadata> GetWopiActionUrls(string supportedWopiQueryParameterValues, XDocument discoveryXml)
        {           
            string wopiTestViewerActionUrl = GetActionUrl(discoveryXml, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_WOPITEST, supportedWopiQueryParameterValues
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTEVALUE_VIEW, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT_WOPITEST);

            string wopiWordViewerActionUrl = GetActionUrl(discoveryXml, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_WORD, supportedWopiQueryParameterValues
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTEVALUE_INTERACTIVE_PREVIEW, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT_DOCX);

            string wopiWordEditorActionUrl = GetActionUrl(discoveryXml, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_WORD, supportedWopiQueryParameterValues
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTEVALUE_EDIT, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT_DOCX);

            string wopiWordNewEditorActionUrl = GetActionUrl(discoveryXml, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_WORD, supportedWopiQueryParameterValues
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTEVALUE_EDITNEW, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT_DOCX);

            string wopiVisioViewerActionUrl = await _featureManager.IsEnabledAsync(nameof(ChartNavigationFeature.ChartNavigation))
                ? GetActionUrl(discoveryXml, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_VISIO, supportedWopiQueryParameterValues
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTEVALUE_EMBED_VIEW, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT_VSD)
                : GetActionUrl(discoveryXml, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_VISIO, supportedWopiQueryParameterValues
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTEVALUE_INTERACTIVE_PREVIEW, WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT
                            , WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_EXT_VSD);

            WopiActionUrlMetadata wopiActionUrlMetadata = await _featureManager.IsEnabledAsync(nameof(WopiFeatureFlags.WopiApiEditNew))
                ?
                new WopiActionUrlMetadata
                {
                    WopiTestViewerActionUrl = wopiTestViewerActionUrl,
                    WordViewerActionUrl = wopiWordViewerActionUrl,
                    WordEditorActionUrl = wopiWordEditorActionUrl,
                    WordNewEditorActionUrl = wopiWordNewEditorActionUrl,
                    VisioViewerActionUrl = wopiVisioViewerActionUrl
                }
                :
                new WopiActionUrlMetadata
                {
                    WopiTestViewerActionUrl = wopiTestViewerActionUrl,
                    WordViewerActionUrl = wopiWordViewerActionUrl,
                    WordEditorActionUrl = wopiWordEditorActionUrl,
                    VisioViewerActionUrl = wopiVisioViewerActionUrl
                };
            return wopiActionUrlMetadata;
        }

        /// <summary>
        /// Gets the action URL.
        /// </summary>
        /// <param name="discoveryXml">The discovery XML.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="supportedWopiQueryParameterValues">The supported wopi query parameter values.</param>
        /// <param name="modeName">Name of the mode.</param>
        /// <param name="extensionAttributeName">Name of the extension attribute.</param>
        /// <param name="extensionValue">The extension value.</param>
        /// <returns></returns>
        private static string GetActionUrl(XDocument discoveryXml, string appName, string supportedWopiQueryParameterValues
                            , string modeName, string extensionAttributeName, string extensionValue)
        {
            XElement xElement = discoveryXml.Descendants(WopiDiscoveryXmlNodes.DISCOVERY_ELEMENT_APP).FirstOrDefault(xe =>
                                                xe.Attribute(WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_NAME).Value.Equals(appName));

            XElement app = xElement?.Descendants(WopiDiscoveryXmlNodes.DISCOVERY_ELEMENT_ACTION).FirstOrDefault(xe =>
                                        xe.Attribute(WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_NAME)
                                        .Value.Equals(modeName) && xe.Attribute(extensionAttributeName)
                                        .Value.Equals(extensionValue));

            if (app == null) throw new Exception($"Found no XElement from WopiDiscoveryXmlNodes for: {extensionValue} and attribute {extensionAttributeName}.");

            string wopiUrlSrc = app.Attribute(WopiDiscoveryXmlNodes.DISCOVERY_ATTRIBUTE_URLSRC).Value;

            string wopiUrlSrcBase = wopiUrlSrc.Substring(0, wopiUrlSrc.IndexOf('<'));
            string wopiUrlSrcParameters = wopiUrlSrc.Substring(wopiUrlSrc.IndexOf('<'));

            List<string> queryParameters = Regex.Matches(wopiUrlSrcParameters, @"\<(.+?)\>")
                                .Cast<Match>()
                                .Select(m => m.Groups[1].Value).ToList();

            List<string> supportedQueryParameterValues = supportedWopiQueryParameterValues?.TrimEnd(';').Split(';').ToList();

            StringBuilder stringBuilder = new StringBuilder();

            queryParameters.ForEach(p =>
            {
                string key = p.Split('=')[0];

                string supportedQueryParameter = supportedQueryParameterValues?.FirstOrDefault(sp => sp.Split('=')[0].Equals(key));

                if (!string.IsNullOrWhiteSpace(supportedQueryParameter))
                    stringBuilder.Append($"{supportedQueryParameter}&");
            });

            wopiUrlSrcBase += stringBuilder.ToString().TrimEnd('&');

            return wopiUrlSrcBase;
        }
    }
}