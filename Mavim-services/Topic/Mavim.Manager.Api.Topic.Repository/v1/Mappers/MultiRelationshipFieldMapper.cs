using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using Mavim.Manager.Model;
using System;
using System.Linq;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class MultiRelationshipFieldMapper : GenericRelationshipFieldMapper<IMultiValueSimpleField, IMultiRelationshipField>
    {

        public MultiRelationshipFieldMapper(IMavimDbDataAccess dataAccess) : base(dataAccess) { }

        /// <summary>
        /// Gets the generic mapped multi relationship service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override IMultiRelationshipField GetGenericMappedServiceField(Model.IMultiValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new MultiRelationshipField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValues = field.FieldValues?.Cast<Model.IElement>().Select(GetRelationFieldValue),
                FieldValueType = FieldType.MultiRelationship,
                OpenLocation = ((IFieldRelationDefinition)field.FieldDefinition)?.BrowseLocation?.DcvID?.ToString(),
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped multi relationship repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        protected override object[] GetGenericMappedRepoField(IMultiRelationshipField field, Model.ISimpleField simpleField = null)
        {
            if (field == null) return new object[] { null };

            return field.FieldValues == null ? new object[] { null } : field.FieldValues.Select(f => GetElementByDcvId(f.Dcv)).ToArray<object>();
        }
    }
}
