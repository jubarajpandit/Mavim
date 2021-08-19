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
    /// Relationships controller for managing relations
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/")]
    public class RelationshipsController : ControllerBase
    {
        private IRelationshipService Service { get; set; }

        /// <summary>
        /// RelationshipsController Constructor
        /// </summary>
        /// <param name="service"></param>
        public RelationshipsController(IRelationshipService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Get the Relations of the topic
        /// </summary>
        /// <remarks>Get the Relations of the topic supplied by topic DCV ID.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<IRelationship>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("topic/{topicId}/relations")]
        public async Task<ActionResult<IEnumerable<IRelationship>>> GetTopicRelations(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId)
        {
            return Ok(await Service.GetRelationships(topicId));
        }

        /// <summary>
        /// Create relation
        /// </summary>
        /// <remarks>Creates a relation between two topics.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="relation">The relation.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IRelationship), (int)HttpStatusCode.OK)]
        [HttpPost]
        [Route("relation")]
        public async Task<ActionResult<IRelationship>> SaveRelation(
            Guid dbId,
            DataLanguages dataLanguage,
            [FromBody] SaveRelationship relation)
        {
            return Ok(await Service.SaveRelationship(relation));
        }

        /// <summary>
        /// Delete relation from topic
        /// </summary>
        /// <remarks>Deletes the relation by topic id and relationId.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The Topic identifier.</param>
        /// <param name="relationId">The relation identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpDelete]
        [Route("topic/{topicId}/relation/{relationId}")]
        public async Task<ActionResult> DeleteRelation(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Relation identifier is not valid")] string relationId)
        {
            await Service.DeleteRelationship(topicId, relationId);
            return NoContent();
        }
    }
}
