using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.v1.Models
{
    public class TopicPath : ITopicPath
    {
        public List<IPathItem> Path { get; set; }
        public List<ITopic> Data { get; set; }
    }
}
