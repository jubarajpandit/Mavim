using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Services.v1.Mappers
{
    internal static class FieldMapperFactory
    {
        public static FieldMapperBase GetFieldMapper(Business.Interfaces.v1.enums.FieldType fieldType)
        {
            return fieldType switch
            {
                Business.Interfaces.v1.enums.FieldType.Unknown => throw new NotImplementedException(),
                Business.Interfaces.v1.enums.FieldType.Text => new TextFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.MultiText => new MultiTextFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.Number => new NumberFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.MultiNumber => new MultiNumberFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.Decimal => new DecimalFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.MultiDecimal => new MultiDecimalFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.Boolean => new BooleanFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.Date => new DateFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.MultiDate => new MultiDateFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.List => new ListFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.Relationship => new RelationshipFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.MultiRelationship => new MultiRelationshipFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.RelationshipList => new RelationshipListFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.Hyperlink => new HyperlinkFieldMapper(),
                Business.Interfaces.v1.enums.FieldType.MultiHyperlink => new MultiHyperlinkFieldMapper(),
                _ => throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null)
            };
        }

        public static FieldMapperBase GetFieldMapper(FieldType fieldType)

        {
            return fieldType switch
            {
                FieldType.Unknown => throw new NotImplementedException(),
                FieldType.Text => new TextFieldMapper(),
                FieldType.MultiText => new MultiTextFieldMapper(),
                FieldType.Number => new NumberFieldMapper(),
                FieldType.MultiNumber => new MultiNumberFieldMapper(),
                FieldType.Decimal => new DecimalFieldMapper(),
                FieldType.MultiDecimal => new MultiDecimalFieldMapper(),
                FieldType.Boolean => new BooleanFieldMapper(),
                FieldType.Date => new DateFieldMapper(),
                FieldType.MultiDate => new MultiDateFieldMapper(),
                FieldType.List => new ListFieldMapper(),
                FieldType.Relationship => new RelationshipFieldMapper(),
                FieldType.MultiRelationship => new MultiRelationshipFieldMapper(),
                FieldType.RelationshipList => new RelationshipListFieldMapper(),
                FieldType.Hyperlink => new HyperlinkFieldMapper(),
                FieldType.MultiHyperlink => new MultiHyperlinkFieldMapper(),
                _ => throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null)
            };
        }
    }
}