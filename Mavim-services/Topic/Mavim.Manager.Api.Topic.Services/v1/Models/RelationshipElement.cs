using Mavim.Manager.Api.Topic.Services.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class RelationshipElement : IRelationshipElement
    {
        public string Dcv { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
