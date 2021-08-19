using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1.Model;
using Mavim.Manager.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IMiddleware = Mavim.Libraries.Middlewares.Language;
using IRepo = Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1;
using Repo = Mavim.Manager.Api.Int.ChLog.Relationship.Repository.v1.Model;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1
{
    public class RelationService : IRelationService
    {
        #region Private Members
        private readonly IRepo.IRelationRepository _relationRepository;
        private readonly IJwtSecurityToken _token;
        private readonly IRepo.Enum.DataLanguageType _dataLanguage;
        #endregion

        public RelationService(IRepo.IRelationRepository changelogRepository, IJwtSecurityToken token, IMiddleware.Interfaces.IDataLanguage language)
        {
            _relationRepository = changelogRepository ?? throw new ArgumentNullException(nameof(changelogRepository));
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _dataLanguage = language != null ? Map(language.Type) : throw new ArgumentNullException(nameof(language));
        }

        public async Task<IEnumerable<IRelation>> GetRelationsByTopic(Guid dbId, string topicId)
        {
            CheckDatabaseAndTopic(dbId, topicId);

            IEnumerable<IRepo.Interface.IRelation> result = await _relationRepository.GetRelationsByTopic(_token.TenantId, dbId, Map(_dataLanguage), topicId);
            return result.Select(Map);
        }

        public async Task<IEnumerable<IRelation>> GetPendingRelationsByTopic(Guid dbId, string topicId)
        {
            CheckDatabaseAndTopic(dbId, topicId);

            IEnumerable<IRepo.Interface.IRelation> result = await _relationRepository.GetAllTopicRelationsByStatus(_token.TenantId, dbId, Map(_dataLanguage), topicId, IRepo.Enum.ChangeStatus.Pending);
            return result.Select(Map);
        }

        public async Task<IEnumerable<IRelation>> GetAllPendingRelations(Guid dbId)
        {
            CheckDatabaseInput(dbId);

            IEnumerable<IRepo.Interface.IRelation> result = await _relationRepository.GetAllRelationsByStatus(_token.TenantId, dbId, Map(_dataLanguage), IRepo.Enum.ChangeStatus.Pending);
            return result.Select(Map);
        }

        public async Task<Interfaces.v1.Enum.ChangeStatus> GetRelationStatus(Guid dbId, string topicId)
        {
            CheckDatabaseAndTopic(dbId, topicId);

            IEnumerable<IRepo.Interface.IRelation> relations = await _relationRepository.GetRelationsByTopic(_token.TenantId, dbId, Map(_dataLanguage), topicId);

            IRepo.Enum.ChangeStatus? status = relations.OrderByDescending(x => x.TimestampApproved)
                            .ThenByDescending(x => x.TimestampChanged)
                            .FirstOrDefault()
                            ?.Status;

            return Map(status ?? IRepo.Enum.ChangeStatus.Approved);
        }

        public async Task<IRelation> SaveRelation(Guid dbId, ISaveRelation saveRelation)
        {
            CheckSaveRelation(dbId, saveRelation);

            IRepo.Interface.IRelation relation = EnrichSaveRelation(dbId, saveRelation);

            return Map(await _relationRepository.SaveRelation(relation));
        }

        public async Task UpdateRelationStatus(Guid dbId, Guid changelogId, Interfaces.v1.Enum.ChangeStatus status)
        {
            CheckUpdateRelationStatus(dbId, changelogId);

            IRepo.Interface.IRelation relation = await _relationRepository.GetRelationById(_token.TenantId, changelogId);

            if (relation == null
                || relation.DataLanguage != Map(_dataLanguage)
                || relation.DatabaseId != dbId)
                throw new RequestNotFoundException($"Changelog id {changelogId} not found");

            if (relation.Status != IRepo.Enum.ChangeStatus.Pending)
                throw new RequestNotFoundException("Cannot update non-pending relations");

            await _relationRepository.UpdateRelationStatus(
                _token.TenantId,
                changelogId,
                _token.Email,
                DateTime.Now,
                Map(status)
               );
        }

        private void CheckDatabaseAndTopic(Guid dbId, string topicId)
        {
            CheckDatabaseInput(dbId);
            CheckTopicIdInput(topicId);
        }

        private void CheckSaveRelation(Guid dbId, ISaveRelation saveRelation)
        {
            CheckDatabaseInput(dbId);
            CheckSaveRelationInput(saveRelation);
        }

        private void CheckUpdateRelationStatus(Guid dbId, Guid changelogId)
        {
            CheckDatabaseInput(dbId);
            CheckChangelogIdInput(changelogId);
        }

        private void CheckDatabaseInput(Guid dbId)
        {
            if (dbId == Guid.Empty)
                throw new ArgumentException("DbId argument empty");
        }

        private void CheckChangelogIdInput(Guid changelogId)
        {
            if (changelogId == Guid.Empty)
                throw new ArgumentException("ChangelogId argument empty");
        }

        private void CheckTopicIdInput(string topicId)
        {
            if (topicId == null || !new Regex(RegexUtils.Dcv).IsMatch(topicId))
                throw new ArgumentException("TopicID is invalid");
        }

        private void CheckSaveRelationInput(ISaveRelation relation)
        {
            if (relation is null)
                throw new ArgumentNullException(nameof(relation));
        }

        private IRelation Map(IRepo.Interface.IRelation relation) => relation == null
            ? null
            : new Relation
            {
                ChangelogId = relation.ChangelogId,
                InitiatorUserEmail = relation.InitiatorUserEmail,
                ReviewerUserEmail = relation.ReviewerUserEmail,
                TimestampChanged = relation.TimestampChanged,
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

        private IRepo.Interface.IRelation EnrichSaveRelation(Guid dbId, ISaveRelation saveRelation) => saveRelation is null
            ? null
            : new Repo.Relation
            {
                TenantId = _token.TenantId,
                DatabaseId = dbId,
                DataLanguage = Map(_dataLanguage),
                InitiatorUserEmail = _token.Email,
                TopicId = saveRelation.TopicId,
                RelationId = saveRelation.RelationId,
                Action = Map(saveRelation.Action),
                OldCategory = saveRelation.OldCategory,
                OldTopicId = saveRelation.OldTopicId,
                Category = saveRelation.Category,
                ToTopicId = saveRelation.ToTopicId,
                TimestampChanged = DateTime.Now,
                Status = Map(Interfaces.v1.Enum.ChangeStatus.Pending)
            };

        private IRepo.Enum.Action Map(Interfaces.v1.Enum.Action action) => action switch
        {
            Interfaces.v1.Enum.Action.Create => IRepo.Enum.Action.Create,
            Interfaces.v1.Enum.Action.Delete => IRepo.Enum.Action.Delete,
            Interfaces.v1.Enum.Action.Edit => IRepo.Enum.Action.Edit,
            _ => throw new ArgumentOutOfRangeException(nameof(action)),
        };

        private Interfaces.v1.Enum.Action Map(IRepo.Enum.Action action) => action switch
        {
            IRepo.Enum.Action.Create => Interfaces.v1.Enum.Action.Create,
            IRepo.Enum.Action.Delete => Interfaces.v1.Enum.Action.Delete,
            IRepo.Enum.Action.Edit => Interfaces.v1.Enum.Action.Edit,
            _ => throw new ArgumentOutOfRangeException(nameof(action)),
        };

        private IRepo.Enum.ChangeStatus Map(Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum.ChangeStatus status) => status switch
        {
            Interfaces.v1.Enum.ChangeStatus.Approved => IRepo.Enum.ChangeStatus.Approved,
            Interfaces.v1.Enum.ChangeStatus.Pending => IRepo.Enum.ChangeStatus.Pending,
            Interfaces.v1.Enum.ChangeStatus.Rejected => IRepo.Enum.ChangeStatus.Rejected,
            _ => throw new ArgumentOutOfRangeException(nameof(status)),
        };

        private static Interfaces.v1.Enum.ChangeStatus Map(IRepo.Enum.ChangeStatus task)
        {
            return task switch
            {
                IRepo.Enum.ChangeStatus.Approved => Interfaces.v1.Enum.ChangeStatus.Approved,
                IRepo.Enum.ChangeStatus.Pending => Interfaces.v1.Enum.ChangeStatus.Pending,
                IRepo.Enum.ChangeStatus.Rejected => Interfaces.v1.Enum.ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(task))
            };
        }

        private static IRepo.Enum.DataLanguageType Map(IRepo.Enum.DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                IRepo.Enum.DataLanguageType.Dutch => IRepo.Enum.DataLanguageType.Dutch,
                IRepo.Enum.DataLanguageType.English => IRepo.Enum.DataLanguageType.English,
                _ => throw new ArgumentException($"unsupported DataLanguage: {dataLanguage.ToString()}")
            };

        private static IRepo.Enum.DataLanguageType Map(IMiddleware.Enums.DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                IMiddleware.Enums.DataLanguageType.Dutch => IRepo.Enum.DataLanguageType.Dutch,
                IMiddleware.Enums.DataLanguageType.English => IRepo.Enum.DataLanguageType.English,
                _ => throw new ArgumentException(string.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };
    }

}
