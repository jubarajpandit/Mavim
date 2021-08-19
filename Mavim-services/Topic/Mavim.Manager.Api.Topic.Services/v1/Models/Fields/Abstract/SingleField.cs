
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Services.v1.Models.Fields.Abstract
{
    public abstract class SingleField<T> : Field, ISingleField<T>
    {
        public T Data { get; set; }
    }
}
