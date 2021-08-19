using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using Microsoft.Extensions.Logging;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class HyperlinkFieldMapper : GenericHyperlinkFieldMapper<Model.ISingleValueSimpleField, ISingleHyperlinkField>
    {
        public HyperlinkFieldMapper(ILoggerFactory loggerFactory) : base(loggerFactory) { }

        /// <summary>
        /// Gets the generic mapped text service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override ISingleHyperlinkField GetGenericMappedServiceField(Model.ISingleValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new SingleHyperlinkField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValue = Map(field.FieldValue),
                FieldValueType = FieldType.Hyperlink,
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
        protected override object[] GetGenericMappedRepoField(ISingleHyperlinkField field, Model.ISimpleField simpleField = null)
        {
            return new object[] { field?.FieldValue?.OriginalString };
        }
    }
}