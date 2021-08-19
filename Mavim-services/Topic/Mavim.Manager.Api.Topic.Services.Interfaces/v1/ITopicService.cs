using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1
{
    public interface ITopicService
    {
        /// <summary>
        /// Gets the root topic of the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <returns></returns>
        Task<ITopic> GetRootTopic(Guid dbId);

        Task<ITopicPath> GetPathToRoot(string dcvId);

        /// <summary>
        /// Gets the child topic based on the dcv from the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of topics</returns>
        Task<ITopic> GetTopic(Guid dbId, string dcvId);

        /// <summary>
        /// Gets topics by code from the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of topics</returns>
        Task<IReadOnlyList<ITopic>> GetTopicsByCode(string topicCode);

        /// <summary>
        /// Gets the children of mavim database topic based on the dcv from the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of topics</returns>
        Task<IEnumerable<ITopic>> GetChildren(string dcvId);

        /// <summary>
        /// Gets the siblings of mavim database topic based on the dcv from the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of topics</returns>
        Task<IEnumerable<ITopic>> GetSiblings(string dcvId);

        /// <summary>
        /// Gets the topics that reflect the different relationship categories.
        /// </summary>
        /// <returns>Collection of topics</returns>
        Task<IEnumerable<ITopic>> GetRelationshipCategories();

        /// <summary>
        /// Updates the complete topic.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <returns>Topic</returns>
        Task<ITopic> UpdateTopic(string dcvId, ISaveTopic topic);
    }
}
