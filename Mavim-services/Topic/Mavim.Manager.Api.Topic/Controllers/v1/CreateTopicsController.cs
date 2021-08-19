using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Controllers.v1
{
    /// <summary>
    /// Topics controller for creating topics
    /// </summary>
    [Authorize]
    [ApiController]
    [FeatureGate(TopicFeature.CreateTopicFeature)]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class CreateTopicsController : ControllerBase
    {
        /// <summary>
        /// Create a new topic
        /// </summary>
        /// <remarks>A new topic is created as a sibling after the referenced topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="createTopic">The topic.</param>
        /// <param name="command"></param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ITopic), (int)HttpStatusCode.OK)]
        [HttpPost]
        [Route("topic/{topicId}")]
        public async Task<ActionResult<ITopic>> CreateTopicAfter(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromBody] CreateTopic createTopic,
            [FromServices] ICreateTopicAfterCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await command.Execute(topicId, createTopic.Name, createTopic.Type, createTopic.Icon));
        }

        /// <summary>
        /// Create new child topic
        /// </summary>
        /// <remarks>A new topic is created as a child of the referenced topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="createTopic">The topic.</param>
        /// <param name="command"></param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ITopic), (int)HttpStatusCode.OK)]
        [HttpPost]
        [Route("topic/{topicId}/children")]
        public async Task<ActionResult<ITopic>> CreateChildTopic(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromBody] CreateTopic createTopic,
            [FromServices] ICreateChildTopicCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await command.Execute(topicId, createTopic.Name, createTopic.Type, createTopic.Icon));
        }
    }
}

