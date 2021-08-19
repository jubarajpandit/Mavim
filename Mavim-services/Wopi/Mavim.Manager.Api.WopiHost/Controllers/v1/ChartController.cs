using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.WopiFileLock.Services.Interfaces;
using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Controllers.v1
{
    [Authorize]
    [Route("v1/{dbId}/{dataLanguage}/[controller]/wopi/files")]
    public class ChartController : ControllerBase
    {
        private readonly IChartService _chartService;
        private readonly IWopiFileLockService _wopiFileLockService;
        private readonly ILogger<DescriptionController> _logger;
        private readonly IFeatureManager _featureManager;

        private const string EmbeddingPageOrigin = "EmbeddingPageOrigin";
        private const string EmbeddingPageSessionInfo = "EmbeddingPageSessionInfo";

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="chartService">The descriptionService.</param>
        /// <param name="catalogClient">The catalog client.</param>
        /// <param name="wopiFileLockService">The wopi file lock service.</param>
        public ChartController(ILogger<DescriptionController> logger, IChartService chartService, IWopiFileLockService wopiFileLockService, IFeatureManager featureManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wopiFileLockService = wopiFileLockService ?? throw new ArgumentNullException(nameof(wopiFileLockService));
            _chartService = chartService ?? throw new ArgumentNullException(nameof(chartService));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="access_token">The access token.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// Handles: CheckFileInfo
        /// </remarks>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ICheckFileInfo>> CheckFileInfo(string id, string access_token, Guid dbId)
        {
            string decodedString = string.Empty;
            string embeddedPageOrigin = string.Empty;
            string embeddingPageSessionInfo = string.Empty;
            StringValues sessionContext;

            if (await _featureManager.IsEnabledAsync(nameof(ChartNavigationFeature.ChartNavigation)))
            {
                if (Request.Headers.TryGetValue("X-WOPI-SessionContext", out sessionContext) &&
                !string.IsNullOrWhiteSpace(sessionContext.ToString()))
                {
                    byte[] bytes = Convert.FromBase64String(sessionContext);
                    decodedString = Encoding.UTF8.GetString(bytes);

                    if (!string.IsNullOrWhiteSpace(decodedString))
                    {
                        List<string> splitValues = decodedString.Split('&').ToList();

                        Dictionary<string, string> KeyValuePairs = splitValues.Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);

                        KeyValuePairs.TryGetValue(EmbeddingPageOrigin, out embeddedPageOrigin);
                        KeyValuePairs.TryGetValue(EmbeddingPageSessionInfo, out embeddingPageSessionInfo);
                    }
                }
            }

            return Ok(await _chartService.GetFileInfo(id, access_token, embeddedPageOrigin, embeddingPageSessionInfo));
        }

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="access_token">The access token.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// Handles: GetFile
        /// </remarks>
        [HttpGet]
        [Route("{id}/contents")]
        public async Task<ActionResult<ActionResult<FileStreamResult>>> GetFileContent(string id, string access_token, Guid dbId)
        {
            Stream fileContent = await _chartService.GetFileContent(id, access_token);
            return File(fileContent, "application/octet-stream");
        }
    }
}
