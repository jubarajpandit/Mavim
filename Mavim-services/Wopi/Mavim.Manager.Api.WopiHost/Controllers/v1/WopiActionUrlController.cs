using Mavim.Libraries.Middlewares.WopiValidator.Helpers;
using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Controllers.v1
{
    [Route("v1/[controller]")]
    public class WopiActionUrlController : Controller
    {
        private readonly IWopiActionUrlMetadataService _wopiActionUrlMetadataService;
        private readonly ILogger<WopiActionUrlController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="wopiActionUrlMetadataService">The descriptionService.</param>
        /// <exception cref="ArgumentNullException">
        /// logger
        /// or
        /// wopiActionUrlMetadataService
        /// </exception>
        public WopiActionUrlController(ILogger<WopiActionUrlController> logger, IWopiActionUrlMetadataService wopiActionUrlMetadataService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wopiActionUrlMetadataService = wopiActionUrlMetadataService ?? throw new ArgumentNullException(nameof(wopiActionUrlMetadataService));
        }

        /// <summary>
        /// Gets the file metadata.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">File id argument is null or empty - id</exception>
        [HttpGet]
        public async Task<ActionResult<IWopiActionUrlMetaData>> GetActionUrls()
        {
            return Ok(await _wopiActionUrlMetadataService.GetWopiSourceMetadata());
        }
    }
}