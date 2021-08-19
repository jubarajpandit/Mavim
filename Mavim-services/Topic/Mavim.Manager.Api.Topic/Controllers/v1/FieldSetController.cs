using Mavim.Libraries.Features.Enums;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using Mavim.Manager.Api.Topic.v1.Mappers.Interfaces;
using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiModels = Mavim.Manager.Api.Topic.v1.Models;

namespace Mavim.Manager.Api.Topic.Controllers.v1
{
    /// <summary>
    /// FieldSet controller for getting updating fields
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/topic/{topicId}/")]
    public class FieldSetController : ControllerBase
    {
        private readonly IFieldMapper _fieldMapper;
        private IFieldService Service { get; set; }

        /// <summary>
        /// FieldSetController Constructor
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="fieldMapper"></param>
        /// <exception cref="ArgumentNullException">service</exception>
        public FieldSetController(IFieldService service, IFieldMapper fieldMapper)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
            _fieldMapper = fieldMapper ?? throw new ArgumentNullException(nameof(fieldMapper));
        }

        /// <summary>
        /// Get the fields from topic
        /// </summary>
        /// <remarks>From a specific topic requested with the topic id, a collection of fields is returned.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<IField>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("fieldsets")]
        public async Task<ActionResult<IEnumerable<IField>>> GetTopicFields(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId)
        {
            return Ok(await Service.GetFields(topicId));
        }

        /// <summary>
        /// Get field from topic
        /// </summary>
        /// <remarks>Get a specific field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("fieldsets/{fieldSetId}/{fieldId}")]
        public async Task<ActionResult<IField>> GetFieldByDcvAndFieldsetId(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId)
        {
            return Ok(await Service.GetField(topicId, fieldSetId, fieldId));
        }

