using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Factory;
using Mavim.Manager.Api.Utils;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Repository.v1
{
    public class FieldsRepository : IFieldRepository
    {
        private readonly IMavimDatabaseModel _model;
        private readonly FieldMapperFactory _fieldMapperFactory;

        public FieldsRepository(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage, FieldMapperFactory fieldMapperFactory)
        {
            _model = dataAccess?.DatabaseModel ?? throw new ArgumentNullException(nameof(dataAccess));
            _model.DataLanguage = new Language(Map(dataLanguage.Type));
            _fieldMapperFactory = fieldMapperFactory ?? throw new ArgumentNullException(nameof(fieldMapperFactory));
        }

        /// <summary>
        /// Gets the fields by DCV from the Mavim database by establishing the connection with Mavim database using on behalf of access token..
        /// </summary>
        /// <param name="fieldSetId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IField>> GetFields(string fieldSetId)
        {
            if (DcvId.FromDcvKey(fieldSetId) == null)
                throw new ArgumentException($"FieldId not in right format", fieldSetId);

            IElement fieldSet = GetElementByDcvId(DcvId.FromDcvKey(fieldSetId));

            if (fieldSet == null)
                throw new ArgumentException($"Could not find fields with FieldsetId '{fieldSetId}'");

            List<IFieldSet> fieldSets = GetAllFieldSetsFromElement(fieldSet).ToList();

            List<IField> repoFields = (await Task.WhenAll(fieldSets
                                            .SelectMany((fieldset) => fieldset
                                            .Where(field => field.FieldDefinition != null)
                                            .Select(async x => await Map(x)))
                                            .Where(field => field != null))).ToList();

            return await Task.FromResult(repoFields.Where(field => field != null).ToList());
        }

        /// <summary>
        /// Gets the field by DCV and field identifier.
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="fieldSetId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public async Task<IField> GetField(string topicId, string fieldSetId, string fieldId)
        {
            if (!DcvUtils.IsValid(topicId))
                throw new ArgumentException("TopicId not in right format", topicId);

            if (!DcvUtils.IsValid(fieldSetId))
                throw new ArgumentException("FieldSetId not in right format", topicId);

            if (!DcvUtils.IsValid(fieldId))
                throw new ArgumentException("FieldId not in right format", topicId);

            IElement topic = GetElementByDcvId(DcvId.FromDcvKey(topicId));

            if (topic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{topicId}'");

            IEnumerable<IFieldSet> fieldSets = GetAllFieldSetsFromElement(topic);

            ISimpleField simpleField = FindFieldByFieldId(fieldSets, fieldSetId, fieldId);

            return await Task.FromResult(await Map(simpleField));
        }

        /// <summary>
        /// Updates the field value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="fieldSetId">The field set identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">FieldId not in right format</exception>
        /// <exception cref="ArgumentNullException">
        /// fieldSetId
        /// or
        /// fieldId
        /// </exception>
        /// <exception cref="Exception">Error while logging in to the Mavim database: {Session.LoginError.ToString()}</exception>
        /// <exception cref="InvalidOperationException">No element found for DCV id: {dcvId}.</exception>
        public async Task<IField> UpdateFieldValue(IField field, string dcvId, string fieldSetId, string fieldId)
        {
            IDcvId dcv = DcvId.FromDcvKey(dcvId);

            if (field == null) throw new ArgumentNullException(nameof(field));

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (dcv == null)
                throw new ArgumentException("DcvId not in right format", dcvId);

            if (string.IsNullOrWhiteSpace(fieldSetId))
                throw new ArgumentNullException(nameof(fieldSetId));

            if (string.IsNullOrWhiteSpace(fieldId))
                throw new ArgumentNullException(nameof(fieldId));

            IElement baseTopic = GetElementByDcvId(dcv);

            if (baseTopic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{dcvId}'");

            IEnumerable<IFieldSet> fieldSets = GetAllFieldSetsFromElement(baseTopic).ToList();
            ISimpleField simpleField = FindFieldByFieldId(fieldSets, fieldSetId, fieldId);

            IField fieldToUpdate = await Map(simpleField);

            if (fieldToUpdate.FieldValueType != field.FieldValueType)
                throw new RequestNotFoundException($"Field with Id {field.FieldId} is not of type {field.FieldValueType}");

            FieldMapperBase fieldMapper = await _fieldMapperFactory.GetFieldMapper(field.FieldValueType);

            object[] updateFieldValues = fieldMapper.ConvertToArrayObject(field, simpleField);

            UpdateFields(simpleField, updateFieldValues);

            simpleField = FindFieldByFieldId(fieldSets, fieldSetId, fieldId);

            return await Task.FromResult(await Map(simpleField));
        }

        #region Private Methods

        /// <summary>
        /// Map switch for Single / Multi Simple Field Value
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private async Task<IField> Map(ISimpleField field)
        {
            FieldMapperBase fieldMapper = await _fieldMapperFactory.GetFieldMapper(field);

            return fieldMapper?.GetMappedRepoField(field);
        }

        private void UpdateFields(ISimpleField field, object[] fieldValue)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (fieldValue == null) throw new ArgumentNullException(nameof(fieldValue));

            if (field is ISingleValueSimpleField singleValueSimpleField)
            {
                singleValueSimpleField.FieldValue = fieldValue.FirstOrDefault();
                return;
            }

            if (!(field is IMultiValueSimpleField multiValueSimpleField))
                throw new ArgumentException("Invalid field type.");

            multiValueSimpleField.RemoveAllItems();
            multiValueSimpleField.AddValues(fieldValue);
        }

        /// <summary>
        /// Gets the element by DCV identifier.
        /// </summary>
        /// <param name="dcvId"></param>
        /// <returns></returns>
        private IElement GetElementByDcvId(IDcvId dcvId)
        {
            return _model.ElementRepository.GetElement(dcvId);
        }

        /// <summary>
        /// Gets all field sets from element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The element does not have a connection to the database - element</exception>
        private IEnumerable<IFieldSet> GetAllFieldSetsFromElement(IElement element)
        {
            if (element.Model == null || element.Model.MavimHandle == -1)
                throw new ArgumentException("The element does not have a connection to the database", nameof(element));

            return element.GetFields(element.Model.DataLanguage);
        }

        /// <summary>
        /// Find SimpleField by ID
        /// </summary>
        /// <param name="fieldSets">The field sets.</param>
        /// <param name="fieldSetId">The field set identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// fieldSets
        /// or
        /// fieldSetId
        /// or
        /// fieldId
        /// </exception>
        private ISimpleField FindFieldByFieldId(IEnumerable<IFieldSet> fieldSets, string fieldSetId, string fieldId)
        {
            if (fieldSets == null) throw new ArgumentNullException(nameof(fieldSets));
            if (string.IsNullOrWhiteSpace(fieldSetId)) throw new ArgumentNullException(nameof(fieldSetId));
            if (string.IsNullOrWhiteSpace(fieldId)) throw new ArgumentNullException(nameof(fieldId));

            return fieldSets
                .Where(fieldSet => fieldSetId.Equals(fieldSet.Definition.ID))
                .SelectMany(fieldSet => fieldSet)
                .FirstOrDefault(simpleField => fieldId.Equals(simpleField.FieldDefinition.ID));
        }

        private static LanguageSupport.MvmSRV_Lang Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_NL,
                DataLanguageType.English => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_EN,
                _ => throw new ArgumentException(string.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };
        #endregion
    }
}
