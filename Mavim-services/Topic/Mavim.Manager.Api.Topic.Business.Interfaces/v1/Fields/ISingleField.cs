namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields
{
    public interface ISingleField<T> : IField
    {
        T Data { get; set; }
    }
}
