using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;

namespace Mavim.Manager.Api.Topic.Repository.v1.Models
{
    public class PathItem : IPathItem
    {
        public int Order { get; set; }
        public string DcvId { get; set; }
    }
}
