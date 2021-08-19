using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.v1.Models;
using System.Collections.Generic;
using System.Linq;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract
{
    internal abstract class GenericRelationshipFieldMapper<TRepo, TBusiness> : GenericFieldMapper<TRepo, TBusiness> where TRepo : Repository.Interfaces.v1.Fields.IField where TBusiness : Interfaces.v1.Fields.IField
    {
        protected IEnumerable<Dictionary<string, IRelationshipElement>> Map(IEnumerable<Dictionary<string, IRepo.RelationShips.IRelationshipElement>> fieldValues)
        {
            return fieldValues.SelectMany(item => item.Select(pair =>
              new Dictionary<string, IRelationshipElement> {
                    {
                        pair.Key, new RelationshipElement { Dcv = pair.Value.Dcv, Icon = pair.Value.Icon, Name = pair.Value.Name }
                    }
              }
           ));
        }

        protected IEnumerable<Dictionary<string, IRepo.RelationShips.IRelationshipElement>> Map(IEnumerable<Dictionary<string, IRelationshipElement>> fieldValues)
        {
            return fieldValues.SelectMany(item => item.Select(pair =>
              new Dictionary<string, IRepo.RelationShips.IRelationshipElement> {
                    {
                        pair.Key, new Repository.v1.Models.RelationshipElement { Dcv = pair.Value.Dcv, Icon = pair.Value.Icon, Name = pair.Value.Name }
                    }
              }
           ));
        }
    }
}