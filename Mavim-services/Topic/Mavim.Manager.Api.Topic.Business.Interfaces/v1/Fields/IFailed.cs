namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields
{
    public interface IFailed<T>
    {
        T Item { get; set; }
        string Reason { get; set; }
    }
}