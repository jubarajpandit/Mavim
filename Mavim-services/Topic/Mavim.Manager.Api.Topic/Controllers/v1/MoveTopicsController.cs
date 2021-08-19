using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.Topic.Commands.Interfaces;
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
    /// Topic controller for moving topics in the hierarchical tree
    /// </summary>
    [Authorize]
    [ApiController]
    [FeatureGate(TopicFeature.MoveTopicFeature)]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class MoveTopicsController : ControllerBase
    {
        /// <summary>
        /// Move a topic to the top of the branch
        /// </summary>
        /// <remarks>Move a specific topic to the top of the branch.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="command"></param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}/movetotop")]
        public async Task<ActionResult> MoveTopicToTop(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IMoveToTopCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            await command.Execute(topicId);
            return Ok();
        }

        /// <summary>
        /// Move a topic to the bottom of the branch
        /// </summary>
        /// <remarks>Move a specific topic to the bottom of the branch.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}/movetobottom")]
        public async Task<ActionResult> MoveTopicToBottom(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IMoveToBottomCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            await command.Execute(topicId);
            return Ok();
        }

        /// <summary>
        /// Move a topic up in the branch
        /// </summary>
        /// <remarks>Move a specific topic one place up in the branch.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}/moveup")]
        public async Task<ActionResult> MoveTopicUp(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IMoveUpCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            await command.Execute(topicId);
            return Ok();
        }

        /// <summary>
        /// Move a topic down in the branch
        /// </summary>
        /// <remarks>Move a specific topic one place down in the branch.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}/movedown")]
        public async Task<ActionResult> MoveTopicDown(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IMoveDownCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            await command.Execute(topicId);
            return Ok();
        }

        /// <summary>
        /// Move a topic up in the branch
        /// </summary>
        /// <remarks>Move a specific topic one place up in the branch.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}/movelevelup")]
        public async Task<ActionResult> MoveTopicLevelUp(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IMoveLevelUpCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            await command.Execute(topicId);
            return Ok();
        }

        /// <summary>
        /// Move a topic down in the branch
        /// </summary>
        /// <remarks>Move a specific topic one place down in the branch.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}/moveleveldown")]
        public async Task<ActionResult> MoveTopicLevelDown(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IMoveLevelDownCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            await command.Execute(topicId);
            return Ok();
        }

        /// <summary>
        /// Change the parent of a paticular topic
        /// </summary>
        /// <remarks>Change the parent of a paticular topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="topic"></param>
        /// <param name="command"></param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}/parentid")]
        public async Task<ActionResult> ChangeParentTopic(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromBody] SaveTopicParent topic,
            [FromServices] IChangeParentCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await command.Execute(topicId, topic.ParentId);
            return NoContent();
        }
    }
}

