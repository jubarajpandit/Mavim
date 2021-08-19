using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.v1.Models;
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
    /// Topics controller for updating topics
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class TopicsController : ControllerBase
    {
        private ITopicService Service { get; set; }
        /// <summary>
        /// TopicsController Constructor
        /// </summary>
        /// <param name="service">The service</param>
        public TopicsController(ITopicService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Get the root topic
        /// </summary>
        /// <remarks>Get the root topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ITopic), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("root")]
        public async Task<ActionResult<ITopic>> GetTopicRoot(
            Guid dbId,
            DataLanguages dataLanguage)
        {
            return Ok(await Service.GetRootTopic(dbId));
        }

        /// <summary>
        /// Get the path of the supplied topic id to the root
        /// </summary>
        /// <remarks>Get the all topics in the path to the root topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ITopicPath), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("path/{topicId}")]
        public async Task<ActionResult<ITopicPath>> GetPathToRoot(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId)
        {
            return Ok(await Service.GetPathToRoot(topicId));
        }

        /// <summary>
        /// Get Topic
        /// </summary>
        /// <remarks>Get a specific topic by the DCV indentifier.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ITopic), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("topic/{topicId}")]
        public async Task<ActionResult<ITopic>> GetTopic(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId)
        {
            return Ok(await Service.GetTopic(dbId, topicId));
        }

        /// <summary>
        /// Updates the name of the topic
        /// </summary>
        /// <remarks>Updates the name of a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="topic">The topic</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ITopic), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("topic/{topicId}")]
        public async Task<ActionResult<ITopic>> UpdateTopic(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromBody] SaveTopic topic)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await Service.UpdateTopic(topicId, topic));
        }

        /// <summary>
        /// Get the children of a topic
        /// </summary>
        /// <remarks>Get the children topics of the topic</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ITopic>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("topic/{topicId}/children")]
        public async Task<ActionResult<IEnumerable<ITopic>>> GetTopicChildren(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId)
        {
            return Ok(await Service.GetChildren(topicId));
        }

        /// <summary>
        /// Get the sibling topics of the topic
        /// </summary>
        /// <remarks>Get the siblings topics of the topic supplied by the topic id.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The DCV identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ITopic>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("topic/{topicId}/siblings")]
        public async Task<ActionResult<IEnumerable<ITopic>>> GetTopicSiblings(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId)
        {
            return Ok(await Service.GetSiblings(topicId));
        }
    }
}
