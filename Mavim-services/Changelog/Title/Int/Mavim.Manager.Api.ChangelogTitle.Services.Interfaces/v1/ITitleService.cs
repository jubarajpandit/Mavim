using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1
{
    public interface ITitleService
    {
        /// <summary>
        /// Gets the titles.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        Task<IEnumerable<ITitle>> GetTitles(Guid dbid, string dcvid);
        /// <summary>
        /// Gets the pending title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        Task<ITitle> GetPendingTitle(Guid dbid, string dcvid);
        /// <summary>
        /// Gets all pending titles.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <returns></returns>
        Task<IEnumerable<ITitle>> GetAllPendingTitles(Guid dbid);
        /// <summary>
        /// Gets the title status.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        Task<ChangeStatus> GetTitleStatus(Guid dbid, string dcvid);
        /// <summary>
        /// Saves the title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        Task SaveTitle(ISaveTitle title);
        /// <summary>
        /// Approves the title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        Task<ITitle> ApproveTitle(Guid dbid, string dcvid);
        /// <summary>
        /// Rejects the title.
        /// </summary>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        Task<ITitle> RejectTitle(Guid dbid, string dcvid);
    }
}
