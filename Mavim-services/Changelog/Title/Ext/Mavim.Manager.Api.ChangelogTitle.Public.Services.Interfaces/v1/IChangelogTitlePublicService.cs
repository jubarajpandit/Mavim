using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1
{
    public interface IChangelogTitlePublicService
    {
        Task<IEnumerable<IChangelogTitle>> GetTitles(Guid dbId, string dcvId);
        Task<IChangelogTitle> GetPendingTitle(Guid dbId, string dcvId);
        Task<IEnumerable<IChangelogTitle>> GetAllPendingTitles(Guid dbId);
        Task<ChangeStatus> GetTitleStatus(Guid dbid, string dcvid);
        Task<IChangelogTitle> ApproveTitle(Guid dbId, string dcvId);
        Task<IChangelogTitle> RejectTitle(Guid dbId, string dcvId);
    }
}
