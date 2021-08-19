using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1
{
    public interface ITitleRepository
    {
        Task<IEnumerable<ITitle>> GetTitles(Guid tenantId, Guid dbid, string dcvid);
        Task<ITitle> GetPendingTitle(Guid tenantId, Guid dbid, string dcvid);
        Task<IEnumerable<ITitle>> GetAllPendingTitles(Guid tenantId, Guid dbid);
        Task<ChangeStatus> GetTitleStatus(Guid tenantId, Guid dbid, string dcvid);
        Task SaveTitle(ITitle title);
        Task<ITitle> UpdateTitleState(ITitle title);
    }
}
