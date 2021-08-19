using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;

namespace Mavim.Manager.Api.Topic.Repository.v1.Models
{
    class SimpleDispatchInstruction : ISimpleDispatchInstruction
    {
        public string TypeName { get; set; }
        public string Dcv { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsPublic { get; set; }
    }
}
