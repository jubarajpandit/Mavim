using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.v1.Model;
using Mavim.Manager.ChLog.Relationship.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel = Mavim.Manager.ChLog.Relationship.DbModel;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Repository.v1
{
    public class RelationRepository : IRelationRepository
    {
        #region Private Members
        protected readonly RelationDbContext _dbContext;
        protected readonly ILogger<RelationRepository> _logger;
        #endregion

        public RelationRepository(RelationDbContext relationDbContext, ILogger<RelationRepository> logger)
        {
            _dbContext = relationDbContext ?? throw new ArgumentNullException(nameof(relationDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IRelation> GetRelationById(Guid tenantId, Guid changelogId)
        {
            _logger.LogTrace($"GetRelationByID TenantID: {tenantId} - ChangelogID: {changelogId}");

            var relation = await _dbContext.Relations.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.TenantId == tenantId &&
                    x.ChangelogId == changelogId
                );

            return Map(relation);
        }

        public async Task<IEnumerable<IRelation>> GetRelationsByTopic(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, string topicId)
        {
            _logger.LogTrace($"GetRelationsByTopic TenantID: {tenantId} - DbId: {dbId} - DataLanguageType {dataLanguage} - TopicID: {topicId}");
            if (string.IsNullOrEmpty(topicId)) throw new ArgumentNullException(nameof(topicId));

            DbModel.Enums.DataLanguageType dbLanguage = Map(dataLanguage);
            IEnumerable<DbModel.Relation> result = await _dbContext.Relations.AsNoTracking()
                .Where(x =>
                x.TenantId == tenantId &&
                x.DatabaseId == dbId &&
                x.DataLanguage == dbLanguage &&
                x.TopicId == topicId
               ).ToListAsync();

            return result.Select(Map);
        }

        public async Task<IEnumerable<IRelation>> GetAllRelationsByStatus(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, ChangeStatus status)
        {
            _logger.LogTrace($"GetAllRelationsByStatus TenantID: {tenantId} - DbId: {dbId} - DataLanguageType {dataLanguage} - Status: {status}");
            DbModel.Enums.DataLanguageType dbLanguage = Map(dataLanguage);
            DbModel.Enums.ChangeStatus dbStatus = Map(status);
            IEnumerable<DbModel.Relation> result = await _dbContext.Relations.AsNoTracking()
                .Where(x =>
                x.TenantId == tenantId &&
                x.DatabaseId == dbId &&
                x.DataLanguage == dbLanguage &&
                x.Status == dbStatus
               ).ToListAsync();

            return result.Select(Map);
        }

        public async Task<IEnumerable<IRelation>> GetAllTopicRelationsByStatus(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, string topicId, ChangeStatus status)
        {
            _logger.LogTrace($"GetAllTopicRelationsByStatus TenantID: {tenantId} - DbId: {dbId} - DataLanguageType {dataLanguage} - TopicId: {topicId} - Status: {status}");
            if (string.IsNullOrEmpty(topicId)) throw new ArgumentNullException(nameof(topicId));

            DbModel.Enums.DataLanguageType dbLanguage = Map(dataLanguage);
            DbModel.Enums.ChangeStatus dbStatus = Map(status);
            IEnumerable<DbModel.Relation> result = await _dbContext.Relations.AsNoTracking()
                .Where(x =>
                x.TenantId == tenantId &&
                x.DatabaseId == dbId &&
                x.TopicId == topicId &&
                x.DataLanguage == dbLanguage &&
                x.Status == dbStatus
               ).ToListAsync();

            return result.Select(Map);
        }

        public async Task<IRelation> SaveRelation(IRelation relation)
        {
            _logger.LogTrace($"SaveRelation {relation}");
            if (relation == null) throw new ArgumentNullException(nameof(relation));

            DbModel.Relation dbRelation = Map(relation);

            await _dbContext.Relations.AddAsync(dbRelation);
            await _dbContext.SaveChangesAsync();

            return Map(dbRelation);
        }

        public async Task UpdateRelationStatus(Guid tenantId, Guid changelogId, string reviewerUserEmail, DateTime approvedTime, ChangeStatus status)
        {
            _logger.LogTrace($"UpdateRelationStatus TenantID: {tenantId} - ChangelogId: {changelogId} - ApproverEmail {reviewerUserEmail} - ApprovedTime: {approvedTime} - Status: {status}");
            if (string.IsNullOrEmpty(reviewerUserEmail)) throw new ArgumentNullException("ApproverEmail argument empty");

            DbModel.Relation relation = await _dbContext.Relations.FirstOrDefaultAsync(x =>
                                                    x.TenantId == tenantId &&
                                                    x.ChangelogId == changelogId
                                                );

            if (relation == null)
                throw new RequestNotFoundException($"Changelog id {changelogId} not found");

            relation.Status = Map(status);
            relation.TimestampApproved = approvedTime;
            relation.ReviewerUserEmail = reviewerUserEmail;

            _dbContext.Relations.Update(relation);
            await _dbContext.SaveChangesAsync();
        }

        private IRelation Map(DbModel.Relation relation) =>
            relation == null ? null : new Relation
            {
                ChangelogId = relation.ChangelogId,
                TenantId = relation.TenantId,
                DatabaseId = relation.DatabaseId,
                DataLanguage = Map(relation.DataLanguage),
                InitiatorUserEmail = relation.InitiatorUserEmail,
                TimestampChanged = relation.TimestampChanged,
                ReviewerUserEmail = relation.ReviewerUserEmail,
                TimestampApproved = relation.TimestampApproved,
                TopicId = relation.TopicId,
                RelationId = relation.RelationId,
                Action = Map(relation.Action),
                Status = Map(relation.Status),
                OldCategory = relation.OldCategory,
                OldTopicId = relation.OldTopicId,
                Category = relation.Category,
                ToTopicId = relation.ToTopicId
            };

        private DbModel.Relation Map(IRelation relation) =>
            relation == null ? null : new DbModel.Relation
            {
                ChangelogId = relation.ChangelogId,
                TenantId = relation.TenantId,
                DatabaseId = relation.DatabaseId,
                DataLanguage = Map(relation.DataLanguage),
                InitiatorUserEmail = relation.InitiatorUserEmail,
                TimestampChanged = relation.TimestampChanged,
                ReviewerUserEmail = relation.ReviewerUserEmail,
                TimestampApproved = relation.TimestampApproved,
                TopicId = relation.TopicId,
                RelationId = relation.RelationId,
                Action = Map(relation.Action),
                Status = Map(relation.Status),
                OldCategory = relation.OldCategory,
                OldTopicId = relation.OldTopicId,
                Category = relation.Category,
                ToTopicId = relation.ToTopicId
            };

        private Interfaces.v1.Enum.Action Map(DbModel.Enums.Action action) => action switch
        {
            DbModel.Enums.Action.Create => Interfaces.v1.Enum.Action.Create,
            DbModel.Enums.Action.Delete => Interfaces.v1.Enum.Action.Delete,
            DbModel.Enums.Action.Edit => Interfaces.v1.Enum.Action.Edit,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action,
                    "Action not defined.")
        };

        private DbModel.Enums.Action Map(Interfaces.v1.Enum.Action action) => action switch
        {
            Interfaces.v1.Enum.Action.Create => DbModel.Enums.Action.Create,
            Interfaces.v1.Enum.Action.Delete => DbModel.Enums.Action.Delete,
            Interfaces.v1.Enum.Action.Edit => DbModel.Enums.Action.Edit,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action,
                    "Action not defined.")
        };

        private static ChangeStatus Map(DbModel.Enums.ChangeStatus changeStatus) => changeStatus switch
        {
            DbModel.Enums.ChangeStatus.Pending => ChangeStatus.Pending,
            DbModel.Enums.ChangeStatus.Approved => ChangeStatus.Approved,
            DbModel.Enums.ChangeStatus.Rejected => ChangeStatus.Rejected,
            _ => throw new ArgumentOutOfRangeException(nameof(changeStatus), changeStatus,
                "ChangeStatus not defined.")
        };

        private DbModel.Enums.ChangeStatus Map(ChangeStatus changeStatus) => changeStatus switch
        {
            ChangeStatus.Pending => DbModel.Enums.ChangeStatus.Pending,
            ChangeStatus.Approved => DbModel.Enums.ChangeStatus.Approved,
            ChangeStatus.Rejected => DbModel.Enums.ChangeStatus.Rejected,
            _ => throw new ArgumentOutOfRangeException(nameof(changeStatus), changeStatus,
                "ChangeStatus not defined.")
        };

        private DbModel.Enums.DataLanguageType Map(DataLanguageType dataLanguage) => dataLanguage switch
        {
            DataLanguageType.Dutch => DbModel.Enums.DataLanguageType.Dutch,
            DataLanguageType.English => DbModel.Enums.DataLanguageType.English,
            _ => throw new ArgumentException($"unsupported DataLanguage: {dataLanguage.ToString()}")
        };
        private DataLanguageType Map(DbModel.Enums.DataLanguageType dataLanguage) => dataLanguage switch
        {
            DbModel.Enums.DataLanguageType.Dutch => DataLanguageType.Dutch,
            DbModel.Enums.DataLanguageType.English => DataLanguageType.English,
            _ => throw new ArgumentException($"unsupported DataLanguage: {dataLanguage.ToString()}")
        };
    }
}
