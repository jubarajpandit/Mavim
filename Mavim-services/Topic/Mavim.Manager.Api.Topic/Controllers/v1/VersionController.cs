using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Mavim.Manager.Api.Topic.Controllers.v1
{
    /// <summary>
    /// Version controller
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}")]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionController" /> class
        /// </summary>
        /// <param name="logger">The logger.</param>
        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get API version
        /// </summary>
        /// <remarks>Retrieve the API version number.</remarks>
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("version")]
        public ActionResult<string> Version()
        {
            _logger.LogDebug("Retrieving API version number.");

            try
            {
                Version version = typeof(Startup).Assembly.GetName().Version;
                return Ok(version.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
