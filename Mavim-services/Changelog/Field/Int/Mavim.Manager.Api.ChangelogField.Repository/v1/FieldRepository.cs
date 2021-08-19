using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1;
using Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1.Enum;
using Mavim.Manager.ChangelogField.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel = Mavim.Manager.ChangelogField.DbModel;

namespace Mavim.Manager.Api.ChangelogField.Repository.v1
{
    public class FieldRepository : IFieldRepository
    {
        #region Private Members
        protected readonly FieldDbContext _dbContext;
        protected readonly ILogger<FieldRepository> _logger;
        #endregion
        public FieldRepository(FieldDbContext fieldDbContext, ILogger<FieldRepository> logger)
        {
            _dbContext = fieldDbContext ?? throw new ArgumentNullException(nameof(fieldDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IChangelogField> GetField(Guid changelogId, Guid tenantId)
        {
            _logger.LogTrace($"Retrieving field with arguments: --changelogId: {changelogId} --tenantId: {tenantId}");
            ValidateChangelogId(changelogId);
            ValidateTenantId(tenantId);

            DbModel.ChangelogField result = await _dbContext.Fields.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == changelogId &&
                    x.TenantId == tenantId
                );

            return Map(result);
        }

        public async Task<IEnumerable<IChangelogField>> GetFieldsByTopic(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, string topicId)
        {
            _logger.LogTrace($"Retrieving fields with arguments: --tenantId: {tenantId} --dbId: {dbId} --dataLanguage: {dataLanguage} --topicId: {topicId}");
            ValidateDatabaseId(dbId);
            ValidateTenantId(tenantId);
            ValidateTopicId(topicId);

            IEnumerable<DbModel.ChangelogField> result = await _dbContext.Fields.AsNoTracking()
                .Where(x =>
                    x.TenantId == tenantId &&
                    x.DatabaseId == dbId &&
                    x.DataLanguage == Map(dataLanguage) &&
                    x.TopicId == topicId)
                .ToListAsync();

            return result.Select(Map);
        }

        public async Task<IEnumerable<IChangelogField>> GetFieldsByTopicAndStatus(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, string topicId, ChangeStatus status)
        {
            _logger.LogTrace($"Retrieving fields By Topic and Status with arguments: --tenantId: {tenantId} --dbId: {dbId} --dataLanguage: {dataLanguage} --topicId: {topicId} --status: {status}");
            ValidateDatabaseId(dbId);
            ValidateTenantId(tenantId);
            ValidateTopicId(topicId);

            IEnumerable<DbModel.ChangelogField> result = await _dbContext.Fields.AsNoTracking()
                .Where(x =>
                    x.TenantId == tenantId &&
                    x.DatabaseId == dbId &&
                    x.DataLanguage == Map(dataLanguage) &&
                    x.TopicId == topicId &&
                    x.Status == Map(status))
                .ToListAsync();

            return result.Select(Map);
        }

        public async Task<IEnumerable<IChangelogField>> GetFields(Guid tenantId, Guid dbId, DataLanguageType dataLanguage)
        {
            _logger.LogTrace($"Retrieving fields with arguments: --tenantId: {tenantId} --dbId: {dbId} --dataLanguage: {dataLanguage}");
            ValidateDatabaseId(dbId);
            ValidateTenantId(tenantId);
            IEnumerable<DbModel.ChangelogField> result = await _dbContext.Fields.AsNoTracking()
                .Where(x =>
                    x.TenantId == tenantId &&
                    x.DatabaseId == dbId &&
                    x.DataLanguage == Map(dataLanguage))
                .ToListAsync();

            return result.Select(Map);
        }

        public async Task<IEnumerable<IChangelogField>> GetFieldsByStatus(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, ChangeStatus status)
        {
            _logger.LogTrace($"Retrieving fields by status with arguments: --tenantId: {tenantId} --dbId: {dbId} --dataLanguage: {dataLanguage} --status: {status}");
            ValidateDatabaseId(dbId);
            ValidateTenantId(tenantId);
            IEnumerable<DbModel.ChangelogField> result = await _dbContext.Fields.AsNoTracking()
                .Where(x =>
                    x.TenantId == tenantId &&
                    x.DatabaseId == dbId &&
                    x.DataLanguage == Map(dataLanguage) &&
                    x.Status == Map(status))
                .ToListAsync();

            return result.Select(Map);
        }

        public async Task SaveField(ISaveFieldChange field)
        {
            _logger.LogTrace("Saving field");
            validateSaveField(field);
            _dbContext.Fields.Add(Map(field));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IChangelogField> UpdateFieldStatus(Guid changelogId, Guid tenantId, string reviewerEmail, ChangeStatus status)
        {
            ValidateChangelogId(changelogId);
            ValidateTenantId(tenantId);
            _logger.LogTrace($"Retrieving field with arguments: --changelogId: {changelogId} --tenantId: {tenantId}");
            DbModel.ChangelogField changelogField = await _dbContext.Fields
                .FirstOrDefaultAsync(x =>
                    x.Id == changelogId &&
                    x.TenantId == tenantId
                );

            if (changelogField == null)
                throw new RequestNotFoundException($"No field change found with arguments: --changelogId: {changelogId} --tenantId: {tenantId}");

            _logger.LogTrace($"Updating status field with arguments: --changelogId: {changelogId} --tenantId: {tenantId} --status: {status}");
            changelogField.ReviewerEmail = reviewerEmail;
            changelogField.TimestampReviewed = DateTime.Now;
            changelogField.Status = Map(status);

            _dbContext.Fields.Update(changelogField);
            await _dbContext.SaveChangesAsync();

            return Map(changelogField);
        }

        #region Private methods

        private void validateSaveField(ISaveFieldChange field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
        }

        private void ValidateDatabaseId(Guid dbId)
        {
            if (dbId == Guid.Empty)
                throw new ArgumentNullException(nameof(dbId));
        }

        private void ValidateTenantId(Guid tenantId)
        {
            if (tenantId == Guid.Empty)
                throw new ArgumentNullException(nameof(tenantId));
        }

        private void ValidateChangelogId(Guid changelogId)
        {
            if (changelogId == Guid.Empty)
                throw new ArgumentNullException(nameof(changelogId));
        }

        private void ValidateTopicId(string topicId)
        {
            if (string.IsNullOrWhiteSpace(topicId))
                throw new ArgumentNullException(nameof(topicId));
        }

        private IChangelogField Map(DbModel.ChangelogField field) =>
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

        private DbModel.ChangelogField Map(ISaveFieldChange field) =>
            field == null ? null : new DbModel.ChangelogField
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

        private DataLanguageType Map(DbModel.Enum.DataLanguageType type) =>
            type switch
            {
                DbModel.Enum.DataLanguageType.Dutch => DataLanguageType.Dutch,
                DbModel.Enum.DataLanguageType.English => DataLanguageType.English,
                _ => throw new ArgumentOutOfRangeException("DataLanguage not supported")
            };

        private DbModel.Enum.DataLanguageType Map(DataLanguageType type) =>
            type switch
            {
                DataLanguageType.Dutch => DbModel.Enum.DataLanguageType.Dutch,
                DataLanguageType.English => DbModel.Enum.DataLanguageType.English,
                _ => throw new ArgumentOutOfRangeException("DataLanguage not supported")
            };

        private ChangeStatus Map(DbModel.Enum.ChangeStatus status) =>
            status switch
            {
                DbModel.Enum.ChangeStatus.Approved => ChangeStatus.Approved,
                DbModel.Enum.ChangeStatus.Rejected => ChangeStatus.Rejected,
                DbModel.Enum.ChangeStatus.Pending => ChangeStatus.Pending,
                _ => throw new ArgumentOutOfRangeException("Status not supported")
            };

        private DbModel.Enum.ChangeStatus Map(ChangeStatus status) =>
            status switch
            {
                ChangeStatus.Approved => DbModel.Enum.ChangeStatus.Approved,
                ChangeStatus.Rejected => DbModel.Enum.ChangeStatus.Rejected,
                ChangeStatus.Pending => DbModel.Enum.ChangeStatus.Pending,
                _ => throw new ArgumentOutOfRangeException("Status not supported")
            };

        private FieldType Map(DbModel.Enum.FieldType status) =>
            status switch
            {
                DbModel.Enum.FieldType.Boolean => FieldType.Boolean,
                DbModel.Enum.FieldType.Date => FieldType.Date,
                DbModel.Enum.FieldType.Decimal => FieldType.Decimal,
                DbModel.Enum.FieldType.List => FieldType.List,
                DbModel.Enum.FieldType.MultiDate => FieldType.MultiDate,
                DbModel.Enum.FieldType.MultiDecimal => FieldType.MultiDecimal,
                DbModel.Enum.FieldType.MultiNumber => FieldType.MultiNumber,
                DbModel.Enum.FieldType.MultiRelationship => FieldType.MultiRelationship,
                DbModel.Enum.FieldType.MultiText => FieldType.MultiText,
                DbModel.Enum.FieldType.Number => FieldType.Number,
                DbModel.Enum.FieldType.Relationship => FieldType.Relationship,
                DbModel.Enum.FieldType.RelationshipList => FieldType.RelationshipList,
                DbModel.Enum.FieldType.Text => FieldType.Text,
                DbModel.Enum.FieldType.Unknown => FieldType.Unknown,
                _ => throw new ArgumentOutOfRangeException("Fieldtype not supported")
            };

        private DbModel.Enum.FieldType Map(FieldType status) =>
            status switch
            {
                FieldType.Boolean => DbModel.Enum.FieldType.Boolean,
                FieldType.Date => DbModel.Enum.FieldType.Date,
                FieldType.Decimal => DbModel.Enum.FieldType.Decimal,
                FieldType.List => DbModel.Enum.FieldType.List,
                FieldType.MultiDate => DbModel.Enum.FieldType.MultiDate,
                FieldType.MultiDecimal => DbModel.Enum.FieldType.MultiDecimal,
                FieldType.MultiNumber => DbModel.Enum.FieldType.MultiNumber,
                FieldType.MultiRelationship => DbModel.Enum.FieldType.MultiRelationship,
                FieldType.MultiText => DbModel.Enum.FieldType.MultiText,
                FieldType.Number => DbModel.Enum.FieldType.Number,
                FieldType.Relationship => DbModel.Enum.FieldType.Relationship,
                FieldType.RelationshipList => DbModel.Enum.FieldType.RelationshipList,
                FieldType.Text => DbModel.Enum.FieldType.Text,
                FieldType.Unknown => DbModel.Enum.FieldType.Unknown,
                _ => throw new ArgumentOutOfRangeException("Fieldtype not supported")
            };
        #endregion
    }
}