using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;
using System.Linq;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class ListFieldMapper : GenericListFieldMapper<Model.ISingleValueSimpleField, ISingleListField>
    {
        /// <summary>
        /// Gets the generic mapped list service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override ISingleListField GetGenericMappedServiceField(Model.ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new SingleListField
            {
                FieldSetId = field.FieldSetDefinition.ID,
                FieldId = field.FieldDefinition.ID,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                SetName = field.FieldSetDefinition.Name,
                FieldName = field.FieldDefinition.Name,
                FieldValueType = FieldType.List,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly,
                Usage = field.FieldDefinition.Usage.ToString(),
                FieldValue = Map(field.FieldValue),
                Options = Map(field.FieldDefinition)
            };
        }

        /// <summary>
        /// Gets the generic mapped list repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">simpleField</exception>
        /// <exception cref="ArgumentException">simpleField</exception>
        protected override object[] GetGenericMappedRepoField(ISingleListField field, Model.ISimpleField simpleField = null)
        {
            if (field == null) return new object[] { null };
            if (simpleField == null) throw new ArgumentNullException(nameof(simpleField));
            if (!(simpleField is Model.ISingleValueSimpleField singleSimpleField)) throw new ArgumentException(nameof(simpleField));

            Model.IFieldListDefinition fieldListDefinition = (Model.IFieldListDefinition)singleSimpleField.FieldDefinition;
            Model.IFieldDefinitionListItem[] selectedItem = fieldListDefinition.ListItems.Where(item => item.ID.Equals(field.FieldValue?.Keys.FirstOrDefault())).ToArray();

            return selectedItem.Cast<object>().ToArray();
        }
    }
}