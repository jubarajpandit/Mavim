using Mavim.Manager.WopiFileLock.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiFileLock.Services.Interfaces
{
    public interface IWopiFileLockService
    {
        Task<IWopiFileLockState> GetFileLock(Guid dbId, string id, string userObjectId);

        Task<Dictionary<string, string>> ProcessFileOperationWopiRequest(string id, Guid dbId, string userToken,
            Dictionary<string, string> requestHeaders, string version);

        Task<Dictionary<string, string>> ProcessPutFileWopiRequest(string id, Guid dbId, string userObjectId,
            Dictionary<string, string> requestHeaders, long currentFileSize);
    }
}
