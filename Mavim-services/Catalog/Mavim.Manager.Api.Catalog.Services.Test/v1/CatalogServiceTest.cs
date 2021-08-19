using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Catalog.Services.Interfaces.v1.Models;
using Mavim.Manager.Api.Catalog.Services.v1;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Catalog.Services.Test.v1
{
    public class CatalogServiceTest
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


        [Theory, MemberData(nameof(ValidDatabaseObjects))]
        [Trait("Category", "CatalogService")]
        public async Task GetMavimDatabases_ValidArguments_DatabaseInfoList(IMavimDatabaseInfo clientResponse, IDatabaseInfo serviceResponse)
        {
            // Arrange
            var databaseInfoClientMock = new Mock<IMavimDatabaseInfoClient>();
            databaseInfoClientMock.Setup(x => x.GetMavimDatabaseInfoList()).ReturnsAsync(new List<IMavimDatabaseInfo>() { clientResponse });
            var tokenMock = GetTokenMock(_tenantId);
            var service = new CatalogService(tokenMock.Object, databaseInfoClientMock.Object);

            // Act
            var result = await service.GetMavimDatabases();

            // Assert
            databaseInfoClientMock.Verify(mock => mock.GetMavimDatabaseInfoList(), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IDatabaseInfo>>(result);
            Assert.Equal(result.First().DisplayName, serviceResponse.DisplayName);
            Assert.Equal(result.First().DatabaseID, serviceResponse.DatabaseID);
        }

        [Theory, MemberData(nameof(InvalidClientResponse))]
        [Trait("Category", "CatalogService")]
        public async Task GetMavimDatabases_InvalidArguments_RequestNotFoundException(IEnumerable<IMavimDatabaseInfo> clientResponse)
        {
            // Arrange
            var databaseInfoClientMock = new Mock<IMavimDatabaseInfoClient>();
            databaseInfoClientMock.Setup(x => x.GetMavimDatabaseInfoList()).ReturnsAsync(clientResponse);
            var tokenMock = GetTokenMock(_tenantId);
            var service = new CatalogService(tokenMock.Object, databaseInfoClientMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await service.GetMavimDatabases());

            // Assert
            databaseInfoClientMock.Verify(mock => mock.GetMavimDatabaseInfoList(), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RequestNotFoundException>(result);
        }

        [Theory, MemberData(nameof(ValidDatabaseClientObject))]
        [Trait("Category", "CatalogService")]
        public async Task GetMavimDatabases_InvalidTenantId_ForbiddenRequestException(IMavimDatabaseInfo clientResponse)
        {
            // Arrange
            var databaseInfoClientMock = new Mock<IMavimDatabaseInfoClient>();
            databaseInfoClientMock.Setup(x => x.GetMavimDatabaseInfoList()).ReturnsAsync(new List<IMavimDatabaseInfo> { clientResponse });
            var tokenMock = GetTokenMock();
            var service = new CatalogService(tokenMock.Object, databaseInfoClientMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await service.GetMavimDatabases());

            // Assert
            databaseInfoClientMock.Verify(mock => mock.GetMavimDatabaseInfoList(), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ForbiddenRequestException>(result);
        }

        public static IEnumerable<object[]> ValidDatabaseObjects
        {
            get
            {
                yield return new object[] {
                    GetDatabaseInfoClientMock(_id, _displayName, _connectionString, _schema, _tenantId, _applicationTenantId, _applicationId, _applicationSecretKey, _isInternal).Object,
                    GetDatabaseInfoServiceMock(_id, _displayName).Object
                };
            }
        }

        public static IEnumerable<object[]> ValidDatabaseClientObject
        {
            get
            {
                yield return new object[] {
                    GetDatabaseInfoClientMock(_id, _displayName, _connectionString, _schema, _tenantId, _applicationTenantId, _applicationId, _applicationSecretKey, _isInternal).Object
                };
            }
        }

        public static IEnumerable<object[]> InvalidTenantIdObjects
        {
            get
            {
                yield return new object[] {
                    GetDatabaseInfoClientMock(_id, _displayName, _connectionString, _schema, _tenantId, _applicationTenantId, _applicationId, _applicationSecretKey, _isInternal).Object,
                };
            }
        }

        public static IEnumerable<object[]> InvalidClientResponse
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { new List<IMavimDatabaseInfo>() };
            }
        }

        private static Mock<IDatabaseInfo> GetDatabaseInfoServiceMock(Guid databaseId, string displayName)
        {
            var mock = new Mock<IDatabaseInfo>();
            mock.Setup(x => x.DatabaseID).Returns(databaseId);
            mock.Setup(x => x.DisplayName).Returns(displayName);

            return mock;
        }

        private static Mock<IMavimDatabaseInfo> GetDatabaseInfoClientMock(Guid id, string displayName, string connectionString, string schema, Guid tenantId, Guid applicationTenantId, Guid applicationId, Guid applicationSecret, bool isInternal)
        {
            var mock = new Mock<IMavimDatabaseInfo>();
            mock.Setup(x => x.Id).Returns(id);
            mock.Setup(x => x.DisplayName).Returns(displayName);
            mock.Setup(x => x.ConnectionString).Returns(connectionString);
            mock.Setup(x => x.Schema).Returns(schema);
            mock.Setup(x => x.TenantId).Returns(tenantId);
            mock.Setup(x => x.ApplicationTenantId).Returns(applicationTenantId);
            mock.Setup(x => x.ApplicationId).Returns(applicationSecret);
            mock.Setup(x => x.ApplicationSecretKey).Returns(applicationSecret);
            mock.Setup(x => x.IsInternalDatabase).Returns(isInternal);

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
