using Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.WopiHost.Repository.Models
{
    public class CheckFileInfo : ICheckFileInfo
    {
        public string ComputedFileHash { get; set; }
        public long FileSize { get; set; }
    }
}
