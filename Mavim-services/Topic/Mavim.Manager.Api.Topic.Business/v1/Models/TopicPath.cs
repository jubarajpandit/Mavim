using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Business.v1.Models
{
    public class TopicPath : ITopicPath
    {
        public List<IPathItem> Path { get; set; }
        public List<ITopic> Data { get; set; }
    }
}
