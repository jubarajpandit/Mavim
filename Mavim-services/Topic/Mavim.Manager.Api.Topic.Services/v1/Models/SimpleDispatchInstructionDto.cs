
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class SimpleDispatchInstruction : ISimpleDispatchInstruction
    {
        public string TypeName { get; set; }
        public string Dcv { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
