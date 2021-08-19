namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface IFailed<T>
    {
        T Item { get; set; }
        string Reason { get; set; }
    }
}