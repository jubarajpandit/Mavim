using Mavim.Libraries.Features.Enums;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogTitle.Public.Controllers.v1
{
    [Authorize]
    [Route("/v1/{dbId}/{dataLanguage}/changelog/titles/")]
    [FeatureGate(ChangelogFeature.Changelog)]
    public class ChangelogTitlePublicController : ControllerBase
    {
        #region Private Members
        private readonly IChangelogTitlePublicService _titleService;
        private readonly ILogger<ChangelogTitlePublicController> _logger;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogTitlePublicController"/> class.
        /// </summary>
        /// <param name="titleService">The title service.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// titleService
        /// or
        /// logger
        /// </exception>
        public ChangelogTitlePublicController(IChangelogTitlePublicService titleService, ILogger<ChangelogTitlePublicController> logger)
        {
            _titleService = titleService ?? throw new ArgumentNullException(nameof(titleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves the titles in the changelog by dbId and dcvId
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="dcvId">The dcvId.</param>
        /// <returns>
        /// List of changelog titles
        /// </returns>
        [HttpGet]
        [Route("topic/{dcvId}")]
        public async Task<ActionResult<IEnumerable<IChangelogTitle>>> GetTitles(Guid dbId, string dcvId)
        {
            _logger.LogTrace($"Get changelog titles from dbId: {dbId} for dcvId: {dcvId}");

            return Ok(await _titleService.GetTitles(dbId, dcvId));
        }

        /// <summary>
        /// Gets the pending title.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="dcvId">The dcvId.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("topic/{dcvId}/pending")]
        public async Task<ActionResult<IChangelogTitle>> GetPendingTitle(Guid dbId, string dcvId)
        {
            _logger.LogTrace($"Get pending title from dbId: {dbId} for dcvId: {dcvId}");

            return Ok(await _titleService.GetPendingTitle(dbId, dcvId));
        }

        /// <summary>
        /// Gets all pending titles.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("pending")]
        public async Task<ActionResult<IEnumerable<IChangelogTitle>>> GetAllPendingTitles(Guid dbId)
        {
            _logger.LogTrace($"Get all pending titles from dbId: {dbId}");

            return Ok(await _titleService.GetAllPendingTitles(dbId));
        }

        /// <summary>
        /// Gets the status of a title.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="dcvId">The dcvId.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("topic/{dcvId}/status")]
        public async Task<ActionResult<ChangeStatus>> GetTitleStatus(Guid dbId, string dcvId)
        {
            _logger.LogTrace($"Get title status from dbId: {dbId} for dcvId: {dcvId}");

            return Ok(await _titleService.GetTitleStatus(dbId, dcvId));
        }

        /// <summary>
        /// Approves the title.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="dcvId">The dcvId.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("topic/{dcvId}/approve")]
        public async Task<ActionResult<IChangelogTitle>> ApproveTitle(Guid dbId, string dcvId)
        {
            _logger.LogTrace($"Approve title from dbId: {dbId} for dcvId: {dcvId}");

            return Ok(await _titleService.ApproveTitle(dbId, dcvId));
        }

        /// <summary>
        /// Rejects the title.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="dcvId">The dcvId.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("topic/{dcvId}/reject")]
        public async Task<ActionResult<IChangelogTitle>> RejectTitle(Guid dbId, string dcvId)
        {
            _logger.LogTrace($"Reject title from dbId: {dbId} for dcvId: {dcvId}");

            return Ok(await _titleService.RejectTitle(dbId, dcvId));
        }
    }
}
