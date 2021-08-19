using Mavim.Manager.Api.Topic.Services.v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using IService = Mavim.Manager.Api.Topic.Services.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract
{
    internal abstract class GenericFieldMapper<TBusiness, TService> : FieldMapperBase where TBusiness : IBusiness.Fields.IField where TService : IService.Fields.IField
    {
        internal override IService.Fields.IField GetMappedServiceField(IBusiness.Fields.IField field)
        {
            if (!(field is TBusiness fieldBusiness)) throw new Exception($"Field {nameof(field)} is not of correct type {nameof(TBusiness)}.");

            return GetGenericMappedServiceField(fieldBusiness);
        }

        internal override IBusiness.Fields.IField GetMappedBusinessField(IService.Fields.IField field)
        {
            if (!(field is TService fieldService)) throw new Exception($"Field {nameof(field)} is not of correct type {nameof(TBusiness)}.");

            return GetGenericMappedBusinessField(fieldService);
        }

        protected abstract TService GetGenericMappedServiceField(TBusiness field);

        protected abstract TBusiness GetGenericMappedBusinessField(TService field);

        protected Dictionary<string, IService.IRelationshipElement> Map(Dictionary<string, IBusiness.IRelationshipElement> fieldValue)
        {
            Dictionary<string, IService.IRelationshipElement> dictionary = fieldValue?.ToDictionary<KeyValuePair<string, IBusiness.IRelationshipElement>, string, IService.IRelationshipElement>(
                pair => pair.Key,
                pair => new RelationshipElement
                {
                    Dcv = pair.Value?.Dcv,
                    Icon = pair.Value?.Icon,
                    Name = pair.Value?.Name
                });

            return dictionary;
        }

        protected Dictionary<string, IBusiness.IRelationshipElement> Map(Dictionary<string, IService.IRelationshipElement> relationshipTopic)
        {
            Dictionary<string, IBusiness.IRelationshipElement> dictionary = relationshipTopic?.ToDictionary<KeyValuePair<string, IService.IRelationshipElement>, string, IBusiness.IRelationshipElement>(
                pair => pair.Key,
                pair => new Business.v1.Models.RelationshipElement
                {
                    Dcv = pair.Value?.Dcv,
                    Icon = pair.Value?.Icon,
                    Name = pair.Value?.Name
                });

            return dictionary;
        }
    }
}
