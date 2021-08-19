using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Controllers.v1
{
    [Route("/v1/[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="settings">The settings.</param>
        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>Retrieve the API version number.</summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Index()
        {
            _logger.LogDebug("Retrieving API version number.");

            try
            {
                Version version = typeof(Startup).Assembly.GetName().Version;
                return Ok(version.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred trying to retrieve the API version.");
                throw;
            }
        }
    }
}
