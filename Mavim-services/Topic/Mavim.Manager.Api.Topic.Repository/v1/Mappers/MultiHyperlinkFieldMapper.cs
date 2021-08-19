using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class MultiHyperlinkFieldMapper : GenericHyperlinkFieldMapper<Model.IMultiValueSimpleField, IMultiHyperlinkField>
    {
        public MultiHyperlinkFieldMapper(ILoggerFactory loggerFactory) : base(loggerFactory) { }

        /// <summary>
        /// Gets the generic mapped multi hyperlink service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override IMultiHyperlinkField GetGenericMappedServiceField(Model.IMultiValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new MultiHyperlinkField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValues = field.FieldValues?.Select(i => Map(i)),
                FieldValueType = FieldType.MultiHyperlink,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped multi text repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">field</exception>
        protected override object[] GetGenericMappedRepoField(IMultiHyperlinkField field, Model.ISimpleField simpleField = null)
        {
            if (field == null) return new object[] { null };

            return field.FieldValues == null ? new object[] { null } : field.FieldValues.Select(i => i?.OriginalString).ToArray<object>();
        }
    }
}