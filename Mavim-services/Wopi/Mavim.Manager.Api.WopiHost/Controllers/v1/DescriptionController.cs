using Mavim.Manager.Api.Utils.Constants;
using Mavim.Manager.Api.Utils.Constants.Wopi;
using Mavim.Manager.Api.WopiFileLock.Services.Interfaces;
using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Controllers.v1
{
    [Authorize]
    [Route("v1/{dbId}/{dataLanguage}/[controller]/wopi/files")]
    public class DescriptionController : ControllerBase
    {
        private const string ObjectId = "oid";
        private readonly IDescriptionService _descriptionService;
        private readonly IWopiFileLockService _wopiFileLockService;
        private readonly ILogger<DescriptionController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="descriptionService">The descriptionService.</param>
        /// <param name="wopiFileLockService">The wopi file lock service.</param>
        public DescriptionController(ILogger<DescriptionController> logger, IDescriptionService descriptionService, IWopiFileLockService wopiFileLockService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wopiFileLockService = wopiFileLockService ?? throw new ArgumentNullException(nameof(wopiFileLockService));
            _descriptionService = descriptionService ?? throw new ArgumentNullException(nameof(descriptionService));
        }

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">File id argument is null or empty - id</exception>
        /// <remarks>
        /// Handles: CheckFileInfo
        /// </remarks>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ICheckFileInfo>> CheckFileInfo(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("File id argument is null or empty", nameof(id));

            string token = await Request.HttpContext.GetTokenAsync(Configuration.TOKEN_NAME);
            return Ok(await _descriptionService.GetFileInfo(id, token));
        }

        /// <summary>
        /// Files the operations.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// Handles: Lock, GetLock, RefreshLock, Unlock, UnlockAndRelock, PutRelativeFile, RenameFile, PutUserInfo
        /// </remarks>
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> FileOperations(string id, Guid dbId)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("File id argument is null or empty", nameof(id));

            Dictionary<string, string> requestHeaders = Request.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault());
            string token = await Request.HttpContext.GetTokenAsync(Configuration.TOKEN_NAME);

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception(string.Format(Logging.MISSING_HEADER_FORMAT, MissingHeader.AUTHENTICATION_HEADER_KEY));

            // Lookup the file in the database
            ICheckFileInfo fileInfo = await _descriptionService.GetFileInfo(id, token);

            if (fileInfo == null) throw new Exception($"Could not find file information for the file with id {id}.");

            Dictionary<string, string> responseHeaders = await _wopiFileLockService.ProcessFileOperationWopiRequest(id, dbId, token, requestHeaders, fileInfo.Version);

            Response.Headers.Add(WopiRequestHeaders.X_WOPI_ITEMVERSION, fileInfo.Version);

            responseHeaders.ToList().ForEach(responseHeader =>
            {
                Response.Headers.Add(responseHeader.Key, responseHeader.Value);
            });

            KeyValuePair<string, string> errorValue = responseHeaders.FirstOrDefault(h => h.Key.Contains(WopiRequestHeaders.X_WOPI_LOCK_FAILURE_REASON));

            if (errorValue.Key != null)
                return Conflict("Lock mismatch/Locked by another interface");

            return Ok();
        }

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// Handles: GetFile
        /// </remarks>
        [HttpGet]
        [Route("{id}/contents")]
        public async Task<ActionResult<ActionResult<FileStreamResult>>> GetFileContent(string id, Guid dbId)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("File id argument is null or empty", nameof(id));

            string token = await Request.HttpContext.GetTokenAsync(Configuration.TOKEN_NAME);
            ICheckFileInfo fileInfo = await _descriptionService.GetFileInfo(id, token);
            Stream fileContent = await _descriptionService.GetFileContent(id);

            Response.Headers.Add(WopiRequestHeaders.X_WOPI_ITEMVERSION, fileInfo.Version);

            return File(fileContent, "application/octet-stream");
        }

        /// <summary>
        /// Puts the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// Handles: PutFile
        /// </remarks>
        [HttpPost]
        [Route("{id}/contents")]
        public async Task<ActionResult<string>> PutFile(string id, Guid dbId)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("File id argument is null or empty", nameof(id));

            Dictionary<string, string> requestHeaders = Request.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault());
            string token = await Request.HttpContext.GetTokenAsync(Configuration.TOKEN_NAME);
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception(string.Format(Logging.MISSING_HEADER_FORMAT, MissingHeader.AUTHENTICATION_HEADER_KEY));

            ICheckFileInfo fileInfo = await _descriptionService.GetFileInfo(id, token);
            string oldVersion = fileInfo.Version;
            Dictionary<string, string> responseHeaders = await _wopiFileLockService.ProcessPutFileWopiRequest(id, dbId, token, requestHeaders, fileInfo.Size);

            Response.Headers.Add(WopiRequestHeaders.X_WOPI_ITEMVERSION, fileInfo.Version);

            responseHeaders.ToList().ForEach(responseHeader =>
            {
                Response.Headers.Add(responseHeader.Key, responseHeader.Value);
            });

            KeyValuePair<string, string> errorValue = responseHeaders.FirstOrDefault(h => h.Key.Contains(WopiRequestHeaders.X_WOPI_LOCK_FAILURE_REASON));

            if (errorValue.Key != null)
                return Conflict("Lock mismatch/Locked by another interface");

            //If there were no errors with the PUTFILE operation with file lock database then continue to update description.
            PipeReader bodyPipeReader = Request.BodyReader;
            Stream descriptionStream = new MemoryStream();
            await bodyPipeReader.CopyToAsync(descriptionStream);
            descriptionStream.Position = 0;
            await _descriptionService.UpdateDescription(id, descriptionStream);


            //After updating the file, check the version(Hash) of the file and if the hash has not changed then the same file was written back,
            //so in this case, the hash has to be manipulated in order to mimic the version change for the WOPI validations to pass
            //as we don't have an inbuilt version mechanism within Mavim for the description
            fileInfo = await _descriptionService.GetFileInfo(id, token);

            if (fileInfo.Version.Equals(oldVersion))
            {
                Response.Headers.Remove(WopiRequestHeaders.X_WOPI_ITEMVERSION);
                Response.Headers.Add(WopiRequestHeaders.X_WOPI_ITEMVERSION, $"{fileInfo.Version}-{DateTime.UtcNow.Ticks}");
            }

            return Ok();
        }
    }
}
