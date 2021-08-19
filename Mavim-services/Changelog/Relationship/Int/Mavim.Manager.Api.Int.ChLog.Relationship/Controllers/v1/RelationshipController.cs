using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.Int.ChLog.Relationship.Controllers.v1.Models;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface;
using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IRelation = Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface.IRelation;
using Service = Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1.Model;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Controllers.v1
{
    [Authorize]
    [FeatureGate(ChangelogFeature.Changelog)]
    [Route("/v1/{dbId}/{dataLanguage}/changelog/relationships/")]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationService _relationService;

        public RelationshipController(IRelationService relationService, ILogger<RelationshipController> logger)
        {
            _relationService = relationService ?? throw new ArgumentNullException(nameof(relationService));
        }

        /// <summary>
        /// Get all pening relations
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        [Route("pending")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IRelation>>> GetAllPendingRelations(Guid dbId)
        {
            return Ok(await _relationService.GetAllPendingRelations(dbId));
        }

        /// <summary>
        /// Get relations by topic
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("topics/{topicId}")]
        public async Task<ActionResult<IEnumerable<IRelation>>> GetRelationsByTopic(Guid dbId, [RegularExpression(RegexUtils.Dcv)] string topicId)
        {
            return Ok(await _relationService.GetRelationsByTopic(dbId, topicId));
        }

        /// <summary>
        /// Get the status by topic
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("topics/{topicId}/status")]
        public async Task<ActionResult<ChangeStatus>> GetStatusByTopic(Guid dbId, [RegularExpression(RegexUtils.Dcv)] string topicId)
        {
            return Ok(await _relationService.GetRelationStatus(dbId, topicId));
        }

        /// <summary>
        /// Get pending relations by topic
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("topics/{topicId}/pending")]
        public async Task<ActionResult<IEnumerable<IRelation>>> GetPendingRelationsByTopic(Guid dbId, [RegularExpression(RegexUtils.Dcv)] string topicId)
        {
            return Ok(await _relationService.GetPendingRelationsByTopic(dbId, topicId));
        }

        /// <summary>
        /// Add relation
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="saveRelation"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<IRelation>> CreateRelation(Guid dbId, [FromBody] SaveCreateRelation saveRelation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _relationService.SaveRelation(dbId, Map(saveRelation)));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<IRelation>> DeleteRelation(Guid dbId, [FromBody] SaveRelation saveRelation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _relationService.SaveRelation(dbId, Map(saveRelation)));
        }

        [HttpPost]
        [Route("edit")]
        public async Task<ActionResult<IRelation>> EditRelation(Guid dbId, [FromBody] SaveEditRelation saveRelation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _relationService.SaveRelation(dbId, Map(saveRelation)));
        }

        /// <summary>
        /// Update state to approved
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="changelogId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{changelogId}/approve")]
        public async Task<ActionResult> ApproveRelation(Guid dbId, Guid changelogId)
        {
            await _relationService.UpdateRelationStatus(dbId, changelogId, ChangeStatus.Approved);
            return Ok();
        }

        /// <summary>
        /// Update state to rejected
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="changelogId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{changelogId}/reject")]
        public async Task<ActionResult> RejectRelation(Guid dbId, Guid changelogId)
        {
            await _relationService.UpdateRelationStatus(dbId, changelogId, ChangeStatus.Rejected);
            return Ok();
        }

        private ISaveRelation Map(SaveCreateRelation saveRelation) => new Service.SaveRelation()
        {
            TopicId = saveRelation.TopicId,
            RelationId = saveRelation.RelationId,
            Action = Services.Interfaces.v1.Enum.Action.Create,
            Category = saveRelation.Category,
            ToTopicId = saveRelation.ToTopicId,
        };

        private ISaveRelation Map(SaveRelation saveRelation) => new Service.SaveRelation()
        {
            TopicId = saveRelation.TopicId,
            RelationId = saveRelation.RelationId,
            Action = Services.Interfaces.v1.Enum.Action.Delete,
        };

        private ISaveRelation Map(SaveEditRelation saveRelation) => new Service.SaveRelation()
        {
            TopicId = saveRelation.TopicId,
            RelationId = saveRelation.RelationId,
            Action = Services.Interfaces.v1.Enum.Action.Edit,
            Category = saveRelation.Category,
            ToTopicId = saveRelation.ToTopicId,
            OldCategory = saveRelation.OldCategory,
            OldTopicId = saveRelation.OldToTopicId,
        };
    }
}
