using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.Topic.Queries.Interfaces;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Mavim.Manager.Topic.Queries;

namespace Mavim.Manager.Api.Topic.Controllers.v1
{
    /// <summary>
    /// Topics controller for retrieving topic metadata
    /// </summary>
    [Authorize]
    [ApiController]
    [FeatureGate(TopicFeature.CreateTopicFeature)]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class TopicsMetaController : ControllerBase
    {
        /// <summary>
        /// Get types for a topic
        /// </summary>
        /// <remarks>Get a collection of types for an specific topic supplied by the topic id.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The DCV identifier.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IDictionary<string, ElementTypeInfo>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("topic/{topicId}/types")]
        public async Task<ActionResult<IDictionary<string, ElementTypeInfo>>> GetTopicTypes(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromServices] IQueryRetrieveTopicTypesCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Ok(await command.Execute(topicId));
        }

        /// <summary>
        /// Get availible icons for a topic type
        /// </summary>
        /// <remarks>Get a collection of icons for a specific topic type.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicType">The Topic type.</param>
        /// <param name="command"></param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("types/{topicType}/icons")]
        public async Task<ActionResult<IDictionary<string, string>>> GetTopicIcons(
            Guid dbId,
            DataLanguages dataLanguage,
            string topicType,
            [FromServices] IQueryRetrieveTopicIconsCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Ok(await command.Execute(topicType));
        }

        /// <summary>
        /// Get custom icon by custom icon id
        /// </summary>
        /// <remarks>Get custom icon for an specific topic supplied by the custom icon id.</remarks>
        /// <param name="dbId"></param>
        /// <param name="dataLanguage"></param>
        /// <param name="customIconId"></param>
        /// <param name="mediator"></param>
        /// <returns></returns>

        [Produces("image/png")]      
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]        
        [HttpGet]
        [Route("topic/{customIconId}/customicon")]
        public async Task<FileContentResult> GetTopicCustomIcon(
            Guid dbId,
            DataLanguages dataLanguage,
            string customIconId,
            [FromServices] IMediator mediator)
        {            
            var image = await mediator.Send(new GetTopicCustomIconByCustomIconIdQuery.Query(customIconId));            
            var result = File(image, "image/png", customIconId + ".png");
            Response.Headers.Add("Cache-Control", "2592000");
            return result;
        }
    }
}

