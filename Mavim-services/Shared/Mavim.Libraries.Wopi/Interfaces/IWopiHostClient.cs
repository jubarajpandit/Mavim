using System;
using System.Threading.Tasks;

namespace Mavim.Libraries.Wopi.Interfaces
{
    public interface IWopiHostClient
    {
        Task<IFileInfo> GetFileInfo(Guid dbId, string dcvId);
    }
}
