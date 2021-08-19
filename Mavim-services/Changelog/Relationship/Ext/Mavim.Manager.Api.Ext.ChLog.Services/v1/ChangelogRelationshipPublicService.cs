using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IChangelog = Mavim.Libraries.Changelog.Interfaces;
using IChangelogEnum = Mavim.Libraries.Changelog.Enums;

namespace Mavim.Manager.Api.Ext.ChLog.Services.v1
{
    public class ChangelogRelationshipPublicService : IChangelogRelationshipPublicService
    {
        #region Private Members
        private readonly IChangelog.IChangelogRelationshipClient _changelogRelationshipClient;
        private readonly ILogger<ChangelogRelationshipPublicService> _logger;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogRelationshipPublicService"/> class.
        /// </summary>
        /// <param name="changelogRelationshipClient">The relationship client.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// changelogRelationshipClient
        /// or
        /// logger
        /// </exception>
        public ChangelogRelationshipPublicService(IChangelog.IChangelogRelationshipClient changelogRelationshipClient, ILogger<ChangelogRelationshipPublicService> logger)
        {
            _changelogRelationshipClient = changelogRelationshipClient ?? throw new ArgumentNullException(nameof(changelogRelationshipClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Gets the relationships.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogRelationship>> GetRelations(Guid dbId, string topicId)
        {
            CheckInputParameters(dbId, topicId);

            IEnumerable<IChangelog.IChangelogRelationship> result = await _changelogRelationshipClient.GetRelations(dbId, topicId);
            return result.Select(Map);
        }

        /// <summary>
        /// Gets the pending relations.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogRelationship>> GetPendingRelations(Guid dbId, string topicId)
        {
            CheckInputParameters(dbId, topicId);

            IEnumerable<IChangelog.IChangelogRelationship> result = await _changelogRelationshipClient.GetPendingRelations(dbId, topicId);
            return result.Select(Map);
        }

        /// <summary>
        /// Gets all pending relations.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IChangelogRelationship>> GetAllPendingRelations(Guid dbId)
        {
            CheckInputParameters(dbId);

            IEnumerable<IChangelog.IChangelogRelationship> result = await _changelogRelationshipClient.GetAllPendingRelations(dbId);
            return result.Select(Map);
        }

        /// <summary>
        /// Gets the relations status by topicId.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <returns></returns>
        public async Task<ChangeStatus> GetRelationStatus(Guid dbId, string topicId)
        {
            CheckInputParameters(dbId, topicId);

            IChangelogEnum.ChangeStatus result = await _changelogRelationshipClient.GetRelationStatus(dbId, topicId);

            return Map(result);
        }

        /// <summary>
        /// Approves the pending relation change.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="changelogId">The changelog identifier.</param>
        /// <returns></returns>
        public async Task<IChangelogRelationship> ApproveRelation(Guid dbId, Guid changelogId)
        {
            CheckInputParameters(dbId, changelogId);

            IChangelog.IChangelogRelationship result = await _changelogRelationshipClient.ApproveRelation(dbId, changelogId);
            return Map(result);
        }

        /// <summary>
        /// Rejects the pending relation change.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="changelogId">The changelog identifier.</param>
        /// <returns></returns>
        public async Task<IChangelogRelationship> RejectRelation(Guid dbId, Guid changelogId)
        {
            CheckInputParameters(dbId, changelogId);

            IChangelog.IChangelogRelationship result = await _changelogRelationshipClient.RejectRelation(dbId, changelogId);
            return Map(result);
        }

        #region Private Methods
        private void CheckInputParameters(Guid dbId)
        {
            if (dbId != Guid.Empty) return;
            _logger.LogError("dbid argument empty");
            throw new ArgumentNullException(nameof(dbId));
        }

        private void CheckInputParameters(Guid dbId, string topicId)
        {
            CheckInputParameters(dbId);

            if (!string.IsNullOrEmpty(topicId)) return;
            _logger.LogError("dcvid is null or empty");
            throw new ArgumentNullException(nameof(topicId));
        }

        private void CheckInputParameters(Guid dbId, Guid changelogId)
        {
            CheckInputParameters(dbId);

            if (changelogId != Guid.Empty) return;
            _logger.LogError("changelogId is empty");
            throw new ArgumentNullException(nameof(changelogId));
        }

        private void CheckInputParameters(Guid dbId, string topicId, string relationId)
        {
            CheckInputParameters(dbId, topicId);

            if (!string.IsNullOrEmpty(relationId)) return;
            _logger.LogError("dcvid is null or empty");
            throw new ArgumentNullException(nameof(relationId));
        }

        private static IChangelogRelationship Map(IChangelog.IChangelogRelationship relationship) =>
            relationship == null ? null :
            new Models.ChangelogRelation
            {
                ChangelogId = relationship.ChangelogId,
                InitiatorUserEmail = relationship.InitiatorUserEmail,
                ReviewerUserEmail = relationship.ReviewerUserEmail,
                TimestampChanged = relationship.TimestampChanged,
                TimestampApproved = relationship.TimestampApproved,
                TopicDcv = relationship.TopicDcv,
                Status = Map(relationship.Status),
                RelationDcv = relationship.RelationDcv,
                FromCategory = relationship.FromCategory,
                FromTopicDcv = relationship.FromTopicDcv,
                ToCategory = relationship.ToCategory,
                ToTopicDcv = relationship.ToTopicDcv
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
