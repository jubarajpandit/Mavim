using Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers
{
    internal class MultiDateFieldMapper : GenericDateFieldMapper<IRepo.IMultiDateField, IBusiness.IMultiDateField>
    {
        protected override IBusiness.IMultiDateField GetGenericMappedBusinessField(IRepo.IMultiDateField field)
        {
            return new MultiDateField
            {
                FieldSetId = field.FieldSetId,
                FieldId = field.FieldId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                SetName = field.SetName,
                FieldName = field.FieldName,
                FieldValueType = Map(field.FieldValueType),
                Required = field.Required,
                Readonly = field.Readonly,
                Usage = field.Usage,
                RelationshipCategory = Map(field.RelationshipCategory),
                Characteristic = Map(field.Characteristic),
                OpenLocation = field.OpenLocation,
                Data = field.FieldValues
            };
        }

        protected override IRepo.IMultiDateField GetGenericMappedRepoField(IBusiness.IMultiDateField field)
        {
            return new Repository.v1.Fields.MultiDateField
            {
                FieldSetId = field.FieldSetId,
                FieldId = field.FieldId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                SetName = field.SetName,
                FieldName = field.FieldName,
                FieldValueType = Map(field.FieldValueType),
                Required = field.Required,
                Readonly = field.Readonly,
                Usage = field.Usage,
                RelationshipCategory = Map(field.RelationshipCategory),
                Characteristic = Map(field.Characteristic),
                OpenLocation = field.OpenLocation,
                FieldValues = field.Data,
            };
        }
    }
}
