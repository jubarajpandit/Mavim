using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IChangelog = Mavim.Libraries.Changelog.Interfaces;
using IChangelogEnum = Mavim.Libraries.Changelog.Enums;

namespace Mavim.Manager.Api.ChangelogTitle.Public.Services.v1
{
    public class ChangelogTitlePublicService : IChangelogTitlePublicService
    {
        #region Private Members
        private readonly IChangelog.IChangelogTitleClient _changelogTitleClient;
        private readonly ILogger<ChangelogTitlePublicService> _logger;
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
        public ChangelogTitlePublicService(IChangelog.IChangelogTitleClient changelogTitleClient, ILogger<ChangelogTitlePublicService> logger)
        {
            _changelogTitleClient = changelogTitleClient ?? throw new ArgumentNullException(nameof(changelogTitleClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Gets the titles.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogTitle>> GetTitles(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid, dcvid);

            IEnumerable<IChangelog.IChangelogTitle> result = await _changelogTitleClient.GetTitles(dbid, dcvid);
            return result.Select(Map);
        }

        /// <summary>
        /// Gets the pending title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<IChangelogTitle> GetPendingTitle(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid, dcvid);

            IChangelog.IChangelogTitle result = await _changelogTitleClient.GetPendingTitle(dbid, dcvid);
            return Map(result);
        }

        /// <summary>
        /// Gets all pending titles.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogTitle>> GetAllPendingTitles(Guid dbid)
        {
            CheckInputParameters(dbid);

            IEnumerable<IChangelog.IChangelogTitle> result = await _changelogTitleClient.GetAllPendingTitles(dbid);
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

            IChangelogEnum.ChangeStatus result = await _changelogTitleClient.GetTitleStatus(dbid, dcvid);

            return Map(result);
        }

        /// <summary>
        /// Approves the title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<IChangelogTitle> ApproveTitle(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid, dcvid);

            IChangelog.IChangelogTitle result = await _changelogTitleClient.ApproveTitle(dbid, dcvid);
            return Map(result);
        }

        /// <summary>
        /// Rejects the title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<IChangelogTitle> RejectTitle(Guid dbid, string dcvid)
        {
            CheckInputParameters(dbid, dcvid);

            IChangelog.IChangelogTitle result = await _changelogTitleClient.RejectTitle(dbid, dcvid);
            return Map(result);
        }

        #region Private Methods
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

        private static IChangelogTitle Map(IChangelog.IChangelogTitle title) =>
            title == null ? null :
            new Models.ChangelogTitle
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

        private static ChangeStatus Map(IChangelogEnum.ChangeStatus titleStatus)
        {
            return titleStatus switch
            {
                IChangelogEnum.ChangeStatus.Pending => ChangeStatus.Pending,
                IChangelogEnum.ChangeStatus.Approved => ChangeStatus.Approved,
                IChangelogEnum.ChangeStatus.Rejected => ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(titleStatus), titleStatus, null)
            };
        }
        #endregion
    }
}
