using Mavim.Libraries.Changelog.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Libraries.Changelog.Interfaces
{
    public interface IChangelogTitleClient
    {
        Task<IEnumerable<IChangelogTitle>> GetTitles(Guid dbId, string dcvId);
        Task<IChangelogTitle> GetPendingTitle(Guid dbId, string dcvId);
        Task<IEnumerable<IChangelogTitle>> GetAllPendingTitles(Guid dbId);
        Task<ChangeStatus> GetTitleStatus(Guid dbId, string dcvId);
        Task<IChangelogTitle> ApproveTitle(Guid dbId, string dcvId);
        Task<IChangelogTitle> RejectTitle(Guid dbId, string dcvId);
    }
}
