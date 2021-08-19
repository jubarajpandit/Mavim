namespace Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1
{
    public interface ICheckFileInfo
    {
        string ComputedFileHash { get; set; }

        long FileSize { get; set; }
    }
}
