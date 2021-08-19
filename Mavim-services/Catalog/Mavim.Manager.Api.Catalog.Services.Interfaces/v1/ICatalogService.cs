using Mavim.Manager.Api.Catalog.Services.Interfaces.v1.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Catalog.Services.Interfaces.v1
{
    public interface ICatalogService
    {
        Task<IEnumerable<IDatabaseInfo>> GetMavimDatabases();

        Task<IDatabaseInfo> GetMavimDatabase(Guid dbId);
    }
}
