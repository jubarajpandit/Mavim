using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.MavimDatabaseInfo.Services.Interfaces.v1;
using Mavim.Manager.Api.MavimDatabaseInfo.Services.v1;
using Mavim.Manager.MavimDatabaseInfo.EFCore;
using Mavim.Manager.MavimDatabaseInfo.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Catalog.Services.Test.v1
{
    public class MavimDatabaseInfoServiceTest
    {
        private static readonly Guid _id = Guid.NewGuid();
        private static readonly string _displayName = "displayName";
        private static readonly string _connectionString = "connectionString";
        private static readonly string _schema = "schema";
        private static readonly Guid _tenantId = Guid.NewGuid();
        private static readonly Guid _applicationTenantId = Guid.NewGuid();
        private static readonly Guid _applicationId = Guid.NewGuid();
        private static readonly Guid _applicationSecretKey = Guid.NewGuid();
        private static readonly bool _isInternal = false;

        [Fact]
        [Trait("Category", "CatalogService")]
        public async Task GetMavimDatabases_ValidArguments_DbConnectionInfoList()
        {
            // Arrange
            var options = getInMemoryMavimDatabaseInfoDbContextOptions("DbConnectionInfoList");
            var logging = new Mock<ILogger<MavimDatabaseInfoDbContext>>();
            var dbContext = new MavimDatabaseInfoDbContext(logging.Object, options);
            var tokenMock = GetTokenMock(_tenantId);
            var service = new MavimDatabaseInfoService(dbContext, tokenMock.Object);

            // Act
            var result = await service.GetMavimDatabases();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IDbConnectionInfo>>(result);
            Assert.True(result.Count() == 1);
            Assert.Equal(result.First().TenantId, tokenMock.Object.TenantId);
        }

        [Fact]
        [Trait("Category", "CatalogService")]
        public async Task GetMavimDatabase_ValidArguments_DbConnectionInfo()
        {
            // Arrange
            var options = getInMemoryMavimDatabaseInfoDbContextOptions("DbConnectionInfo");
            var logging = new Mock<ILogger<MavimDatabaseInfoDbContext>>();
            var dbContext = new MavimDatabaseInfoDbContext(logging.Object, options);
            var tokenMock = GetTokenMock(_tenantId);
            var service = new MavimDatabaseInfoService(dbContext, tokenMock.Object);

            // Act
            var result = await service.GetMavimDatabase(_id);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IDbConnectionInfo>(result);
            Assert.Equal(result.TenantId, tokenMock.Object.TenantId);
        }

        [Fact]
        [Trait("Category", "CatalogService")]
        public async Task GetMavimDatabase_UnknownDatabaseId_RequestNotFoundException()
        {
            // Arrange
            var options = getInMemoryMavimDatabaseInfoDbContextOptions("RequestNotFoundException");
            var logging = new Mock<ILogger<MavimDatabaseInfoDbContext>>();
            var dbContext = new MavimDatabaseInfoDbContext(logging.Object, options);
            var tokenMock = GetTokenMock(_tenantId);
            var service = new MavimDatabaseInfoService(dbContext, tokenMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await service.GetMavimDatabase(Guid.Empty));

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RequestNotFoundException>(result);
        }

        [Fact]
        [Trait("Category", "CatalogService")]
        public async Task GetMavimDatabase_TenantIdMisMatch_ForbiddenRequestException()
        {
            // Arrange
            var options = getInMemoryMavimDatabaseInfoDbContextOptions("ForbiddenRequestException");
            var logging = new Mock<ILogger<MavimDatabaseInfoDbContext>>();
            var dbContext = new MavimDatabaseInfoDbContext(logging.Object, options);
            var tokenMock = GetTokenMock();
            var service = new MavimDatabaseInfoService(dbContext, tokenMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await service.GetMavimDatabase(_id));

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RequestNotFoundException>(result);
        }

        private DbContextOptions<MavimDatabaseInfoDbContext> getInMemoryMavimDatabaseInfoDbContextOptions(string databaseName)
        {
            var options = new DbContextOptionsBuilder<MavimDatabaseInfoDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

            var logging = new Mock<ILogger<MavimDatabaseInfoDbContext>>();

            // Insert seed data into the database using one instance of the context
            using (var context = new MavimDatabaseInfoDbContext(logging.Object, options))
            {
                context.MavimDatabases.Add(GetDbConnectionInfoMock(_id, _displayName, _connectionString, _schema, _tenantId, _applicationTenantId, _applicationId, _applicationSecretKey, _isInternal));
                context.MavimDatabases.Add(GetDbConnectionInfoMock(Guid.Empty, _displayName + "1", _connectionString, _schema, Guid.Empty, _applicationTenantId, _applicationId, _applicationSecretKey, _isInternal));
                context.SaveChanges();
            }

            return options;
        }

        private DbConnectionInfo GetDbConnectionInfoMock(Guid id, string displayName, string connectionString, string schema, Guid tenantId, Guid applicationTenantId, Guid applicationId, Guid applicationSecret, bool isInternal)
        {
            var mock = new DbConnectionInfo()
            {
                Id = id,
                DisplayName = displayName,
                ConnectionString = connectionString,
                Schema = schema,
                TenantId = tenantId,
                ApplicationTenantId = applicationTenantId,
                ApplicationId = applicationId,
                ApplicationSecretKey = applicationSecret,
                IsInternalDatabase = isInternal
            };

            return mock;
        }

        private static Mock<IJwtSecurityToken> GetTokenMock(Guid tenantId = default)
        {
            var mock = new Mock<IJwtSecurityToken>();
            mock.Setup(x => x.AppId).Returns(_applicationId);
            mock.Setup(x => x.TenantId).Returns(tenantId);
            mock.Setup(x => x.Name).Returns(_displayName);
            mock.Setup(x => x.RawData).Returns("rawData");
            mock.Setup(x => x.UserID).Returns(_id);

            return mock;
        }
    }
}
