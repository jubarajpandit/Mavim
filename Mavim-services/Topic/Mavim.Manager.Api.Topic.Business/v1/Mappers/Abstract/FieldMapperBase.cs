using Mavim.Manager.Api.Topic.Business.v1.Models;
using System;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;


namespace Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract
{
    internal abstract class FieldMapperBase
    {
        internal abstract IBusiness.Fields.IField GetMappedBusinessField(IRepo.Fields.IField field);

        internal abstract IRepo.Fields.IField GetMappedRepoField(IBusiness.Fields.IField field);

        protected IBusiness.enums.FieldType Map(IRepo.enums.FieldType type)
        {
            return Enum.TryParse(type.ToString(), out IBusiness.enums.FieldType result) ? result : IBusiness.enums.FieldType.Unknown;
        }

        protected IRepo.enums.FieldType Map(IBusiness.enums.FieldType type)
        {
            return Enum.TryParse(type.ToString(), out IRepo.enums.FieldType result) ? result : IRepo.enums.FieldType.Unknown;
        }

        protected IBusiness.IRelationshipElement Map(IRepo.RelationShips.IRelationshipElement relationshipTopic)
        {
            return relationshipTopic == null ? null : new RelationshipElement
            {
                Dcv = relationshipTopic.Dcv,
                Name = relationshipTopic.Name,
                Icon = relationshipTopic.Icon
            };
        }

        protected IRepo.RelationShips.IRelationshipElement Map(IBusiness.IRelationshipElement relationshipTopic)
        {
            return relationshipTopic == null ? null : new Repository.v1.Models.RelationshipElement
            {
                Dcv = relationshipTopic.Dcv,
                Name = relationshipTopic.Name,
                Icon = relationshipTopic.Icon
            };
        }

    }
}