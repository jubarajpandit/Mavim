using Azure.Security.KeyVault.Secrets;
using Mavim.Admin.Api.Import.Catalog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo = Mavim.Manager.MavimDatabaseInfo.EFCore;

namespace Mavim.Admin.Api.Import.Catalog.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/v1/admin/import")]
    public class ImportCatalogController : ControllerBase
    {
        private readonly Repo.MavimDatabaseInfoDbContext _dbContext;
        private readonly SecretClient _client;

        public ImportCatalogController(Repo.MavimDatabaseInfoDbContext dbContext, SecretClient client)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _client = client ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpPost]
        [Route("catalog")]
        public async Task<ActionResult<List<DbConnectionInfo>>> Import(List<DbConnectionInfo> customers)
        {
            IEnumerable<Task> tasks = customers.Select((info, key) =>
            {
                var secret = info.ApplicationSecretKey.ToString();
                var secretKey = Guid.NewGuid().ToString();

                customers[key].Id = Guid.NewGuid();
                customers[key].ApplicationSecretKey = secretKey;

                return _client.SetSecretAsync(secretKey, secret);
            });
            await Task.WhenAll(tasks);

            List<Repo.Models.DbConnectionInfo> dbConnectionInfo = customers.Select(Map).ToList();

            await _dbContext.MavimDatabases.AddRangeAsync(dbConnectionInfo);
            await _dbContext.SaveChangesAsync();

            return Ok(dbConnectionInfo);
        }

        private Repo.Models.DbConnectionInfo Map(DbConnectionInfo customer) => new Repo.Models.DbConnectionInfo
        {
            Id = customer.Id,
            DisplayName = customer.DisplayName,
            ConnectionString = customer.ConnectionString,
            Schema = customer.Schema,
            TenantId = customer.TenantId,
            ApplicationId = customer.ApplicationId,
            ApplicationSecretKey = new Guid(customer.ApplicationSecretKey),
            ApplicationTenantId = customer.ApplicationTenantId,
            IsInternalDatabase = customer.IsInternalDatabase
        };
    }
}
