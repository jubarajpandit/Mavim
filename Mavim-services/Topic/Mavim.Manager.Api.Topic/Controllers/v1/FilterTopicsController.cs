using Mavim.Manager.Api.Topic.Features;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Topic.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Controllers.v1
{
    /// <summary>
    /// Topics controller for filtering topics
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class FilterTopicsController : ControllerBase
    {
        /// <summary>
        /// Get Topics by code
        /// </summary>
        /// <remarks>Get specific topics filtered by code.</remarks>
        /// <param name="mediator"></param>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicCode">The Topic code.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ITopic>), (int)HttpStatusCode.OK)]
        [FeatureGate(TopicFeatureFlags.TopicByCode)]
        [HttpGet]
        [Route("filters/topics/code/{topicCode}")]
        public async Task<ActionResult<ITopic>> GetTopicsByCode(
            [FromServices] IMediator mediator,
            Guid dbId,
            DataLanguages dataLanguage,
            string topicCode)
        {
            return Ok(await mediator.Send(new GetTopicByCodeQuery.Query(topicCode)));
        }
    }
}
