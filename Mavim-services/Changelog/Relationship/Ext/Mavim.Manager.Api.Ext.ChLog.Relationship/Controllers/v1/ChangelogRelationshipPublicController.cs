using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.Ext.ChLog.Relationship.Models;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Ext.ChLog.Relationship.Controllers.v1
{
    [Authorize]
    [FeatureGate(ChangelogFeature.Changelog)]
    [Route("/v1/{databaseId}/{dataLanguage}/changelog/relationships/")]
    public class ChangelogRelationshipPublicController : ControllerBase
    {
        #region Private Members
        private readonly IChangelogRelationshipPublicService _relationService;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogRelationshipPublicController"/> class.
        /// </summary>
        /// <param name="relationService">The title service.</param>
        /// <param name="logger">The logger.</param>
        /// titleService
        /// or
        /// logger
        /// </exception>
        public ChangelogRelationshipPublicController(IChangelogRelationshipPublicService relationService, ILogger<ChangelogRelationshipPublicController> logger)
        {
            _relationService = relationService ?? throw new ArgumentNullException(nameof(relationService));
        }

        /// <summary>
        /// Retrieves the relations in the changelog by dbId and topicId
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns>
        /// List of changelog relations
        /// </returns>
        [HttpGet]
        [Route("topic/{topicId}")]
        public async Task<ActionResult<IEnumerable<IChangelogRelationship>>> GetRelations(GetByTopicRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _relationService.GetRelations(routeParams.DatabaseId, routeParams.TopicId));
        }

        /// <summary>
        /// Gets the pending relation changes of a topic.
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("topic/{topicId}/pending")]
        public async Task<ActionResult<IEnumerable<IChangelogRelationship>>> GetPendingRelations(GetByTopicRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _relationService.GetPendingRelations(routeParams.DatabaseId, routeParams.TopicId));
        }

        /// <summary>
        /// Gets all pending relation changes.
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("pending")]
        public async Task<ActionResult<IEnumerable<IChangelogRelationship>>> GetAllPendingRelations(BaseRouteParam routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _relationService.GetAllPendingRelations(routeParams.DatabaseId));
        }

        /// <summary>
        /// Gets the status of last relation changes by topic.
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("topic/{topicId}/status")]
        public async Task<ActionResult<ChangeStatus>> GetRelationStatus(GetByTopicRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _relationService.GetRelationStatus(routeParams.DatabaseId, routeParams.TopicId));
        }

        /// <summary>
        /// Approves the pending relation change.
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{changelogId}/approve")]
        public async Task<ActionResult<IChangelogRelationship>> ApproveRelation(PatchRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _relationService.ApproveRelation(routeParams.DatabaseId, routeParams.ChangelogId));
        }

        /// <summary>
        /// Rejects the title.
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{changelogId}/reject")]
        public async Task<ActionResult<IChangelogRelationship>> RejectRelation(PatchRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _relationService.RejectRelation(routeParams.DatabaseId, routeParams.ChangelogId));
        }
    }
}
