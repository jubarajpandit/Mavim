using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Business.v1.Mappers;
using Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Business.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Business.v1
{
    public class FieldsBusiness : IFieldBusiness
    {
        private readonly ILogger<FieldsBusiness> _logger;
        private IRepo.Fields.IFieldRepository Repository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldsBusiness" /> class.
        /// </summary>
        /// <param name="fieldRepository">The field repository.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// logger
        /// or
        /// fieldRepository
        /// </exception>
        public FieldsBusiness(IRepo.Fields.IFieldRepository fieldRepository, ILogger<FieldsBusiness> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Repository = fieldRepository ?? throw new ArgumentNullException(nameof(fieldRepository));
        }

        /// <summary>
        /// Gets a list of fields
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<IField>> GetFields(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IEnumerable<IRepo.Fields.IField> fields = await Repository.GetFields(dcvId);
            return fields.Select(Map);
        }

        /// <summary>
        /// Gets a field
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="fieldSetDefinitionId">The field set definition identifier.</param>
        /// <param name="fieldDefinitionId">The field definition identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IField> GetField(string dcvId, string fieldSetDefinitionId, string fieldDefinitionId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            if (!DcvUtils.IsValid(fieldSetDefinitionId))
                throw new BadRequestException($"FieldSetDefinitionID {fieldSetDefinitionId}");

            if (!DcvUtils.IsValid(fieldDefinitionId))
                throw new BadRequestException($"FieldDefinitionID {fieldDefinitionId}");

            IRepo.Fields.IField field = await Repository.GetField(dcvId, fieldSetDefinitionId, fieldDefinitionId);
            return Map(field);
        }

        /// <summary>
        /// Updates the field value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid DCV ID: {dcvId}.</exception>
        public async Task<IField> UpdateFieldValue(IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            if (!DcvUtils.IsValid(field.TopicId))
                throw new BadRequestException($"Invalid DCV ID: { field.TopicId }.");

            IRepo.Fields.IField repoField = Map(field);

            IRepo.Fields.IField repoUpdatedField = await Repository.UpdateFieldValue(repoField, field.TopicId, field.FieldSetId, field.FieldId);

            return Map(repoUpdatedField);
        }

        /// <summary>
        /// Updates the field values.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">fields</exception>
        public async Task<IBulkResult<IField>> UpdateFieldValues(List<IField> fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            BulkResult<IField> bulkResult = new BulkResult<IField>
            {
                Succeeded = new List<IField>(),
                Failed = new List<IFailed<IField>>()
            };

            fields.ToList().ForEach(f =>
            {
                if (!DcvUtils.IsValid(f.TopicId))
                    bulkResult.Failed.Add(Map(f, "Inconsistent TopicId"));

                if (!DcvUtils.IsValid(f.FieldSetId))
                    bulkResult.Failed.Add(Map(f, "Inconsistent FieldSetId"));

                if (!DcvUtils.IsValid(f.FieldId))
                    bulkResult.Failed.Add(Map(f, "Inconsistent FieldId"));
            });

            IEnumerable<IField> validFields = fields.Where(f => !bulkResult.Failed.Select(o => o.Item.FieldId).Contains(f.FieldId));

            Task[] tasks = validFields.Select(async field => await UpdateFieldValueAsync(field, bulkResult)).ToArray();

            await Task.WhenAll(tasks);

            return bulkResult;
        }

        #region Private Methods
        private async Task UpdateFieldValueAsync(IField field, BulkResult<IField> bulkResult)
        {
            try
            {
                IRepo.Fields.IField repoField = Map(field);
                IRepo.Fields.IField repoUpdatedField =
                await Repository.UpdateFieldValue(repoField, field.TopicId, field.FieldSetId, field.FieldId);
                IField BusinessField = Map(repoUpdatedField);
                bulkResult.Succeeded.Add(BusinessField);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                bulkResult.Failed.Add(Map(field, e.Message));
            }
        }

        private static IFailed<IField> Map(IField field, string reason)
        {
            return new Failed<IField> { Item = field, Reason = reason ?? "Invalid field" };
        }

        private static IField Map(IRepo.Fields.IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            FieldMapperBase mapper = FieldMapperFactory.GetFieldMapper(field.FieldValueType);

            return mapper?.GetMappedBusinessField(field);
        }

        private static IRepo.Fields.IField Map(IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            FieldMapperBase mapper = FieldMapperFactory.GetFieldMapper(field.FieldValueType);

            return mapper?.GetMappedRepoField(field);
        }
        #endregion
    }
}