using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips
{
    public interface IRelationshipsRepository
    {
        Task<IEnumerable<IRelationship>> GetRelationships(string dcvId);
        Task<IRelationship> CreateRelationship(string fromTopicDcv, string toTopicDcv, RelationshipType relationshipType);
        Task<bool> DeleteRelationship(string dcvId, string relationshipId);
    }
}
