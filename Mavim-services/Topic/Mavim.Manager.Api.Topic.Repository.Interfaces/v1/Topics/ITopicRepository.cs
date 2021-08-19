using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics
{
    public interface ITopicRepository
    {
        Task<ITopic> GetRootTopic();
        Task<ITopicPath> GetPathToRoot(string dcvId);
        Task<ITopic> GetTopicByDcv(string dcvId);
        Task<IReadOnlyList<ITopic>> GetTopicsByCode(string topicCode);
        Task<IEnumerable<ITopic>> GetChildrenByDcv(string dcvId);
        Task<IEnumerable<ITopic>> GetSiblingsByDcv(string dcvId);
        Task<IEnumerable<ITopic>> GetRelationshipCategories();
        Task<ITopic> UpdateTopicName(string dcvId, string name);
        Task<byte[]> GetTopicCustomIconByCustomIconId(string customIconId);
    }
}
