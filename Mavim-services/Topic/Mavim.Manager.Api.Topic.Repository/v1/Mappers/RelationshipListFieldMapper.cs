using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;
using System.Linq;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class RelationshipListFieldMapper : GenericRelationshipListFieldMapper<Model.ISingleValueSimpleField, ISingleRelationshipListField>
    {
        /// <summary>
        /// Gets the generic mapped relationship list service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override ISingleRelationshipListField GetGenericMappedServiceField(Model.ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            Model.IElement topic = (Model.IElement)field.FieldValue;
            return new SingleRelationshipListField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition.Name,
                FieldValue = Map(topic),
                FieldValueType = FieldType.RelationshipList,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly,
                Options = Map(field.FieldDefinition),
            };
        }

        /// <summary>
        /// Gets the generic mapped relationship list repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">simpleField</exception>
        /// <exception cref="ArgumentException">simpleField</exception>
        protected override object[] GetGenericMappedRepoField(ISingleRelationshipListField field, Model.ISimpleField simpleField = null)
        {
            if (field?.FieldValue == null || !field.FieldValue.Any()) return new object[] { null };
            if (simpleField == null) throw new ArgumentNullException(nameof(simpleField));
            if (!(simpleField is Model.ISingleValueSimpleField singleSimpleField)) throw new ArgumentException(nameof(simpleField));

            Model.IFieldRelationListDefinition fieldListDefinition = (Model.IFieldRelationListDefinition)singleSimpleField.FieldDefinition;
            Model.IElement[] selectedItem = fieldListDefinition.ListParent.Children.Where(item => item.DcvID.ToString().Equals(field.FieldValue.Keys.FirstOrDefault())).ToArray();

            return selectedItem.Cast<object>().ToArray();
        }
    }
}
