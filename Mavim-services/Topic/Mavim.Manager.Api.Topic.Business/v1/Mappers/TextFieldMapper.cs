using Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers
{
    internal class TextFieldMapper : GenericTextFieldMapper<IRepo.ISingleTextField, IBusiness.ISingleTextField>
    {
        protected override IBusiness.ISingleTextField GetGenericMappedBusinessField(IRepo.ISingleTextField field)
        {
            return new SingleTextField
            {
                FieldSetId = field.FieldSetId,
                FieldId = field.FieldId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                FieldName = field.FieldName,
                SetName = field.SetName,
                Data = field.FieldValue,
                FieldValueType = Map(field.FieldValueType),
                Characteristic = Map(field.Characteristic),
                OpenLocation = field.OpenLocation,
                RelationshipCategory = Map(field.RelationshipCategory),
                Usage = field.Usage,
                Required = field.Required,
                Readonly = field.Readonly
            };
        }

        protected override IRepo.ISingleTextField GetGenericMappedRepoField(IBusiness.ISingleTextField field)
        {
            return new Repository.v1.Fields.SingleTextField
            {
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                FieldValue = field.Data,
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
