using Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields;
using System.Linq;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers
{
    internal class MultiRelationshipFieldMapper : GenericRelationshipFieldMapper<IRepo.IMultiRelationshipField, IBusiness.IMultiRelationshipField>
    {
        protected override IBusiness.IMultiRelationshipField GetGenericMappedBusinessField(IRepo.IMultiRelationshipField field)
        {
            return new MultiRelationshipField
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
                Data = field.FieldValues?.Select(Map)
            };
        }

        protected override IRepo.IMultiRelationshipField GetGenericMappedRepoField(IBusiness.IMultiRelationshipField field)
        {
            return new Repository.v1.Fields.MultiRelationshipField
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
                FieldValues = field.Data?.Select(Map)
            };
        }
    }
}
