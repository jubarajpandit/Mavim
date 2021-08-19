namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields
{
    public interface ISingleField<T> : IField
    {
        T FieldValue { get; set; }
    }
}
