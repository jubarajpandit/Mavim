using Mavim.Manager.WopiFileLock.Model;
using System;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiFileLock.Repository.Interfaces.v1
{
    public interface IWopiFileLockRepository
    {
        Task<IWopiFileLockResult> GetFileLockStatus(Guid dbId, string id, string xWopiOldLockValue);
        Task<IWopiFileLockResult> AddFileLock(Guid dbId, string id, string userObjectId, string xWopiLockValue, string version);
        Task<IWopiFileLockResult> UnlockFile(Guid dbId, string id, string xWopiLockValue);
        Task<IWopiFileLockResult> UnlockAndRelock(Guid dbId, string id, string xWopiLockValue, string xWopiOldLockValue);
        Task<IWopiFileLockResult> RefreshLockFile(Guid dbId, string id, string xWopiLockValue, string xWopiOldLockValue);
        Task<IWopiFileLockState> GetFileLock(Guid dbId, string id);
    }
}