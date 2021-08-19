namespace Mavim.Manager.Api.WopiFileLock.Repository.Interfaces.v1
{
    public class WopiFileLockResult : IWopiFileLockResult
    {
        public WopiFileLockStatus FileLockStatus { get; set; }
        public string LockValue { get; set; }
    }
}