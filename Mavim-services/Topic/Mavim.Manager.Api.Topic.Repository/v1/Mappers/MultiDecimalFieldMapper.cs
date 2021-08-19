using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;
using System.Linq;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class MultiDecimalFieldMapper : GenericDecimalFieldMapper<Model.IMultiValueSimpleField, IMultiDecimalField>
    {
        /// <summary>
        /// Gets the generic mapped multi decimal service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override IMultiDecimalField GetGenericMappedServiceField(Model.IMultiValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new MultiDecimalField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValues = field.FieldValues?.Select(i => i?.ToString()),
                FieldValueType = FieldType.MultiDecimal,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped multi decimal repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        protected override object[] GetGenericMappedRepoField(IMultiDecimalField field, Model.ISimpleField simpleField = null)
        {
            if (field == null) return new object[] { null };

            return field.FieldValues == null ? new object[0] : field.FieldValues.ToArray<object>();
        }
    }
}
