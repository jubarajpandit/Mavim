using Mavim.Libraries.Features.Enums;
using Mavim.Libraries.Globalization.Culture;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.ChangelogField.Models;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogField.Services.v1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogField.Controllers.v1
{
    [Authorize]
    [Route("v1/{databaseId}/{dataLanguage}/changelog/fields")]
    [FeatureGate(ChangelogFeature.Changelog)]
    public class FieldsController : ControllerBase
    {
        #region Private Members
        private readonly IFieldService _fieldService;
        private readonly CultureInfo _cultureInfo;
        private readonly JsonSerializerSettings _jsonCultureSettings;
        private readonly DataLanguageType _dataLanguage;
        #endregion

        public FieldsController(IFieldService fieldService, IDataLanguage dataLanguage)
        {
            _fieldService = fieldService ?? throw new ArgumentNullException(nameof(fieldService));
            _cultureInfo = CultureInfo.CreateSpecificCulture(CultureInfoConstant.Dutch);
            _jsonCultureSettings = new JsonSerializerSettings() { Culture = _cultureInfo };
            _dataLanguage = Map(dataLanguage.Type);
        }

        /// <summary>
        /// Gets fields by databaseId, dataLanguage and topicId
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns>List of fields</returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpGet]
        [Route("topics/{topicId}")]
        public async Task<ActionResult<IEnumerable<IChangelogField>>> GetFields(GetByTopicRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _fieldService.GetFields(routeParams.DatabaseId, _dataLanguage, routeParams.TopicId));
        }

        /// <summary>
        /// Gets fields with pending status by databaseId, dataLanguage and topicId
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns>List of fields with pending status</returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpGet]
        [Route("topics/{topicId}/pending")]
        public async Task<ActionResult<IEnumerable<IChangelogField>>> GetPendingFieldsByTopic(GetByTopicRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _fieldService.GetPendingFieldsByTopic(routeParams.DatabaseId, _dataLanguage, routeParams.TopicId));
        }

        /// <summary>
        /// Gets status of a topic by checking the changed fields connnected to this topic by databaseId, dataLanguage and topicId
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns>a ChangeStatus enum value</returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpGet]
        [Route("topics/{topicId}/status")]
        public async Task<ActionResult<ChangeStatus>> GetTopicStatus(GetByTopicRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _fieldService.GetTopicStatusByFields(routeParams.DatabaseId, _dataLanguage, routeParams.TopicId));
        }

        /// <summary>
        /// Gets fields with pending status by databaseId and dataLanguage 
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns>List of fields with pending status</returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpGet]
        [Route("pending")]
        public async Task<ActionResult<IEnumerable<IChangelogField>>> GetPendingFields(BaseRouteParam routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _fieldService.GetPendingFields(routeParams.DatabaseId, _dataLanguage));
        }

        /// <summary>
        /// Sets the field status of the field by id to approved.
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns>Updated field</returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPatch]
        [Route("{changelogId}/approve")]
        public async Task<ActionResult<IChangelogField>> ApproveField(PatchRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _fieldService.ApproveField(routeParams.DatabaseId, _dataLanguage, routeParams.ChangelogId));
        }

        /// <summary>
        /// Sets the field status of the field by id to rejected.
        /// </summary>
        /// <param name="routeParams">The route parameters.</param>
        /// <returns>Updated field</returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPatch]
        [Route("{changelogId}/reject")]
        public async Task<ActionResult<IChangelogField>> RejectField(PatchRouteParams routeParams)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _fieldService.RejectField(routeParams.DatabaseId, _dataLanguage, routeParams.ChangelogId));
        }

        /// <summary>
        /// Saves the boolean field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("bool")]
        public async Task<ActionResult> SaveBooleanField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<bool> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange booleanField = GetSaveFieldChange(baseRouteParam, change, FieldType.Boolean);

            await _fieldService.SaveField(booleanField);
            return Ok();
        }

        /// <summary>
        /// Saves the text field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("text")]
        public async Task<ActionResult> SaveTextField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<string> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange textField = GetSaveFieldChange(baseRouteParam, change, FieldType.Text);

            await _fieldService.SaveField(textField);
            return Ok();
        }

        /// <summary>
        /// Saves the multi text field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("multitext")]
        public async Task<ActionResult> SaveMultiTextField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<List<string>> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange multiTextField = GetSaveFieldChange(baseRouteParam, change, FieldType.MultiText);

            await _fieldService.SaveField(multiTextField);
            return Ok();
        }

        /// <summary>
        /// Saves the number field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("number")]
        public async Task<ActionResult> SaveNumberField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<int> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange numberField = GetSaveFieldChange(baseRouteParam, change, FieldType.Number);

            await _fieldService.SaveField(numberField);
            return Ok();
        }

        /// <summary>
        /// Saves the multi number field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("multinumber")]
        public async Task<ActionResult> SaveMultiNumberField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<List<int>> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange multiNumberField = GetSaveFieldChange(baseRouteParam, change, FieldType.MultiNumber);

            await _fieldService.SaveField(multiNumberField);
            return Ok();
        }

        /// <summary>
        /// Saves the decimal field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("decimal")]
        public async Task<ActionResult> SaveDecimalField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<decimal> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange decimalField = GetSaveFieldChange(baseRouteParam, change, FieldType.Decimal);

            await _fieldService.SaveField(decimalField);
            return Ok();
        }

        /// <summary>
        /// Saves the multi decimal field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("multidecimal")]
        public async Task<ActionResult> SaveMultiDecimalField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<List<decimal>> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange multiDecimalField = GetSaveFieldChange(baseRouteParam, change, FieldType.MultiDecimal);

            await _fieldService.SaveField(multiDecimalField);
            return Ok();
        }

        /// <summary>
        /// Saves the date field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("date")]
        public async Task<ActionResult> SaveDateField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<DateTime?> change)
        {
            if (!ModelState.IsValid || change == null) return BadRequest(ModelState);

            ISaveFieldChange dateField = GetSaveFieldChange(baseRouteParam, change, FieldType.Date);

            await _fieldService.SaveField(dateField);
            return Ok();
        }

        /// <summary>
        /// Saves the multi date field.
        /// </summary>
        /// <param name="baseRouteParam">The save (post) route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request route or body</exception>
        [HttpPost]
        [Route("multidate")]
        public async Task<ActionResult> SaveMultiDateField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<List<DateTime?>> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange multiDateField = GetSaveFieldChange(baseRouteParam, change, FieldType.MultiDate);

            await _fieldService.SaveField(multiDateField);
            return Ok();
        }

        /// <summary>
        /// Saves the list field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("list")]
        public async Task<ActionResult> SaveListField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<Dictionary<string, string>> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange listField = GetSaveFieldChange(baseRouteParam, change, FieldType.List);

            await _fieldService.SaveField(listField);
            return Ok();
        }

        /// <summary>
        /// Saves the relationship field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("relationship")]
        public async Task<ActionResult> SaveRelationshipField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<RelationshipElement> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange relationshipField = GetSaveFieldChange(baseRouteParam, change, FieldType.Relationship);

            await _fieldService.SaveField(relationshipField);
            return Ok();
        }

        /// <summary>
        /// Saves the multi relationship field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("multirelationship")]
        public async Task<ActionResult> SaveMultiRelationshipField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<List<RelationshipElement>> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange multiRelationshipField = GetSaveFieldChange(baseRouteParam, change, FieldType.MultiRelationship);

            await _fieldService.SaveField(multiRelationshipField);
            return Ok();
        }


        /// <summary>
        /// Saves the relationship list field.
        /// </summary>
        /// <param name="baseRouteParam">The route parameters.</param>
        /// <param name="change">The field changes.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Invalid field request body</exception>
        [HttpPost]
        [Route("relationshiplist")]
        public async Task<ActionResult> SaveRelationshipListField(BaseRouteParam baseRouteParam, [FromBody] FieldChange<Dictionary<string, RelationshipElement>> change)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ISaveFieldChange relationshipListField = GetSaveFieldChange(baseRouteParam, change, FieldType.RelationshipList);

            await _fieldService.SaveField(relationshipListField);
            return Ok();
        }

        private ISaveFieldChange GetSaveFieldChange<T>(BaseRouteParam baseRouteParam, FieldChange<T> change, FieldType type) =>
            new SaveFieldChange
            {
                DatabaseId = baseRouteParam.DatabaseId,
                DataLanguage = _dataLanguage,
                TimestampChanged = DateTime.Now,
                TopicId = change.TopicId,
                Status = ChangeStatus.Pending,
                FieldSetId = change.FieldSetId,
                FieldId = change.FieldId,
                Type = type,
                OldFieldValue = JsonConvert.SerializeObject(change.OldFieldValue, _jsonCultureSettings),
                NewFieldValue = JsonConvert.SerializeObject(change.NewFieldValue, _jsonCultureSettings)
            };

        private static DataLanguageType Map(Libraries.Middlewares.Language.Enums.DataLanguageType dataLanguage) =>
                dataLanguage switch
                {
                    Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch => DataLanguageType.Dutch,
                    Libraries.Middlewares.Language.Enums.DataLanguageType.English => DataLanguageType.English,
                    _ => throw new ArgumentException($"unsupported DataLanguage: {dataLanguage}")
                };
    }
}
