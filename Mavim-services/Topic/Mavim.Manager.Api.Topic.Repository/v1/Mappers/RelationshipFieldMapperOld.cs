using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using Mavim.Manager.Model;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class RelationshipFieldMapperOld : GenericRelationshipFieldMapper<ISingleValueSimpleField, ISingleRelationshipField>
    {

        public RelationshipFieldMapperOld(IMavimDbDataAccess dataAccess) : base(dataAccess) { }

        /// <summary>
        /// Gets the generic mapped relationship service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override ISingleRelationshipField GetGenericMappedServiceField(ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            IElement topic = (IElement)field.FieldValue;
            return new SingleRelationshipField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition.Name,
                FieldValue = GetRelationFieldValue(topic),
                FieldValueType = FieldType.Relationship,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly,
            };
        }

        /// <summary>
        /// Gets the generic mapped relationship repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        protected override object[] GetGenericMappedRepoField(ISingleRelationshipField field, Model.ISimpleField simpleField = null)
        {
            if (field?.FieldValue == null) return new object[] { null };

            Model.IElement relation = GetElementByDcvId(field.FieldValue.Dcv);
            return new object[] { relation };
        }
    }
}
