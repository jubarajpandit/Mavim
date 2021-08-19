using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Interface;
using Mavim.Manager.Api.ChangelogTitle.Services.v1.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepo = Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1;
using Repo = Mavim.Manager.Api.ChangelogTitle.Repository.v1.Model;

namespace Mavim.Manager.Api.ChangelogTitle.Services.v1
{
    public class TitleService : ITitleService
    {
        #region Private Members
        private readonly IRepo.ITitleRepository _titleRepository;
        private readonly ILogger<TitleService> _logger;
        private readonly IJwtSecurityToken _token;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleService"/> class.
        /// </summary>
        /// <param name="titleRepository">The title repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="token">The token.</param>
        /// <exception cref="ArgumentNullException">
        /// titleRepository
        /// or
        /// logger
        /// or
        /// token
        /// </exception>
        public TitleService(IRepo.ITitleRepository titleRepository, ILogger<TitleService> logger, IJwtSecurityToken token)
        {
            _titleRepository = titleRepository ?? throw new ArgumentNullException(nameof(titleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// Gets the titles.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ITitle>> GetTitles(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid, dcvid);

            IEnumerable<IRepo.Interface.ITitle> result = await _titleRepository.GetTitles(_token.TenantId, dbid, dcvid);
            return result.Select(Map);
        }

        /// <summary>
        /// Gets the pending title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<ITitle> GetPendingTitle(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid, dcvid);

            IRepo.Interface.ITitle result = await _titleRepository.GetPendingTitle(_token.TenantId, dbid, dcvid);
            return Map(result);
        }

        /// <summary>
        /// Gets all pending titles.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ITitle>> GetAllPendingTitles(Guid dbid)
        {
            CheckInputParameters(dbid);

            IEnumerable<IRepo.Interface.ITitle> result = await _titleRepository.GetAllPendingTitles(_token.TenantId, dbid);
            return result.Select(Map);
        }

        /// <summary>
        /// Gets the title status.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<ChangeStatus> GetTitleStatus(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid, dcvid);

            return (ChangeStatus)await _titleRepository.GetTitleStatus(_token.TenantId, dbid, dcvid);
        }

        /// <summary>
        /// Saves the title.
        /// </summary>
        /// <param name="title">The title.</param>
        public async Task SaveTitle(ISaveTitle title)
        {
            CheckInputParameters(title);

            title.TenantId = _token.TenantId;
            title.InitiatorUserEmail = _token.Email?.ToString();
            title.TimestampChanged = DateTime.Now;
            title.Status = ChangeStatus.Pending;

            await _titleRepository.SaveTitle(Map(title));
        }

        /// <summary>
        /// Approves the title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<ITitle> ApproveTitle(Guid dbid, string dcvid)
        {
            return await UpdateTitleStatus(dbid, dcvid, IRepo.Enum.ChangeStatus.Approved);
        }

        /// <summary>
        /// Rejects the title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<ITitle> RejectTitle(Guid dbid, string dcvid)
        {
            return await UpdateTitleStatus(dbid, dcvid, IRepo.Enum.ChangeStatus.Rejected);
        }

        private async Task<ITitle> UpdateTitleStatus(Guid dbid, string dcvid, IRepo.Enum.ChangeStatus status)
        {
            CheckInputParameters(dbid, dcvid);
            IRepo.Interface.ITitle title = await _titleRepository.GetPendingTitle(_token.TenantId, dbid, dcvid);
            if (title == null)
                throw new RequestNotFoundException("cannot update non-pending titles");

            title.Status = status;
            title.TimestampApproved = DateTime.Now;
            title.ReviewerUserEmail = _token.Email?.ToString();

            IRepo.Interface.ITitle result = await _titleRepository.UpdateTitleState(title);

            return Map(result);
        }

        private void CheckInputParameters(Guid dbid)
        {
            if (dbid != Guid.Empty) return;
            _logger.LogError("dbid argument empty");
            throw new ArgumentNullException(nameof(dbid));
        }

        private void CheckInputParameters(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid);

            if (!string.IsNullOrEmpty(dcvid)) return;
            _logger.LogError("dcvid is null or empty");
            throw new ArgumentNullException(nameof(dcvid));
        }

        private void CheckInputParameters(ISaveTitle title)
        {
            if (title != null) return;
            _logger.LogError("title is null");
            throw new ArgumentNullException(nameof(title));
        }

        private static ITitle Map(IRepo.Interface.ITitle title) =>
            title == null ? null :
            new Title
            {
                ChangelogId = title.ChangelogId,
                InitiatorUserEmail = title.InitiatorUserEmail,
                ReviewerUserEmail = title.ReviewerUserEmail,
                TimestampChanged = title.TimestampChanged,
                TimestampApproved = title.TimestampApproved,
                TopicDcv = title.TopicDcv,
                Status = Map(title.Status),
                FromTitleValue = title.FromTitleValue,
                ToTitleValue = title.ToTitleValue
            };

        private static ChangeStatus Map(IRepo.Enum.ChangeStatus titleStatus)
        {
            return titleStatus switch
            {
                IRepo.Enum.ChangeStatus.Pending => ChangeStatus.Pending,
                IRepo.Enum.ChangeStatus.Approved => ChangeStatus.Approved,
                IRepo.Enum.ChangeStatus.Rejected => ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(titleStatus), titleStatus, null)
            };
        }

        private static IRepo.Interface.ITitle Map(ISaveTitle title) =>
            title == null ? null :
            new Repo.Title
            {
                TenantId = title.TenantId,
                DatabaseId = title.DatabaseId,
                InitiatorUserEmail = title.InitiatorUserEmail,
                TimestampChanged = title.TimestampChanged,
                TopicDcv = title.TopicDcv,
                Status = Map(title.Status),
                FromTitleValue = title.FromTitleValue,
                ToTitleValue = title.ToTitleValue
            };

        private static IRepo.Enum.ChangeStatus Map(ChangeStatus titleStatus)
        {
            return titleStatus switch
            {
                ChangeStatus.Pending => IRepo.Enum.ChangeStatus.Pending,
                ChangeStatus.Approved => IRepo.Enum.ChangeStatus.Approved,
                ChangeStatus.Rejected => IRepo.Enum.ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(titleStatus), titleStatus, null)
            };
        }
    }
}
