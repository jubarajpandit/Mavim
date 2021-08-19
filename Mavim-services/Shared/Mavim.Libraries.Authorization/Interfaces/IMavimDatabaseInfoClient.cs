using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Interfaces
{
    public interface IMavimDatabaseInfoClient
    {
        /// <summary>
        /// Gets all valid databases of the user.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IMavimDatabaseInfo>> GetMavimDatabaseInfoList();

        /// <summary>
        /// Gets the mavim database information.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        Task<IMavimDatabaseInfo> GetMavimDatabaseInfo(Guid dbId);
    }
}
