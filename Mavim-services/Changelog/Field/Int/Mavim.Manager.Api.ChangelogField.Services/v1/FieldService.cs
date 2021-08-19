using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepo = Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1;
using Repo = Mavim.Manager.Api.ChangelogField.Repository.v1.Model;

namespace Mavim.Manager.Api.ChangelogField.Services.v1
{
    public class FieldService : IFieldService
    {
        #region Private Members
        private readonly IRepo.IFieldRepository _fieldRepository;
        private readonly IJwtSecurityToken _token;
        #endregion

        public FieldService(IRepo.IFieldRepository changelogRepository, IJwtSecurityToken token)
        {
            _fieldRepository = changelogRepository ?? throw new ArgumentNullException(nameof(changelogRepository));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public async Task<IEnumerable<IChangelogField>> GetFields(Guid dbId, DataLanguageType dataLanguage, string topicId)
        {
            ValidateDatabaseId(dbId);
            ValidateTopicId(topicId);

            IEnumerable<IRepo.IChangelogField> result = await _fieldRepository.GetFieldsByTopic(_token.TenantId, dbId, Map(dataLanguage), topicId);
            return result.Select(Map);
        }

        public async Task<IEnumerable<IChangelogField>> GetPendingFieldsByTopic(Guid dbId, DataLanguageType dataLanguage, string topicId)
        {
            ValidateDatabaseId(dbId);
            ValidateTopicId(topicId);

            IEnumerable<IRepo.IChangelogField> result = await _fieldRepository.GetFieldsByTopicAndStatus(_token.TenantId, dbId, Map(dataLanguage), topicId, IRepo.Enum.ChangeStatus.Pending);

            return result.Select(Map);
        }

        public async Task<IEnumerable<IChangelogField>> GetPendingFields(Guid dbId, DataLanguageType dataLanguage)
        {
            ValidateDatabaseId(dbId);

            IEnumerable<IRepo.IChangelogField> result = await _fieldRepository.GetFieldsByStatus(_token.TenantId, dbId, Map(dataLanguage), IRepo.Enum.ChangeStatus.Pending);
            return result.Select(Map);
        }

        public async Task<ChangeStatus> GetTopicStatusByFields(Guid dbId, DataLanguageType dataLanguage, string topicId)
        {
            ValidateDatabaseId(dbId);
            ValidateTopicId(topicId);

            IEnumerable<IRepo.IChangelogField> result = await _fieldRepository.GetFieldsByTopic(_token.TenantId, dbId, Map(dataLanguage), topicId);
            IEnumerable<IRepo.IChangelogField> groupedResult = result.GroupBy(x => new { x.FieldId, x.FieldSetId }, (key, g) => g.OrderByDescending(e => e.TimestampChanged).FirstOrDefault());

            ChangeStatus fieldStatus = ChangeStatus.Approved;
            if (groupedResult.Any(x => x.Status == IRepo.Enum.ChangeStatus.Rejected)) fieldStatus = ChangeStatus.Rejected;
            if (groupedResult.Any(x => x.Status == IRepo.Enum.ChangeStatus.Pending)) fieldStatus = ChangeStatus.Pending;

            return fieldStatus;
        }

        public async Task SaveField(ISaveFieldChange field)
        {
            ValidateSaveFieldChange(field);

            field.TenantId = _token.TenantId;
            field.InitiatorEmail = _token.Email;
            field.TimestampChanged = DateTime.Now;
            field.Status = ChangeStatus.Pending;

            await _fieldRepository.SaveField(Map(field));
        }

        public async Task<IChangelogField> ApproveField(Guid dbId, DataLanguageType dataLanguage, Guid changelogId)
        {
            ValidateDatabaseId(dbId);
            ValidateChangelogId(changelogId);

            return await UpdateFieldStatus(dbId, dataLanguage, changelogId, ChangeStatus.Approved);
        }

        public async Task<IChangelogField> RejectField(Guid dbId, DataLanguageType dataLanguage, Guid changelogId)
        {
            ValidateDatabaseId(dbId);
            ValidateChangelogId(changelogId);

            return await UpdateFieldStatus(dbId, dataLanguage, changelogId, ChangeStatus.Rejected);
        }

        #region Private Methods
        private async Task<IChangelogField> UpdateFieldStatus(Guid dbId, DataLanguageType dataLanguage, Guid changelogId, ChangeStatus status)
        {
            IRepo.IChangelogField field = await _fieldRepository.GetField(changelogId, _token.TenantId);
            if (field == null || field.DatabaseId != dbId || field.DataLanguage != Map(dataLanguage) || field.Status != IRepo.Enum.ChangeStatus.Pending)
                throw new RequestNotFoundException($"No Pending field found with arguments: --dbId: {dbId} --dataLanguage: {dataLanguage} --changelogId: {changelogId}");

            IRepo.IChangelogField result = await _fieldRepository.UpdateFieldStatus(changelogId, _token.TenantId, _token.Email, Map(status));

            return Map(result);
        }

        private void ValidateDatabaseId(Guid dbId)
        {
            if (dbId == Guid.Empty)
                throw new ArgumentException(nameof(dbId));
        }

        private void ValidateChangelogId(Guid changelogId)
        {
            if (changelogId == Guid.Empty)
                throw new ArgumentException(nameof(changelogId));
        }

        private void ValidateTopicId(string topicId)
        {
            if (!DcvUtils.IsValid(topicId))
                throw new ArgumentException(nameof(topicId));
        }

        private void ValidateSaveFieldChange(ISaveFieldChange field)
        {
            if (field == null)
                throw new ArgumentException(nameof(field));

            if (field.DatabaseId == Guid.Empty)
                throw new ArgumentException(nameof(field.DatabaseId));

            if (!DcvUtils.IsValid(field.TopicId))
                throw new ArgumentException(nameof(field.TopicId));

            if (!DcvUtils.IsValid(field.FieldSetId))
                throw new ArgumentException(nameof(field.FieldSetId));

            if (!DcvUtils.IsValid(field.FieldId))
                throw new ArgumentException(nameof(field.FieldId));

            if (field.OldFieldValue == field.NewFieldValue)
                throw new ForbiddenRequestException("Not allowed to have equal old and new values when saving a field");
        }

        private IChangelogField Map(IRepo.IChangelogField field) =>
            field == null ? null : new Model.ChangelogField
            {
                Id = field.Id,
                TenantId = field.TenantId,
                DatabaseId = field.DatabaseId,
                DataLanguage = Map(field.DataLanguage),
                InitiatorEmail = field.InitiatorEmail,
                TimestampChanged = field.TimestampChanged,
                ReviewerEmail = field.ReviewerEmail,
                TimestampReviewed = field.TimestampReviewed,
                TopicId = field.TopicId,
                Status = Map(field.Status),
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                Type = Map(field.Type),
                OldFieldValue = field.OldFieldValue,
                NewFieldValue = field.NewFieldValue
            };

        private IRepo.ISaveFieldChange Map(ISaveFieldChange field) =>
            field == null ? null : new Repo.SaveFieldChange
            {
                TenantId = field.TenantId,
                DatabaseId = field.DatabaseId,
                DataLanguage = Map(field.DataLanguage),
                InitiatorEmail = field.InitiatorEmail,
                TimestampChanged = field.TimestampChanged,
                TopicId = field.TopicId,
                Status = Map(field.Status),
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                Type = Map(field.Type),
                OldFieldValue = field.OldFieldValue,
                NewFieldValue = field.NewFieldValue
            };

        private DataLanguageType Map(IRepo.Enum.DataLanguageType type) =>
            type switch
            {
                IRepo.Enum.DataLanguageType.Dutch => DataLanguageType.Dutch,
                IRepo.Enum.DataLanguageType.English => DataLanguageType.English,
                _ => throw new ArgumentOutOfRangeException("DataLanguage not supported")
            };

        private IRepo.Enum.DataLanguageType Map(DataLanguageType type) =>
            type switch
            {
                DataLanguageType.Dutch => IRepo.Enum.DataLanguageType.Dutch,
                DataLanguageType.English => IRepo.Enum.DataLanguageType.English,
                _ => throw new ArgumentOutOfRangeException("DataLanguage not supported")
            };

        private ChangeStatus Map(IRepo.Enum.ChangeStatus status) =>
            status switch
            {
                IRepo.Enum.ChangeStatus.Approved => ChangeStatus.Approved,
                IRepo.Enum.ChangeStatus.Rejected => ChangeStatus.Rejected,
                IRepo.Enum.ChangeStatus.Pending => ChangeStatus.Pending,
                _ => throw new ArgumentOutOfRangeException("Status not supported")
            };

        private IRepo.Enum.ChangeStatus Map(ChangeStatus status) =>
            status switch
            {
                ChangeStatus.Approved => IRepo.Enum.ChangeStatus.Approved,
                ChangeStatus.Rejected => IRepo.Enum.ChangeStatus.Rejected,
                ChangeStatus.Pending => IRepo.Enum.ChangeStatus.Pending,
                _ => throw new ArgumentOutOfRangeException("Status not supported")
            };

        private FieldType Map(IRepo.Enum.FieldType status) =>
            status switch
            {
                IRepo.Enum.FieldType.Boolean => FieldType.Boolean,
                IRepo.Enum.FieldType.Date => FieldType.Date,
                IRepo.Enum.FieldType.Decimal => FieldType.Decimal,
                IRepo.Enum.FieldType.List => FieldType.List,
                IRepo.Enum.FieldType.MultiDate => FieldType.MultiDate,
                IRepo.Enum.FieldType.MultiDecimal => FieldType.MultiDecimal,
                IRepo.Enum.FieldType.MultiNumber => FieldType.MultiNumber,
                IRepo.Enum.FieldType.MultiRelationship => FieldType.MultiRelationship,
                IRepo.Enum.FieldType.MultiText => FieldType.MultiText,
                IRepo.Enum.FieldType.Number => FieldType.Number,
                IRepo.Enum.FieldType.Relationship => FieldType.Relationship,
                IRepo.Enum.FieldType.RelationshipList => FieldType.RelationshipList,
                IRepo.Enum.FieldType.Text => FieldType.Text,
                IRepo.Enum.FieldType.Unknown => FieldType.Unknown,
                _ => throw new ArgumentOutOfRangeException("Fieldtype not supported")
            };

        private IRepo.Enum.FieldType Map(FieldType status) =>
            status switch
            {
                FieldType.Boolean => IRepo.Enum.FieldType.Boolean,
                FieldType.Date => IRepo.Enum.FieldType.Date,
                FieldType.Decimal => IRepo.Enum.FieldType.Decimal,
                FieldType.List => IRepo.Enum.FieldType.List,
                FieldType.MultiDate => IRepo.Enum.FieldType.MultiDate,
                FieldType.MultiDecimal => IRepo.Enum.FieldType.MultiDecimal,
                FieldType.MultiNumber => IRepo.Enum.FieldType.MultiNumber,
                FieldType.MultiRelationship => IRepo.Enum.FieldType.MultiRelationship,
                FieldType.MultiText => IRepo.Enum.FieldType.MultiText,
                FieldType.Number => IRepo.Enum.FieldType.Number,
                FieldType.Relationship => IRepo.Enum.FieldType.Relationship,
                FieldType.RelationshipList => IRepo.Enum.FieldType.RelationshipList,
                FieldType.Text => IRepo.Enum.FieldType.Text,
                FieldType.Unknown => IRepo.Enum.FieldType.Unknown,
                _ => throw new ArgumentOutOfRangeException("Fieldtype not supported")
            };
        #endregion
    }
}
