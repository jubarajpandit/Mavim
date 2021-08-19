using Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IService = Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Services.v1.Mappers
{
    internal class HyperlinkFieldMapper : GenericHyperlinkFieldMapper<IBusiness.ISingleHyperlinkField, IService.ISingleHyperlinkField>
    {
        protected override IService.ISingleHyperlinkField GetGenericMappedServiceField(IBusiness.ISingleHyperlinkField field)
        {
            return new SingleHyperlinkField
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

        protected override IBusiness.ISingleHyperlinkField GetGenericMappedBusinessField(IService.ISingleHyperlinkField field)
        {
            return new Business.v1.Models.Fields.SingleHyperlinkField
            {
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                TopicId = field.TopicId,
                Data = field.Data,
                FieldValueType = Map(field.FieldValueType),
                FieldName = field.FieldName,
                Characteristic = Map(field.Characteristic),
                Required = field.Required,
                Usage = field.Usage,
                Readonly = field.Readonly,
                OpenLocation = field.OpenLocation,
                RelationshipCategory = Map(field.RelationshipCategory),
                SetName = field.SetName
            };
        }
    }
}
