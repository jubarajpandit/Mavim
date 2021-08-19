using Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IService = Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Services.v1.Mappers
{
    internal class BooleanFieldMapper : GenericBooleanFieldMapper<IBusiness.ISingleBooleanField, IService.ISingleBooleanField>
    {
        protected override IService.ISingleBooleanField GetGenericMappedServiceField(IBusiness.ISingleBooleanField field)
        {
            return new SingleBooleanField
            {
                FieldId = field.FieldId,
                FieldName = field.FieldName,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                TopicId = field.TopicId,
                SetName = field.SetName,
                Data = field.Data,
                FieldValueType = Map(field.FieldValueType),
                Characteristic = Map(field.Characteristic),
                OpenLocation = field.OpenLocation,
                RelationshipCategory = Map(field.RelationshipCategory),
                Usage = field.Usage,
                Required = field.Required,
                Readonly = field.Readonly
            };
        }

        protected override IBusiness.ISingleBooleanField GetGenericMappedBusinessField(IService.ISingleBooleanField field)
        {
            return new Business.v1.Models.Fields.SingleBooleanField
            {
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                TopicId = field.TopicId,
                Data = field.Data,
                FieldValueType = Map(field.FieldValueType),
                Characteristic = Map(field.Characteristic),
                FieldName = field.FieldName,
                OpenLocation = field.OpenLocation,
                Readonly = field.Readonly,
                RelationshipCategory = Map(field.RelationshipCategory),
                Required = field.Required,
                SetName = field.SetName,
                Usage = field.Usage
            };
        }
    }
}
