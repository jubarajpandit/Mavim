using Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Business.v1.Models
{
    public class Failed<T> : IFailed<T>
    {
        public T Item { get; set; }
        public string Reason { get; set; }
    }
}