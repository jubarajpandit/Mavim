using Mavim.Manager.Api.Catalog.Controllers.v1;
using Mavim.Manager.Api.Catalog.Services.Interfaces.v1;
using Mavim.Manager.Api.Catalog.Services.Interfaces.v1.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Controllers.v1
{
    public class CatalogControllerTest
    {
        private static readonly Guid DATABASEID = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");

        [Fact]
        [Trait("Category", "Catalog")]
        public async Task GetMavimDatabases_ValidArguments_OkObjectResult()
        {
            // Arrange
            var catalogServiceMock = new Mock<ICatalogService>();
            var databaseInfoMock = new Mock<IDatabaseInfo>();
            catalogServiceMock.Setup(x => x.GetMavimDatabases())
                            .ReturnsAsync(new List<IDatabaseInfo> { databaseInfoMock.Object });

            var controller = new CatalogController(catalogServiceMock.Object);

            // Act
            var actionResult = await controller.GetMavimDatabases();

            // Assert
            catalogServiceMock.Verify(mock => mock.GetMavimDatabases(), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IEnumerable<IDatabaseInfo>;
            Assert.NotNull(fieldsResult);
            Assert.True(fieldsResult.Any());
        }

        [Fact]
        [Trait("Category", "Catalog")]
        public async Task GetMavimDatabase_ValidArguments_OkObjectResult()
        {
            // Arrange
            var catalogServiceMock = new Mock<ICatalogService>();
            var databaseInfoMock = new Mock<IDatabaseInfo>();
            catalogServiceMock.Setup(x => x.GetMavimDatabase(DATABASEID))
                            .ReturnsAsync(databaseInfoMock.Object);

            var controller = new CatalogController(catalogServiceMock.Object);

            // Act
            var actionResult = await controller.GetMavimDatabase(DATABASEID);

            // Assert
            catalogServiceMock.Verify(mock => mock.GetMavimDatabase(DATABASEID), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IDatabaseInfo;
            Assert.NotNull(fieldsResult);
        }
    }
}