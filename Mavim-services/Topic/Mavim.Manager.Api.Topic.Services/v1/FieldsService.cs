using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1.Mappers;
using Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1
{
    public class FieldsService : IFieldService
    {
        private readonly ILogger<FieldsService> _logger;
        private IBusiness.Fields.IFieldBusiness Business { get; }
        private IBusiness.ITopicBusiness TopicBusiness { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldsService" /> class.
        /// </summary>
        /// <param name="fieldBusiness">The field Business.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// logger
        /// or
        /// fieldBusiness
        /// </exception>
        public FieldsService(IBusiness.Fields.IFieldBusiness fieldBusiness, IBusiness.ITopicBusiness topicBusiness, ILogger<FieldsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Business = fieldBusiness ?? throw new ArgumentNullException(nameof(fieldBusiness));
            TopicBusiness = topicBusiness ?? throw new ArgumentNullException(nameof(topicBusiness));
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

            IEnumerable<IBusiness.Fields.IField> fields = await Business.GetFields(dcvId);
            return fields.Select(MapToService);
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

            IBusiness.Fields.IField field = await Business.GetField(dcvId, fieldSetDefinitionId, fieldDefinitionId);
            return MapToService(field);
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

            IBusiness.ITopic businessTopic = await TopicBusiness.GetTopic(field.TopicId);

            if (businessTopic == null || businessTopic.IsReadOnly)
                throw new ForbiddenRequestException("Update of topic is forbidden");

            IBusiness.Fields.IField businessField = MapToBusiness(field);
            IBusiness.Fields.IField businessUpdatedField = await Business.UpdateFieldValue(businessField);

            return MapToService(businessUpdatedField);
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

            IBusiness.ITopic businessTopic = await TopicBusiness.GetTopic(fields.FirstOrDefault()?.TopicId);

            if (businessTopic == null || businessTopic.IsReadOnly)
                throw new ForbiddenRequestException("Update of topic is forbidden");

            IBusiness.Fields.IBulkResult<IBusiness.Fields.IField> bulkResult = await Business.UpdateFieldValues(fields.Select(MapToBusiness).ToList());

            return MapToService(bulkResult);
        }

        #region Private Methods

        private static IField MapToService(IBusiness.Fields.IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            FieldMapperBase mapper = FieldMapperFactory.GetFieldMapper(field.FieldValueType);

            return mapper?.GetMappedServiceField(field);
        }

        private static IBusiness.Fields.IField MapToBusiness(IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            FieldMapperBase mapper = FieldMapperFactory.GetFieldMapper(field.FieldValueType);

            return mapper?.GetMappedBusinessField(field);
        }

        private static IBulkResult<IField> MapToService(IBusiness.Fields.IBulkResult<IBusiness.Fields.IField> bulkResult) =>
            new BulkResult<IField>
            {
                Succeeded = bulkResult?.Succeeded?.Select(MapToService).ToList(),
                Failed = bulkResult?.Failed?.Select(MapToService).ToList()
            };

        private static IFailed<IField> MapToService(IBusiness.Fields.IFailed<IBusiness.Fields.IField> failed) =>
            new Failed<IField>
            {
                Item = MapToService(failed?.Item),
                Reason = failed?.Reason
            };
        #endregion
    }
}