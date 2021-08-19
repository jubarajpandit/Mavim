using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class Failed<T> : IFailed<T>
    {
        public T Item { get; set; }
        public string Reason { get; set; }
    }
}