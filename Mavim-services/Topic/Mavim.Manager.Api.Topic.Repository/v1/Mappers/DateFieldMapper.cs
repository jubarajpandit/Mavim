using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class DateFieldMapper : GenericDateFieldMapper<Model.ISingleValueSimpleField, ISingleDateField>
    {
        /// <summary>
        /// Gets the generic mapped date service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override ISingleDateField GetGenericMappedServiceField(Model.ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new SingleDateField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValue = Map(field.FieldValue),
                FieldValueType = FieldType.Date,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped date repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">field</exception>
        protected override object[] GetGenericMappedRepoField(ISingleDateField field, Model.ISimpleField simpleField = null)
        {
            if (field == null) return null;

            return new object[] { field.FieldValue?.ToUniversalTime() };
        }
    }
}
