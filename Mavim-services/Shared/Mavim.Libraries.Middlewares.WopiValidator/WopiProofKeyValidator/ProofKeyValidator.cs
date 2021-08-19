using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.WopiValidator.Constants;
using Mavim.Libraries.Middlewares.WopiValidator.Enums;
using Mavim.Libraries.Middlewares.WopiValidator.Helpers;
using Mavim.Libraries.Middlewares.WopiValidator.Models;
using Mavim.Manager.Api.Utils.AzAppConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.FeatureManagement;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConfigurationConstants = Mavim.Libraries.Middlewares.WopiValidator.Constants.Configuration;

namespace Mavim.Libraries.Middlewares.WopiValidator.WopiProofKeyValidator
{
    public class ProofKeyValidator : IProofKeyValidator
    {
        #region private members
        private readonly WopiSettings _wopiConfigSettings;
        private readonly ILogger<ProofKeyValidator> _wopiValidatorLogger;
        private readonly HttpClient _httpClient;
        private readonly IFeatureManager _featureManager;
        private readonly IWopiDiscoveryCache _wopidDiscoveryCache;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofKeyValidator" /> class.
        /// </summary>
        /// <param name="wopiDiscoveryCache">The wopi discovery cache.</param>
        /// <param name="wopiConfigSettings">The wopi configuration settings.</param>
        /// <param name="wopiValidationHandlerLogger">The authentication logger.</param>
        /// <param name="featureManager">The feature manager.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <exception cref="ArgumentNullException">authenticationHelper
        /// or
        /// azAuthorizationAppConfigSettings
        /// or
        /// authLogger</exception>
        /// <exception cref="System.ArgumentNullException">wopiConfigSettings
        /// or
        /// wopiValidationHandlerLogger</exception>
        public ProofKeyValidator(
              IWopiDiscoveryCache wopiDiscoveryCache,
              IOptionsSnapshot<WopiSettings> wopiConfigSettings,
              ILogger<ProofKeyValidator> wopiValidationHandlerLogger,
              IFeatureManager featureManager,
              HttpClient httpClient)
        {
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
            _wopiConfigSettings = wopiConfigSettings.Value ?? throw new ArgumentNullException(nameof(wopiConfigSettings));
            _wopiValidatorLogger = wopiValidationHandlerLogger ?? throw new ArgumentNullException(nameof(wopiValidationHandlerLogger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _wopidDiscoveryCache = wopiDiscoveryCache ?? throw new ArgumentNullException(nameof(wopiDiscoveryCache));
        }

        #region private methods
        /// <summary>Handles the authentication process asynchronously.</summary>
        /// <returns></returns>
        public async Task ValidateWopiProofKey(HttpContext context)
        {
            try
            {
                // WOPI requests don't have the access token always in the header, so check first to see if its in the header
                // and then follow to check in thr quiery string if its in there. Authorization header value is present when api
                // call is made from application
                if (!TryGetSecurityTokenFromHeader(context, out JwtSecurityToken jwtSecurityToken))
                    jwtSecurityToken = GetSecurityTokenFromQueryString(context);

                bool isWopiProofKeyValidationEnabled = await _featureManager.IsEnabledAsync(nameof(WopiFeatureFlags.WopiProofKeyValidation));

                //TODO: Remove all these console logs in the next PR.(WI:18011)
                if (_wopiConfigSettings != null)
                    Console.WriteLine($"WopiSettings: {_wopiConfigSettings.DiscoveryUrl} , {_wopiConfigSettings.ProofKeyInfoCacheExpirationInMinutes}");
                else
                    Console.WriteLine("Could not load Wopi configuration settings.  Please check the Azure app configuration settings for the corresponding environment.");


                bool wopiProofKeyValidationResult = await ProcessWopiRequest(context.Request);

                // If Proof validation fails, do not throw an exception with lot of proof specific details.
                if (!isWopiProofKeyValidationEnabled)
                {
                    Console.WriteLine($"Wopi Proof Key Validation Result: {wopiProofKeyValidationResult}. Wopi ProofKey Validation is not enabled. Proceeding further irrespective of the result of proof key validation.");
                    return;
                }

                if (!wopiProofKeyValidationResult)
                    throw new WopiProofValidationFailed("Internal server error");
                else
                    Console.WriteLine("Wopi Proof key validation succeeded.");
            }
            catch (WopiProofValidationFailed ex)
            {
                _wopiValidatorLogger.LogError(ex, "Wopi proof key validation failed.");
                throw;
            }
            catch (Exception ex)
            {
                _wopiValidatorLogger.LogError(ex, Logging.AUTH_TOKEN_FAILED);
                throw;
            }
        }

        /// <summary>
        /// Gets Access tijeb from header.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool TryGetAccessTokenFromHeader(HttpRequest req, out string accessToken)
        {
            accessToken = null;
            StringValues AuthorizationHeaders;

            if (!req.Headers.TryGetValue(ConfigurationConstants.AUTHORIZATION, out AuthorizationHeaders))
                return false;

            string AuthorizationHeader = AuthorizationHeaders.FirstOrDefault();

            if (String.IsNullOrWhiteSpace(AuthorizationHeader))
                return false;

            accessToken = RemoveBearerPrefixFromString(AuthorizationHeader);

            return true;
        }

        private static string RemoveBearerPrefixFromString(string AuthorizationHeader)
        {
            string accessToken;
            if (AuthorizationHeader.Split(' ')[0]
                .Equals(ConfigurationConstants.BEARER_TOKEN_PREFIX, StringComparison.InvariantCultureIgnoreCase))
            {
                accessToken = AuthorizationHeader.Split(' ')[1].Trim();
            }
            else
            {
                accessToken = AuthorizationHeader;
            }

            return accessToken;
        }

        /// <summary>
        /// Gets the security token from header.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Internal server error.</exception>
        private bool TryGetSecurityTokenFromHeader(HttpContext httpContext, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;
            try
            {
                string accessToken;

                if (!TryGetAccessTokenFromHeader(httpContext.Request, out accessToken))
                    return false;

                JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
                jwtSecurityToken = jwtHandler.ReadJwtToken(accessToken);

                if (jwtSecurityToken == null) return false;
                return true;
            }
            catch (Exception ex)
            {
                // This should not be a breaking exception, as WOPI calls from Microsoft doesn't always have a Authorization header, 
                // instead it is always present in the query string.  So, if the Authorization header is not found just handle it
                // and return false which should then trigger the check of access token in the query string.
                _wopiValidatorLogger.LogError(ex, Logging.MISSING_HEADER_FORMAT, ConfigurationConstants.AUTHORIZATION);
                return false;
            }
        }

        /// <summary>
        /// Gets the security token from query string.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Internal server error.</exception>
        private JwtSecurityToken GetSecurityTokenFromQueryString(HttpContext httpContext)
        {
            if (!httpContext.Request.Query.TryGetValue(ConfigurationConstants.ACCESS_TOKEN, out StringValues queryStringValue))
            {
                _wopiValidatorLogger.LogError(string.Format(Logging.MISSING_QUERY_STRING, ConfigurationConstants.ACCESS_TOKEN));
                throw new Exception("Internal server error.");
            }

            string accessToken = queryStringValue.FirstOrDefault();

            if (accessToken.StartsWith(ConfigurationConstants.BEARER_TOKEN_PREFIX, StringComparison.InvariantCultureIgnoreCase))
            {
                accessToken = accessToken.Substring(ConfigurationConstants.BEARER_TOKEN_PREFIX.Length).Trim();
            }

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.ReadJwtToken(accessToken);
        }

        /// <summary>
        /// Processes the wopi request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private async Task<bool> ProcessWopiRequest(HttpRequest request)
        {
            if (!request.Headers.TryGetValue(WopiRequestHeaders.PROOF, out StringValues wopiCurrentProofStringValues))
            {
                _wopiValidatorLogger.LogError($"Request header for '{WopiRequestHeaders.PROOF}' was not found.");
                return false;
            }

            if (!request.Headers.TryGetValue(WopiRequestHeaders.TIME_STAMP, out StringValues wopiTimeStampStringValues))
            {
                _wopiValidatorLogger.LogError($"Request header for '{WopiRequestHeaders.TIME_STAMP}' was not found.");
                return false;
            }

            if (!request.Headers.TryGetValue(WopiRequestHeaders.PROOF_OLD, out StringValues wopiOldProofStringValues))
            {
                _wopiValidatorLogger.LogError($"Request header for '{WopiRequestHeaders.TIME_STAMP}' was not found.");
                return false;
            }

            WopiProof wopiProofCache = await _wopidDiscoveryCache.GetOrAddWopiProofFromCache(_wopiConfigSettings.DiscoveryUrl, _wopiConfigSettings.ProofKeyInfoCacheExpirationInMinutes);

            if (!request.Query.TryGetValue(ConfigurationConstants.ACCESS_TOKEN, out StringValues accessTokenStringValue))
            {
                _wopiValidatorLogger.LogError(string.Format(Logging.MISSING_QUERY_STRING, ConfigurationConstants.ACCESS_TOKEN));
                return false;
            }

            WopiProofKeyInfoHelper wopiProofKeyInfoHelper =
                 new WopiProofKeyInfoHelper(wopiProofCache.CurrentWopiProofKeyInfo, wopiProofCache.OldWopiProofKeyInfo);

            string accessToken = accessTokenStringValue.FirstOrDefault();
            string wopiCurrentProof = wopiCurrentProofStringValues.FirstOrDefault();
            string wopiOldProof = wopiOldProofStringValues.FirstOrDefault();
            long timeStamp = long.Parse(wopiTimeStampStringValues.FirstOrDefault());

            string requestUrl = request.GetEncodedUrl();

            //TODO: Hard coded URI as of now for testing purpose.  This should be obtained from Az App Configuration later. (WI:18170)
            requestUrl = $"{_wopiConfigSettings.RequestUrlOrigin}{requestUrl.Substring(requestUrl.LastIndexOf("/v1/"))}";

            WopiProofKeyValidationInput wopiProofKeyValidationInput =
                new WopiProofKeyValidationInput(accessToken, timeStamp, requestUrl, wopiCurrentProof, wopiOldProof);

            //TODO: Remove all these console logs in the next PR.(WI:18011)
            Console.WriteLine("=============================================================================================================================================================================");
            Console.WriteLine("=============================================================================================================================================================================");
            Console.WriteLine("=============================================================================================================================================================================");
            Console.WriteLine($"Current CspBlob     : {wopiProofCache.CurrentWopiProofKeyInfo.CspBlob}");
            Console.WriteLine($"Current Exponent    : {wopiProofCache.CurrentWopiProofKeyInfo.Exponent}");
            Console.WriteLine($"Current Modulus     : {wopiProofCache.CurrentWopiProofKeyInfo.Modulus}");
            Console.WriteLine("");
            Console.WriteLine($"Old CspBlob         : {wopiProofCache.OldWopiProofKeyInfo.CspBlob}");
            Console.WriteLine($"Old Exponent        : {wopiProofCache.OldWopiProofKeyInfo.Exponent}");
            Console.WriteLine($"Old Modulus         : {wopiProofCache.OldWopiProofKeyInfo.Modulus}");
            Console.WriteLine("");
            Console.WriteLine($"{WopiRequestHeaders.PROOF} : {wopiCurrentProof}");
            Console.WriteLine("");
            Console.WriteLine($"{WopiRequestHeaders.TIME_STAMP} : {timeStamp}");
            Console.WriteLine("");
            Console.WriteLine($"{WopiRequestHeaders.PROOF_OLD} : {wopiOldProof}");
            Console.WriteLine("");
            Console.WriteLine($"accessToken        : {accessToken}");
            Console.WriteLine("");
            Console.WriteLine($"requestUrl         : {requestUrl}");
            Console.WriteLine("");
            Console.WriteLine("=============================================================================================================================================================================");
            Console.WriteLine("=============================================================================================================================================================================");
            Console.WriteLine("=============================================================================================================================================================================");

            bool isWopiProofKeyValidationEnabled = await _featureManager.IsEnabledAsync(nameof(WopiFeatureFlags.WopiProofKeyValidation));

            return isWopiProofKeyValidationEnabled ? await wopiProofKeyInfoHelper.ValidateWopiProof(wopiProofKeyValidationInput) : isWopiProofKeyValidationEnabled;
        }
        #endregion
    }
}
