namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface ISingleField<T> : IField
    {
        /// <summary>
        /// Single object of data
        /// </summary>
        T Data { get; set; }
    }
}
