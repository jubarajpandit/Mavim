using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract;
using System;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers
{
    internal static class FieldMapperFactory
    {
        public static FieldMapperBase GetFieldMapper(Repository.Interfaces.v1.enums.FieldType fieldType)
        {
            return fieldType switch
            {
                Repository.Interfaces.v1.enums.FieldType.Unknown => throw new NotImplementedException(),
                Repository.Interfaces.v1.enums.FieldType.Text => new TextFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.MultiText => new MultiTextFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.Number => new NumberFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.MultiNumber => new MultiNumberFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.Decimal => new DecimalFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.MultiDecimal => new MultiDecimalFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.Boolean => new BooleanFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.Date => new DateFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.MultiDate => new MultiDateFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.List => new ListFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.Relationship => new RelationshipFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.MultiRelationship => new MultiRelationshipFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.RelationshipList => new RelationshipListFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.Hyperlink => new HyperlinkFieldMapper(),
                Repository.Interfaces.v1.enums.FieldType.MultiHyperlink => new MultiHyperlinkFieldMapper(),
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