namespace Mavim.Manager.Api.WopiHost.Models
{
    public class WopiRequest
    {
        public string Id { get; set; }
        public WopiRequestType RequestType { get; set; }
        public string AccessToken { get; set; }
    }
}
