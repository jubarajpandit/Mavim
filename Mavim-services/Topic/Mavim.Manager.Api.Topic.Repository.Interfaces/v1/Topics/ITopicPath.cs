using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics
{
    public interface ITopicPath
    {
        List<IPathItem> Path { get; set; }
        List<ITopic> Data { get; set; }
    }
}
