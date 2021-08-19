using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using Mavim.Manager.Api.Topic.v1.Mappers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModels = Mavim.Manager.Api.Topic.v1.Models;

namespace Mavim.Manager.Api.Topic.v1.Mappers
{
    /// <summary>
    /// FieldMapper
    /// </summary>
    public class FieldMapper : IFieldMapper
    {
        private readonly ILogger<FieldMapper> _logger;
        private readonly IFeatureManager _featureManager;
        private readonly string _hyperlinkFeature = "Hyperlink";

        /// <summary>
        /// FieldMapper
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="featureManager"></param>
        public FieldMapper(ILogger<FieldMapper> logger, IFeatureManager featureManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// MapFields
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<List<IField>> MapFields(string topicId, ApiModels.Fields fields)
        {
            List<IField> mappedFields = new List<IField>();
            mappedFields.AddRange(fields.SingleTextFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.MultiTextFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.SingleNumberFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.MultiNumberFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.SingleBooleanFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.SingleDecimalFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.MultiDecimalFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.SingleDateFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.MultiDateFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.SingleListFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.SingleRelationshipFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.MultiRelationshipFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            mappedFields.AddRange(fields.SingleRelationshipListFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());

            if (await _featureManager.IsEnabledAsync(_hyperlinkFeature))
            {
                mappedFields.AddRange(fields.SingleHyperlinkFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
                mappedFields.AddRange(fields.MultiHyperlinkFields?.Select(field => MapField(field, topicId, field.FieldSetId, field.FieldId)) ?? Array.Empty<IField>());
            }

            return mappedFields;
        }

        /// <summary>
        /// MapField
        /// </summary>
        /// <param name="field"></param>
        /// <param name="topicId"></param>
        /// <param name="fieldSetId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public IField MapField(ApiModels.Field field, string topicId, string fieldSetId, string fieldId)
        {
            return field switch
            {
                ApiModels.RelationshipListField relationshipListField =>
                            Enrich(relationshipListField, topicId, fieldSetId, fieldId),
                ApiModels.RelationshipField relationshipField =>
                            Enrich(relationshipField, topicId, fieldSetId, fieldId),
                ApiModels.MultiRelationshipField multiRelationshipField =>
                            Enrich(multiRelationshipField, topicId, fieldSetId, fieldId),
                _ => throw new Exception()
            };
        }

        /// <summary>
        /// MapField
        /// </summary>
        /// <param name="field"></param>
        /// <param name="topicId"></param>
        /// <param name="fieldSetId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public IField MapField(IField field, string topicId, string fieldSetId, string fieldId)
        {
            switch (field)
            {
                case SingleTextField _:
                    field.FieldValueType = FieldType.Text;
                    break;
                case MultiTextField _:
                    field.FieldValueType = FieldType.MultiText;
                    break;
                case SingleNumberField _:
                    field.FieldValueType = FieldType.Number;
                    break;
                case MultiNumberField _:
                    field.FieldValueType = FieldType.MultiNumber;
                    break;
                case SingleBooleanField _:
                    field.FieldValueType = FieldType.Boolean;
                    break;
                case SingleDecimalField _:
                    field.FieldValueType = FieldType.Decimal;
                    break;
                case MultiDecimalField _:
                    field.FieldValueType = FieldType.MultiDecimal;
                    break;
                case SingleDateField _:
                    field.FieldValueType = FieldType.Date;
                    break;
                case MultiDateField _:
                    field.FieldValueType = FieldType.MultiDate;
                    break;
                case SingleListField _:
                    field.FieldValueType = FieldType.List;
                    break;
                case SingleHyperlinkField _:
                    field.FieldValueType = FieldType.Hyperlink;
                    break;
                case MultiHyperlinkField _:
                    field.FieldValueType = FieldType.MultiHyperlink;
                    break;
                default:
                    _logger.LogWarning($"Unknown field type: {field}. Cannot set field value type.");
                    break;
            }

            field.FieldSetId = fieldSetId;
            field.TopicId = topicId;
            field.FieldId = fieldId;
            return field;
        }

        /// <summary>
        /// Enrich
        /// </summary>
        /// <param name="field"></param>
        /// <param name="topicId"></param>
        /// <param name="fieldSetId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        private IField Enrich(ApiModels.RelationshipField field, string topicId, string fieldSetId, string fieldId)
        {
            return new SingleRelationshipField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = new RelationshipElement { Dcv = field.Data.Dcv, Icon = field.Data.Icon, Name = field.Data.Name },
                FieldValueType = FieldType.Relationship
            };
        }

        private IField Enrich(ApiModels.MultiRelationshipField field, string topicId, string fieldSetId, string fieldId)
        {
            return new MultiRelationshipField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data.Select(topicRelation => new RelationshipElement { Dcv = topicRelation.Dcv, Icon = topicRelation.Icon, Name = topicRelation.Name }),
                FieldValueType = FieldType.MultiRelationship
            };
        }

        private IField Enrich(ApiModels.RelationshipListField field, string topicId, string fieldSetId, string fieldId)
        {
            KeyValuePair<string, ApiModels.RelationshipElement> input = field.Data.FirstOrDefault();
            Dictionary<string, IRelationshipElement> newDictionary = new Dictionary<string, IRelationshipElement>
            {
                {
                    input.Key,
                    Map(input)
                }
            };

            return new SingleRelationshipListField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = newDictionary,
                FieldValueType = FieldType.RelationshipList
            };
        }

        private IRelationshipElement Map(KeyValuePair<string, ApiModels.RelationshipElement> relationResource) =>
            relationResource.Value == null ? null :
            new RelationshipElement()
            {
                Dcv = relationResource.Value.Dcv,
                Name = relationResource.Value.Icon,
                Icon = relationResource.Value.Name
            };
    }
}