
using Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Business.v1.Models
{
    public class SimpleDispatchInstruction : ISimpleDispatchInstruction
    {
        public string TypeName { get; set; }
        public string Dcv { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
