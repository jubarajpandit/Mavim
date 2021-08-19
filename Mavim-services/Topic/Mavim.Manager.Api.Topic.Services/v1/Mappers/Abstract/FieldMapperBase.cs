using Mavim.Manager.Api.Topic.Services.v1.Models;
using System;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using IService = Mavim.Manager.Api.Topic.Services.Interfaces.v1;


namespace Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract
{
    internal abstract class FieldMapperBase
    {
        internal abstract IService.Fields.IField GetMappedServiceField(IBusiness.Fields.IField field);

        internal abstract IBusiness.Fields.IField GetMappedBusinessField(IService.Fields.IField field);

        protected IService.enums.FieldType Map(IBusiness.enums.FieldType type)
        {
            return Enum.TryParse(type.ToString(), out IService.enums.FieldType result) ? result : IService.enums.FieldType.Unknown;
        }

        protected IBusiness.enums.FieldType Map(IService.enums.FieldType type)
        {
            return Enum.TryParse(type.ToString(), out IBusiness.enums.FieldType result) ? result : IBusiness.enums.FieldType.Unknown;
        }

        protected IService.IRelationshipElement Map(IBusiness.IRelationshipElement relationshipTopic)
        {
            return relationshipTopic == null ? null : new RelationshipElement
            {
                Dcv = relationshipTopic.Dcv,
                Name = relationshipTopic.Name,
                Icon = relationshipTopic.Icon
            };
        }

        protected IBusiness.IRelationshipElement Map(IService.IRelationshipElement relationshipTopic)
        {
            return relationshipTopic == null ? null : new Business.v1.Models.RelationshipElement
            {
                Dcv = relationshipTopic.Dcv,
                Name = relationshipTopic.Name,
                Icon = relationshipTopic.Icon
            };
        }

    }
}