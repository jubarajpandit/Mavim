using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class TextFieldMapper : GenericTextFieldMapper<Model.ISingleValueSimpleField, ISingleTextField>
    {
        /// <summary>
        /// Gets the generic mapped text service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override ISingleTextField GetGenericMappedServiceField(Model.ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new SingleTextField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValue = field.FieldValue?.ToString(),
                FieldValueType = FieldType.Text,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped text repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">field</exception>
        protected override object[] GetGenericMappedRepoField(ISingleTextField field, Model.ISimpleField simpleField = null)
        {
            return new object[] { field?.FieldValue };
        }
    }
}