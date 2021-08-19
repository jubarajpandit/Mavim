using Mavim.Manager.Api.Topic.Business.v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract
{
    internal abstract class GenericFieldMapper<TRepo, TBusiness> : FieldMapperBase where TRepo : IRepo.Fields.IField where TBusiness : IBusiness.Fields.IField
    {
        internal override IBusiness.Fields.IField GetMappedBusinessField(IRepo.Fields.IField field)
        {
            if (!(field is TRepo fieldRepo)) throw new ArgumentException($"Field {nameof(field)} is not of correct type {nameof(TRepo)}.");

            return GetGenericMappedBusinessField(fieldRepo);
        }

        internal override IRepo.Fields.IField GetMappedRepoField(IBusiness.Fields.IField field)
        {
            if (!(field is TBusiness fieldBusiness)) throw new ArgumentException($"Field {nameof(field)} is not of correct type {nameof(TRepo)}.");

            return GetGenericMappedRepoField(fieldBusiness);
        }

        protected abstract TBusiness GetGenericMappedBusinessField(TRepo field);

        protected abstract TRepo GetGenericMappedRepoField(TBusiness field);

        protected Dictionary<string, IBusiness.IRelationshipElement> Map(Dictionary<string, IRepo.RelationShips.IRelationshipElement> fieldValue)
        {
            Dictionary<string, IBusiness.IRelationshipElement> dictionary = fieldValue?.ToDictionary<KeyValuePair<string, IRepo.RelationShips.IRelationshipElement>, string, IBusiness.IRelationshipElement>(
                pair => pair.Key,
                pair => new RelationshipElement
                {
                    Dcv = pair.Value?.Dcv,
                    Icon = pair.Value?.Icon,
                    Name = pair.Value?.Name
                });

            return dictionary;
        }

        protected Dictionary<string, IRepo.RelationShips.IRelationshipElement> Map(Dictionary<string, IBusiness.IRelationshipElement> relationshipTopic)
        {
            Dictionary<string, IRepo.RelationShips.IRelationshipElement> dictionary = relationshipTopic?.ToDictionary<KeyValuePair<string, IBusiness.IRelationshipElement>, string, IRepo.RelationShips.IRelationshipElement>(
                pair => pair.Key,
                pair => new Repository.v1.Models.RelationshipElement
                {
                    Dcv = pair.Value?.Dcv,
                    Icon = pair.Value?.Icon,
                    Name = pair.Value?.Name
                });

            return dictionary;
        }
    }
}
