using Mavim.Libraries.Features.Enums;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Interface;
using Mavim.Manager.Api.ChangelogTitle.Services.v1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogTitle.Controllers.v1
{
    [Authorize]
    [Route("/v1/{dbId}/{dataLanguage}/changelog/titles/")]
    [FeatureGate(ChangelogFeature.Changelog)]
    public class TitleController : ControllerBase
    {
        #region Private Members
        private readonly ITitleService _titleService;
        private readonly ILogger<TitleController> _logger;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleController"/> class.
        /// </summary>
        /// <param name="titleService">The title service.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">
        /// titleService
        /// or
        /// logger
        /// </exception>
        public TitleController(ITitleService titleService, ILogger<TitleController> logger)
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
        public async Task<ActionResult<IEnumerable<ITitle>>> GetTitles(Guid dbId, string dcvId)
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
        public async Task<ActionResult<ITitle>> GetPendingTitle(Guid dbId, string dcvId)
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
        public async Task<ActionResult<IEnumerable<ITitle>>> GetAllPendingTitles(Guid dbId)
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
        /// Saves the title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">title</exception>
        [HttpPost]
        [Route("topic/{dcvId}")]
        public async Task<ActionResult> SaveTitle([FromBody] Title title, Guid dbId)
        {
            if (title == null)
                throw new BadRequestException(nameof(title));

            _logger.LogTrace($"Save title from: {title.FromTitle} to: {title.ToTitle} for topic dcv: {title.TopicDcv}.");
            await _titleService.SaveTitle(Map(title, dbId));
            return NoContent();
        }

        /// <summary>
        /// Approves the title.
        /// </summary>
        /// <param name="dbId">The dbId.</param>
        /// <param name="dcvId">The dcvId.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("topic/{dcvId}/approve")]
        public async Task<ActionResult<ITitle>> ApproveTitle(Guid dbId, string dcvId)
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
        public async Task<ActionResult<ITitle>> RejectTitle(Guid dbId, string dcvId)
        {
            _logger.LogTrace($"Reject title from dbId: {dbId} for dcvId: {dcvId}");

            return Ok(await _titleService.RejectTitle(dbId, dcvId));
        }

        private static ISaveTitle Map(Title title, Guid dbId)
        {
            return new SaveTitle
            {
                TopicDcv = title.TopicDcv,
                FromTitleValue = title.FromTitle,
                ToTitleValue = title.ToTitle,
                DatabaseId = dbId
            };
        }
    }
}
