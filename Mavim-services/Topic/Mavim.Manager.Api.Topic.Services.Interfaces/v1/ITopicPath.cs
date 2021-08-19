using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1
{
    public interface ITopicPath
    {
        List<IPathItem> Path { get; set; }
        List<ITopic> Data { get; set; }
    }
}
