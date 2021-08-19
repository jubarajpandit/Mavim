using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.MavimDatabaseInfo.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnectionInfo = Mavim.Manager.MavimDatabaseInfo.EFCore.Models.DbConnectionInfo;
using IService = Mavim.Manager.Api.MavimDatabaseInfo.Services.Interfaces.v1;

namespace Mavim.Manager.Api.MavimDatabaseInfo.Services.v1
{
    public class MavimDatabaseInfoService : IService.IMavimDatabaseInfoService
    {
        private readonly MavimDatabaseInfoDbContext _dbContext;
        private readonly IJwtSecurityToken _token;

        public MavimDatabaseInfoService(MavimDatabaseInfoDbContext dbContext, IJwtSecurityToken token)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public async Task<IEnumerable<IService.IDbConnectionInfo>> GetMavimDatabases()
        {
            IEnumerable<DbConnectionInfo> mavimDatabaseInfos = await _dbContext.MavimDatabases
                                                                                .AsNoTracking()
                                                                                .Where(d => d.TenantId == _token.TenantId)
                                                                                .ToListAsync();

            return mavimDatabaseInfos.Select(Map);
        }

        public async Task<IService.IDbConnectionInfo> GetMavimDatabase(Guid dbId)
        {
            DbConnectionInfo mavimDatabaseInfo = await _dbContext.MavimDatabases
                                                                    .AsNoTracking()
                                                                    .FirstOrDefaultAsync(d => d.Id == dbId && d.TenantId == _token.TenantId);

            if (mavimDatabaseInfo == null)
                throw new RequestNotFoundException("Database not found");

            return Map(mavimDatabaseInfo);
        }

        /// <summary>Adds a new MavimDatabase object to the database</summary>
        /// <param name="mavimDatabase"></param>
        /// <returns></returns>
        public async Task<bool> AddMavimDatabase(IService.IDbConnectionInfo mavimDatabase)
        {
            DbConnectionInfo newMavimDatabase = new DbConnectionInfo
            {
                DisplayName = mavimDatabase.DisplayName,
                ConnectionString = mavimDatabase.ConnectionString
            };

            _dbContext.MavimDatabases.Add(newMavimDatabase);

            //TODO: Change return type in something nice and useful
            return await _dbContext.SaveChangesAsync() > 0;
        }

        private static IService.IDbConnectionInfo Map(DbConnectionInfo dbConnectionInfo) =>
            new Models.DbConnectionInfo
            {
                Id = dbConnectionInfo.Id,
                DisplayName = dbConnectionInfo.DisplayName,
                ConnectionString = dbConnectionInfo.ConnectionString,
                Schema = dbConnectionInfo.Schema,
                TenantId = dbConnectionInfo.TenantId,
                ApplicationTenantId = dbConnectionInfo.ApplicationTenantId,
                ApplicationId = dbConnectionInfo.ApplicationId,
                ApplicationSecretKey = dbConnectionInfo.ApplicationSecretKey,
                IsInternalDatabase = dbConnectionInfo.IsInternalDatabase
            };
    }
}