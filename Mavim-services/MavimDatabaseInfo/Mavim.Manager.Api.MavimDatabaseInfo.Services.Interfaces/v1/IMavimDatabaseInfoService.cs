using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.MavimDatabaseInfo.Services.Interfaces.v1
{
    public interface IMavimDatabaseInfoService
    {
        /// <summary>Gets the Mavim Database connected to the user from header information.</summary>
        /// <returns></returns>
        Task<IDbConnectionInfo> GetMavimDatabase(Guid dbId);

        /// <summary>Gets a list of Mavim Database based on tenant.</summary>
        /// <returns></returns>
        Task<IEnumerable<IDbConnectionInfo>> GetMavimDatabases();

        /// <summary>Adds a new MavimDatabase object to the database</summary>
        /// <returns></returns>
        Task<bool> AddMavimDatabase(IDbConnectionInfo mavimDatabase);
    }
}
