using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Catalog.Services.Interfaces.v1;
using Mavim.Manager.Api.Catalog.Services.Interfaces.v1.Models;
using Mavim.Manager.Api.Catalog.Services.v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Catalog.Services.v1
{
    public class CatalogService : ICatalogService
    {
        private readonly IJwtSecurityToken _token;
        private readonly IMavimDatabaseInfoClient _mavimDatabaseInfoClient;

        public CatalogService(IJwtSecurityToken token, IMavimDatabaseInfoClient mavimDatabaseInfoClient)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _mavimDatabaseInfoClient = mavimDatabaseInfoClient ?? throw new ArgumentNullException(nameof(_mavimDatabaseInfoClient));
        }

        public async Task<IEnumerable<IDatabaseInfo>> GetMavimDatabases()
        {
            IEnumerable<IMavimDatabaseInfo> mavimDatabaseInfoList = await _mavimDatabaseInfoClient.GetMavimDatabaseInfoList();

            if (mavimDatabaseInfoList == null || !mavimDatabaseInfoList.Any())
                throw new RequestNotFoundException("Databases not found");

            if (mavimDatabaseInfoList.Any(dbInfo => dbInfo.TenantId != _token.TenantId))
                throw new ForbiddenRequestException($"Unauthorized for requested databases");

            return mavimDatabaseInfoList.Select(Map);
        }

        public async Task<IDatabaseInfo> GetMavimDatabase(Guid dbId)
        {
            IMavimDatabaseInfo mavimDatabaseInfo = await _mavimDatabaseInfoClient.GetMavimDatabaseInfo(dbId);

            if (mavimDatabaseInfo == null)
                throw new RequestNotFoundException("Database not found");


            if (mavimDatabaseInfo.TenantId != _token.TenantId)
                throw new ForbiddenRequestException($"Unauthorized for requested database");

            return Map(mavimDatabaseInfo);
        }

        private IDatabaseInfo Map(IMavimDatabaseInfo databaseInfo) =>
            new DatabaseInfo
            {
                DisplayName = databaseInfo.DisplayName,
                DatabaseID = databaseInfo.Id
            };
    }
}
