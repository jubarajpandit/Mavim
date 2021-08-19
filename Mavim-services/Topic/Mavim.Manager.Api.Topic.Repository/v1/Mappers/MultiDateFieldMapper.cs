using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using System;
using System.Linq;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers
{
    internal class MultiDateFieldMapper : GenericDateFieldMapper<Model.IMultiValueSimpleField, IMultiDateField>
    {
        /// <summary>
        /// Gets the generic mapped multi date service field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected override IMultiDateField GetGenericMappedServiceField(Model.IMultiValueSimpleField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return new MultiDateField
            {
                FieldId = field.FieldDefinition.ID,
                FieldName = field.FieldDefinition.Name,
                SetOrder = field.FieldSetDefinition.OrderNumber,
                Order = field.OrderNumber,
                FieldSetId = field.FieldSetDefinition.ID,
                SetName = field.FieldSetDefinition?.Name ?? string.Empty,
                FieldValues = field.FieldValues?.Select(Map),
                FieldValueType = FieldType.MultiDate,
                Required = field.FieldDefinition.Required,
                Readonly = field.ReadOnly
            };
        }

        /// <summary>
        /// Gets the generic mapped multi date repo field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="simpleField">The simple field.</param>
        /// <returns></returns>
        protected override object[] GetGenericMappedRepoField(IMultiDateField field, Model.ISimpleField simpleField = null)
        {
            if (field == null) return new object[] { null };

            DateTime?[] multiDateField = field.FieldValues.ToArray();

            object[] returnDates = new object[multiDateField.Length];

            for (int i = 0; i < multiDateField.Length; i++)
            {
                if (multiDateField[i] != null)
                {
                    // DateTime is a value type so you can't assign a DateTime[] to an object[] variable.
                    // You'll have to explicitly create an object array and copy the values over.
                    DateTime?[] dateTimeArray = { ((DateTime)multiDateField[i]).ToUniversalTime() };
                    dateTimeArray.CopyTo(returnDates, i);
                }
                else
                {
                    returnDates[i] = null;
                }
            }

            return returnDates;
        }

    }
}