        /// <summary>
        /// Update boolean field from topic
        /// </summary>
        /// <remarks>Updates a specific boolean field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/bool/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateBooleanSingleField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] SingleBooleanField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            SingleBooleanField singleBooleanField = new SingleBooleanField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data,
                FieldValueType = FieldType.Boolean
            };

            return Ok(await Service.UpdateFieldValue(singleBooleanField));
        }

        /// <summary>
        /// Update text field from topic
        /// </summary>
        /// <remarks>Updates a specific single value text field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/text/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateTextSingleField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] SingleTextField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            SingleTextField singleTextField = new SingleTextField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field?.Data,
                FieldValueType = FieldType.Text
            };

            return Ok(await Service.UpdateFieldValue(singleTextField));
        }

        /// <summary>
        /// Update multi value text field from topic
        /// </summary>
        /// <remarks>Updates a specific multi value text field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/multitext/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateTextMultiField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] MultiTextField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            MultiTextField multiTextField = new MultiTextField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field?.Data,
                FieldValueType = FieldType.MultiText
            };

            return Ok(await Service.UpdateFieldValue(multiTextField));
        }

        /// <summary>
        /// Update number field from topic
        /// </summary>
        /// <remarks>Updates a specific single value number field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/number/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateNumberSingleField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] SingleNumberField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            SingleNumberField singleNumberField = new SingleNumberField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data,
                FieldValueType = FieldType.Number
            };

            return Ok(await Service.UpdateFieldValue(singleNumberField));
        }

        /// <summary>
        /// Update multivalue number field from topic
        /// </summary>
        /// <remarks>Updates a specific boolean field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/multinumber/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateNumberMultiField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] MultiNumberField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            MultiNumberField multiNumberField = new MultiNumberField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field?.Data,
                FieldValueType = FieldType.MultiNumber
            };

            return Ok(await Service.UpdateFieldValue(multiNumberField));
        }

        /// <summary>
        /// Update decimal field from topic
        /// </summary>
        /// <remarks>Updates a specific single value decimal field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/decimal/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateDecimalSingleField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] SingleDecimalField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            SingleDecimalField singleDecimalField = new SingleDecimalField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data,
                FieldValueType = FieldType.Decimal
            };

            return Ok(await Service.UpdateFieldValue(singleDecimalField));
        }

        /// <summary>
        /// Update multi value decimal field from topic
        /// </summary>
        /// <remarks>Updates a specific multi value decimal field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/multidecimal/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateDecimalMultiField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] MultiDecimalField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            MultiDecimalField multiDecimalField = new MultiDecimalField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data,
                FieldValueType = FieldType.MultiDecimal
            };

            return Ok(await Service.UpdateFieldValue(multiDecimalField));
        }

        /// <summary>
        /// Update date field from topic
        /// </summary>
        /// <remarks>Updates a specific single value date field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/date/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateDateSingleField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] SingleDateField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            SingleDateField singleDateField = new SingleDateField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data,
                FieldValueType = FieldType.Date
            };

            return Ok(await Service.UpdateFieldValue(singleDateField));
        }

        /// <summary>
        /// Update multi value date field from topic
        /// </summary>
        /// <remarks>Updates a specific multi value date field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/multidate/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateDateMultiField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] MultiDateField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            MultiDateField multiDateField = new MultiDateField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data,
                FieldValueType = FieldType.MultiDate
            };

            return Ok(await Service.UpdateFieldValue(multiDateField));
        }

        /// <summary>
        /// Update list field from topic
        /// </summary>
        /// <remarks>Updates a specific list field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/list/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateListSingleField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] SingleListField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            SingleListField singleListField = new SingleListField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field.Data,
                FieldValueType = FieldType.List
            };

            return Ok(await Service.UpdateFieldValue(singleListField));
        }

        /// <summary>
        /// Update relationship field from topic
        /// </summary>
        /// <remarks>Updates a specific single value relationship field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/relationship/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateRelationshipSingleField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] ApiModels.RelationshipField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            IField singleRelationshipField = _fieldMapper.MapField(field, topicId, fieldSetId, fieldId);

            return Ok(await Service.UpdateFieldValue(singleRelationshipField));
        }

        /// <summary>
        /// Update multi value relationship field from topic
        /// </summary>
        /// <remarks>Updates a specific multi value relationship field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/multirelationship/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateRelationshipMultiField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] ApiModels.MultiRelationshipField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            IField multiRelationshipField = _fieldMapper.MapField(field, topicId, fieldSetId, fieldId);

            return Ok(await Service.UpdateFieldValue(multiRelationshipField));
        }

        /// <summary>
        /// Update relationshiplist field from topic
        /// </summary>
        /// <remarks>Updates a specific single value relationshiplist field by field and fieldset identifier for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The field set definition identifier.</param>
        /// <param name="fieldId">The field definition identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fieldsets/{fieldSetId}/relationshiplist/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateRelationshipListField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] ApiModels.RelationshipListField field)
        {
            if (field == null) throw new BadRequestException("Invalid field request body");

            ValidateInput(topicId, fieldSetId, fieldId);

            IField singleRelationshipListField = _fieldMapper.MapField(field, topicId, fieldSetId, fieldId);

            return Ok(await Service.UpdateFieldValue(singleRelationshipListField));
        }

        /// <summary>
        /// Updates the fields from topic
        /// </summary>
        /// <remarks>Updates multiple types of fields for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fields">The fields.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [Route("fields")]
        public async Task<ActionResult<List<IField>>> UpdateFields(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [FromBody] ApiModels.Fields fields)
        {
            if (fields == null) throw new BadRequestException("Invalid fields request body");
            ValidateInput(topicId);

            List<IField> mappedFields = await _fieldMapper.MapFields(topicId, fields);

            return Ok(await Service.UpdateFieldValues(mappedFields));
        }

        /// <summary>
        /// Updates a singlevalue hyperlink field
        /// </summary>
        /// <remarks>Updates a single value hyperlink field for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The fieldset identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [FeatureGate(HyperlinkFeature.Hyperlink)]
        [Route("fieldsets/{fieldSetId}/hyperlink/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateSingleHyperlinkField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] ApiModels.PatchSingleHyperlinkField field)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ValidateInput(topicId, fieldSetId, fieldId);

            SingleHyperlinkField singleHyperlinkField = new SingleHyperlinkField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = Map(field?.Data),
                FieldValueType = FieldType.Hyperlink
            };

            return Ok(await Service.UpdateFieldValue(singleHyperlinkField));
        }

        /// <summary>
        /// Updates a multivalue hyperlink field
        /// </summary>
        /// <remarks>Updates a multi value hyperlink field for a specific topic.</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="fieldSetId">The fieldset identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <param name="field">The field.</param>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IField), (int)HttpStatusCode.OK)]
        [HttpPatch]
        [FeatureGate(HyperlinkFeature.Hyperlink)]
        [Route("fieldsets/{fieldSetId}/multihyperlink/{fieldId}")]
        public async Task<ActionResult<IField>> UpdateMultiHyperlinkField(
            Guid dbId,
            DataLanguages dataLanguage,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic identifier is not valid")] string topicId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "FieldSet identifier is not valid")] string fieldSetId,
            [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Field identifier is not valid")] string fieldId,
            [FromBody] ApiModels.PatchMultiHyperlinkField field)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ValidateInput(topicId, fieldSetId, fieldId);

            MultiHyperlinkField multiHyperlinkField = new MultiHyperlinkField
            {
                FieldId = fieldId,
                FieldSetId = fieldSetId,
                TopicId = topicId,
                Data = field?.Data?.Select(Map),
                FieldValueType = FieldType.MultiHyperlink
            };

            return Ok(await Service.UpdateFieldValue(multiHyperlinkField));
        }

        private static void ValidateInput(string topicId)
        {
            if (!DcvUtils.IsValid(topicId)) throw new BadRequestException("URL contains invalid topic id");
        }
        private static void ValidateInput(string topicId, string fieldSetId)
        {
            ValidateInput(topicId);
            if (!DcvUtils.IsValid(fieldSetId)) throw new BadRequestException("URL contains invalid fieldset id");
        }
        private static void ValidateInput(string topicId, string fieldSetId, string fieldId)
        {
            ValidateInput(topicId, fieldSetId);
            if (!DcvUtils.IsValid(fieldId)) throw new BadRequestException("URL contains invalid field id");
        }
        private static Uri Map(string uri)
        {
            Uri.TryCreate(uri, UriKind.Absolute, out Uri result);
            return result;
        }
    }
}
