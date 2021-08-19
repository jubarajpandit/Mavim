using Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IService = Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Services.v1.Mappers
{
    internal class RelationshipListFieldMapper : GenericRelationshipListFieldMapper<IBusiness.ISingleRelationshipListField, IService.ISingleRelationshipListField>
    {
        protected override IService.ISingleRelationshipListField GetGenericMappedServiceField(IBusiness.ISingleRelationshipListField field)
        {
            return new SingleRelationshipListField
            {
                FieldId = field.FieldId,
                FieldName = field.FieldName,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                TopicId = field.TopicId,
                SetName = field.SetName,
                Data = Map(field.Data),
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

        protected override IBusiness.ISingleRelationshipListField GetGenericMappedBusinessField(IService.ISingleRelationshipListField field)
        {
            return new Business.v1.Models.Fields.SingleRelationshipListField
            {
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                TopicId = field.TopicId,
                Data = Map(field.Data),
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
