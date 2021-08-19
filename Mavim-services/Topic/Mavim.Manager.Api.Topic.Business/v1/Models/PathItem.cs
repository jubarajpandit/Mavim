using Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Business.v1.Models
{
    public class PathItem : IPathItem
    {
        public int Order { get; set; }
        public string DcvId { get; set; }
    }
}
