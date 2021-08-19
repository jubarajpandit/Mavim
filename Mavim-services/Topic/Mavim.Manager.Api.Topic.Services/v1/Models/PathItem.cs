using Mavim.Manager.Api.Topic.Services.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class PathItem : IPathItem
    {
        public int Order { get; set; }
        public string DcvId { get; set; }
    }
}
