namespace Mavim.Manager.Api.WopiFileLock.Repository.Interfaces.v1
{
    public interface IWopiFileLockResult
    {
        WopiFileLockStatus FileLockStatus { get; set; }

        string LockValue { get; set; }
    }
}