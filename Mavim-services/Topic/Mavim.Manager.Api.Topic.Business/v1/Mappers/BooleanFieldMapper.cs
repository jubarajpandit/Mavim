using Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Repo = Mavim.Manager.Api.Topic.Repository.v1.Fields;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers
{
    internal class BooleanFieldMapper : GenericBooleanFieldMapper<IRepo.ISingleBooleanField, IBusiness.ISingleBooleanField>
    {
        protected override IBusiness.ISingleBooleanField GetGenericMappedBusinessField(IRepo.ISingleBooleanField field)
        {
            return new SingleBooleanField
            {
                FieldId = field.FieldId,
                FieldName = field.FieldName,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
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

        protected override IRepo.ISingleBooleanField GetGenericMappedRepoField(IBusiness.ISingleBooleanField field)
        {
            return new Repo.SingleBooleanField
            {
                FieldId = field.FieldId,
                FieldSetId = field.FieldSetId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                FieldValue = field.Data,
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
