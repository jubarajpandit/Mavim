using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class NumberFieldMapper : GenericNumberFieldMapper<Model.ISingleValueSimpleField, ISingleNumberField>
    {
        /// <summary>
        /// Gets the generic mapped number service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">field</exception>
        protected override ISingleNumberField GetGenericMappedServiceField(Model.ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new SingleNumberField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValue = field.FieldValue?.ToString(),
                FieldValueType = FieldType.Number,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped number repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">field</exception>
        protected override object[] GetGenericMappedRepoField(ISingleNumberField field, Model.ISimpleField simpleField = null)
        {
            return new object[] { field?.FieldValue };
        }
    }
}