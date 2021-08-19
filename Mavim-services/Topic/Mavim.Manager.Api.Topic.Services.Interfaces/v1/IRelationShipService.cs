using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1
{
    public interface IRelationshipService
    {
        /// <summary>
        /// Saves the relationship.
        /// </summary>
        /// <param name="relationship">The relation.</param>
        /// <returns></returns>
        Task<IRelationship> SaveRelationship(ISaveRelationship patchRelationship);

        /// <summary>
        /// Gets the relationships of topic by DCV identifier.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<IRelationship>> GetRelationships(string dcvId);

        /// <summary>
        /// Deletes the relationship.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="relationshipId">The relation identifier.</param>
        Task DeleteRelationship(string dcvId, string relationshipId);
    }
}
