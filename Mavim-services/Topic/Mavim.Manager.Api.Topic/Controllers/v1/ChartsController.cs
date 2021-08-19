using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Controllers.v1
{
    /// <summary>
    /// Charts API controller
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class ChartController : ControllerBase
    {
        private IChartService _service { get; set; }

        /// <summary>
        /// RelationshipsController Constructor
        /// </summary>
        /// <param name="service"></param>
        public ChartController(IChartService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Get the Charts of the topic
        /// </summary>
        /// <remarks>From a specific topic requested with the topic id, a collection of charts is returned.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<IChart>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("topic/{topicId}/charts")]
        public async Task<ActionResult<IEnumerable<IChart>>> GetTopicCharts(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId)
        {
            return Ok(await _service.GetTopicCharts(topicId));
        }
    }

}
