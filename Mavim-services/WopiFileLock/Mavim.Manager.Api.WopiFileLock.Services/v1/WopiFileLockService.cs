using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Utils.Constants.Wopi;
using Mavim.Manager.Api.WopiFileLock.Repository.Interfaces;
using Mavim.Manager.Api.WopiFileLock.Repository.Interfaces.v1;
using Mavim.Manager.Api.WopiFileLock.Services.Interfaces;
using Mavim.Manager.WopiFileLock.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiFileLock.Services.v1
{
    public class WopiFileLockService : IWopiFileLockService
    {
        private const string ObjectId = "oid"; // the object id in the jwt token is unique per AD user.
        private readonly IWopiFileLockRepository _repository;
        private readonly ILogger<WopiFileLockService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiFileLockService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">repository
        /// or
        /// logger</exception>
        public WopiFileLockService(IWopiFileLockRepository repository, ILogger<WopiFileLockService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the file lock status.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="userObjectId">The user object identifier.</param>
        /// <returns></returns>
        public async Task<IWopiFileLockState> GetFileLock(Guid dbId, string id, string userObjectId)
        {
            _logger.LogDebug($"Get file lock status for file with id: {id} and db id: {dbId}.");
            return await _repository.GetFileLock(dbId, id);
        }

        /// <summary>
        /// Processes the put file wopi request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="userObjectId">The user object identifier.</param>
        /// <param name="requestHeaders">The request headers.</param>
        /// <param name="currentFileSize">Size of the current file.</param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> ProcessPutFileWopiRequest(string id, Guid dbId, string userToken, Dictionary<string, string> requestHeaders, long currentFileSize)
        {
            Dictionary<string, string> responseHeaderValues = new Dictionary<string, string>();
            string userObjectId = GetUserObjectIdFromJwtToken(userToken);

            IWopiFileLockState fileLockState = await _repository.GetFileLock(dbId, id);

            requestHeaders.TryGetValue(WopiRequestHeaders.X_WOPI_LOCK, out string xWopiLockValue);

            // Ensure the file has a valid lock
            if (string.IsNullOrEmpty(fileLockState?.Value))
            {

                responseHeaderValues.Add(WopiResponseHeaders.LOCK, string.Empty);

                if (currentFileSize > 0)
                {
                    // Ensure the file has a valid lock
                    responseHeaderValues.Add(WopiResponseHeaders.LOCK_FAILURE_REASON, "File isn't locked");
                    return responseHeaderValues;
                }

                return responseHeaderValues;
            }

            if (fileLockState.Expires < DateTime.Now)
            {
                // File lock expired, so clear it out
                await _repository.UnlockFile(dbId, id, fileLockState.Value);
                responseHeaderValues.Add(WopiResponseHeaders.LOCK, string.Empty);
                responseHeaderValues.Add(WopiResponseHeaders.LOCK_FAILURE_REASON, "File isn't locked");
                return responseHeaderValues;
            }

            if (xWopiLockValue != fileLockState.Value)
            {
                // File lock mismatch...pass Lock in mismatch response
                responseHeaderValues.Add(WopiResponseHeaders.LOCK, fileLockState.Value);
                responseHeaderValues.Add(WopiResponseHeaders.LOCK_FAILURE_REASON, "Lock mismatch");
                return responseHeaderValues;
            }

            responseHeaderValues.Add(WopiResponseHeaders.LOCK, fileLockState.Value);
            return responseHeaderValues;
        }

        /// <summary>
        /// Processes the file operation wopi request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="userObjectId">The user object identifier.</param>
        /// <param name="requestHeaders">The request headers.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Request does not contain valid headers</exception>
        /// <exception cref="RequestNotImplementedException">Wopi PutRelativeFile functionality is not implemented.</exception>
        public async Task<Dictionary<string, string>> ProcessFileOperationWopiRequest(string id, Guid dbId, string userToken, Dictionary<string, string> requestHeaders, string version)
        {
            if (!requestHeaders.TryGetValue(WopiRequestHeaders.X_WOPI_OVERRIDE, out string xWopiOverride))
                throw new Exception("Request does not contain valid headers");

            string xWopiOldLockValue = string.Empty;

            requestHeaders.TryGetValue(WopiRequestHeaders.X_WOPI_LOCK, out string xWopiLockValue);

            if (requestHeaders.TryGetValue(WopiRequestHeaders.X_WOPI_OLD_LOCK, out string xWopiOldLockOverride) &&
                !string.IsNullOrWhiteSpace(xWopiOldLockOverride))
            {
                xWopiOverride = WopiXOverrideConstants.X_OVERRIDE_UNLOCK_RELOCK;
                requestHeaders.TryGetValue(WopiRequestHeaders.X_WOPI_OLD_LOCK, out xWopiOldLockValue);
            }

            string userObjectId = GetUserObjectIdFromJwtToken(userToken);

            switch (xWopiOverride)
            {
                case WopiXOverrideConstants.X_OVERRIDE_GET_LOCK:
                    return await GetFileLockStatus(id, dbId, xWopiOldLockValue);
                case WopiXOverrideConstants.X_OVERRIDE_LOCK:
                    return await AddFileLock(id, dbId, userObjectId, version, xWopiLockValue);
                case WopiXOverrideConstants.X_OVERRIDE_UNLOCK:
                    return await UnlockFile(id, dbId, xWopiLockValue);
                case WopiXOverrideConstants.X_OVERRIDE_UNLOCK_RELOCK:
                    return await UnlockAndRelock(id, dbId, userObjectId, xWopiLockValue, xWopiOldLockValue);
                case WopiXOverrideConstants.X_OVERRIDE_REFRESH_LOCK:
                    return await RefreshLockFile(id, dbId, userObjectId, xWopiLockValue, xWopiOldLockValue);
                case WopiXOverrideConstants.X_OVERRIDE_PUT_RELATIVE:
                    throw new RequestNotImplementedException("Wopi PutRelativeFile functionality is not implemented.");
                default:
                    _logger.LogWarning($"Undefined switch case: {xWopiOverride}. Returning empty list header values.");
                    return new Dictionary<string, string>();
            }
        }

        private async Task<Dictionary<string, string>> RefreshLockFile(string id, Guid dbId, string userObjectId, string xWopiLockValue, string xWopiOldLockValue)
        {
            IWopiFileLockResult result = await _repository.RefreshLockFile(dbId, id, xWopiLockValue, xWopiOldLockValue);

            return SetResponseHeadersBasedOnFileLockResult(result);
        }

        private async Task<Dictionary<string, string>> UnlockAndRelock(string id, Guid dbId, string userObjectId, string xWopiLockValue, string xWopiOldLockValue)
        {
            IWopiFileLockResult result = await _repository.UnlockAndRelock(dbId, id, xWopiLockValue, xWopiOldLockValue);

            return SetResponseHeadersBasedOnFileLockResult(result);
        }

        private async Task<Dictionary<string, string>> UnlockFile(string id, Guid dbId, string xWopiLockValue)
        {
            IWopiFileLockResult result = await _repository.UnlockFile(dbId, id, xWopiLockValue);

            return SetResponseHeadersBasedOnFileLockResult(result);
        }

        private async Task<Dictionary<string, string>> AddFileLock(string id, Guid dbId, string userObjectId, string version, string xWopiLockValue)
        {
            IWopiFileLockResult result = await _repository.AddFileLock(dbId, id, userObjectId, xWopiLockValue, version);

            return SetResponseHeadersBasedOnFileLockResult(result);
        }

        private Dictionary<string, string> SetResponseHeadersBasedOnFileLockResult(IWopiFileLockResult result)
        {
            Dictionary<string, string> responseHeaderValues = new Dictionary<string, string>();

            switch (result.FileLockStatus)
            {

                case WopiFileLockStatus.FileLockMismatch:
                    responseHeaderValues.Add(WopiRequestHeaders.X_WOPI_LOCK, result.LockValue);
                    responseHeaderValues.Add(WopiRequestHeaders.X_WOPI_LOCK_FAILURE_REASON, "Lock mismatch");
                    break;
                case WopiFileLockStatus.FileNotLocked:
                    responseHeaderValues.Add(WopiRequestHeaders.X_WOPI_LOCK, "");
                    break;
                case WopiFileLockStatus.FileUnLockSuccessful:
                    responseHeaderValues.Add(WopiRequestHeaders.X_WOPI_LOCK, string.Empty);
                    break;

                case WopiFileLockStatus.FileAlreadyLocked:
                    responseHeaderValues.Add(WopiRequestHeaders.X_WOPI_LOCK, result.LockValue);
                    break;
                case WopiFileLockStatus.FileLockSuccessful:
                case WopiFileLockStatus.FileUnLockNReLockSuccessful:
                case WopiFileLockStatus.FileLockExtendedSuccessful:
                    break;
                default:
                    _logger.LogWarning($"Missing switch case: {result.FileLockStatus}.");
                    break;
            }

            return responseHeaderValues;
        }

        private async Task<Dictionary<string, string>> GetFileLockStatus(string fileId, Guid dbId, string xWopiOldLockValue)
        {
            if (string.IsNullOrEmpty(fileId)) throw new ArgumentException("fileid is null or empty", nameof(fileId));

            Dictionary<string, string> responseHeaderValues = new Dictionary<string, string>();
            IWopiFileLockResult result = await _repository.GetFileLockStatus(dbId, fileId, xWopiOldLockValue);
            responseHeaderValues.Add(WopiRequestHeaders.X_WOPI_LOCK, result.LockValue ?? string.Empty);
            return responseHeaderValues;
        }

        private string GetUserObjectIdFromJwtToken(string token)
        {
            string oid = string.Empty;

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            if (jwtHandler.CanReadToken(token))
            {
                JwtSecurityToken sToken = jwtHandler.ReadJwtToken(token);
                if (sToken.Payload.ContainsKey(ObjectId))
                {
                    oid = sToken.Payload[ObjectId].ToString();
                }
            }

            // the token should always be there and have an oid
            if (string.IsNullOrWhiteSpace(oid)) throw new Exception("No 'oid' found in the token.");
            return oid;
        }
    }
}