using Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers
{
    internal class RelationshipListFieldMapper : GenericRelationshipListFieldMapper<IRepo.ISingleRelationshipListField, IBusiness.ISingleRelationshipListField>
    {
        protected override IBusiness.ISingleRelationshipListField GetGenericMappedBusinessField(IRepo.ISingleRelationshipListField field)
        {
            return new SingleRelationshipListField
            {
                FieldId = field.FieldId,
                FieldName = field.FieldName,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                SetName = field.SetName,
                Data = Map(field.FieldValue),
                FieldValueType = Map(field.FieldValueType),
                Characteristic = Map(field.Characteristic),
                OpenLocation = field.OpenLocation,
                RelationshipCategory = Map(field.RelationshipCategory),
                Usage = field.Usage,
                Required = field.Required,
                Readonly = field.Readonly,
                Options = Map(field.Options)
            };
        }

        protected override IRepo.ISingleRelationshipListField GetGenericMappedRepoField(IBusiness.ISingleRelationshipListField field)
        {
            return new Repository.v1.Fields.SingleRelationshipListField
            {
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                FieldValue = Map(field.Data),
                FieldValueType = Map(field.FieldValueType),
                FieldName = field.FieldName,
                Characteristic = Map(field.Characteristic),
                Required = field.Required,
                Usage = field.Usage,
                Readonly = field.Readonly,
                OpenLocation = field.OpenLocation,
                RelationshipCategory = Map(field.RelationshipCategory),
                SetName = field.SetName,
                Options = Map(field.Options)
            };
        }
    }
}
