using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Api.Topic.Features;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
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
    /// Topics controller for deleting topics
    /// </summary>
    [Authorize]
    [ApiController]
    [FeatureGate(TopicFeatureFlags.DeleteTopic)]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class DeleteTopicsController : ControllerBase
    {
        /// <summary>
        /// Delete a topic
        /// </summary>
        /// <remarks>The specific topic requested by the topic id is deleted.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ITopic), (int)HttpStatusCode.OK)]
        [HttpDelete]
        [Route("topic/{topicId}")]
        public async Task<ActionResult<ITopic>> DeleteTopic(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IDeleteTopicCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await command.Execute(topicId);
            return Ok();
        }
    }
}

