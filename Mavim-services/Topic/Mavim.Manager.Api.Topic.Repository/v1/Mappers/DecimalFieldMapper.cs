using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class DecimalFieldMapper : GenericDecimalFieldMapper<Model.ISingleValueSimpleField, ISingleDecimalField>
    {
        /// <summary>
        /// Gets the generic mapped decimal service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override ISingleDecimalField GetGenericMappedServiceField(Model.ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new SingleDecimalField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValue = field.FieldValue?.ToString(),
                FieldValueType = FieldType.Decimal,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped decimal repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        protected override object[] GetGenericMappedRepoField(ISingleDecimalField field, Model.ISimpleField simpleField = null)
        {
            return new object[] { field?.FieldValue };
        }
    }
}
