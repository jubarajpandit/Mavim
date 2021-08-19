using Mavim.Manager.Api.MavimDatabaseInfo.Controllers.v1;
using Mavim.Manager.Api.MavimDatabaseInfo.Services.Interfaces.v1;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Controllers.v1
{
    public class MavimDatabaseInfoControllerTest
    {
        private static readonly Guid DATABASEID = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");

        [Fact]
        [Trait("Category", "Catalog")]
        public async Task GetMavimDatabases_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mavimDatabaseInfoServiceMock = new Mock<IMavimDatabaseInfoService>();
            var databaseInfoMock = new Mock<IDbConnectionInfo>();
            mavimDatabaseInfoServiceMock.Setup(x => x.GetMavimDatabases())
                            .ReturnsAsync(new List<IDbConnectionInfo> { databaseInfoMock.Object });

            var controller = new MavimDatabaseController(mavimDatabaseInfoServiceMock.Object);

            //Act
            var actionResult = await controller.GetMavimDatabases();

            //Assert
            mavimDatabaseInfoServiceMock.Verify(mock => mock.GetMavimDatabases(), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IEnumerable<IDbConnectionInfo>;
            Assert.NotNull(fieldsResult);
            Assert.True(fieldsResult.Any());
        }

        [Fact]
        [Trait("Category", "Catalog")]
        public async Task GetMavimDatabase_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mavimDatabaseInfoServiceMock = new Mock<IMavimDatabaseInfoService>();
            var databaseInfoMock = new Mock<IDbConnectionInfo>();
            mavimDatabaseInfoServiceMock.Setup(x => x.GetMavimDatabase(DATABASEID))
                            .ReturnsAsync(databaseInfoMock.Object);

            var controller = new MavimDatabaseController(mavimDatabaseInfoServiceMock.Object);

            //Act
            var actionResult = await controller.GetMavimDatabase(DATABASEID);

            //Assert
            mavimDatabaseInfoServiceMock.Verify(mock => mock.GetMavimDatabase(DATABASEID), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IDbConnectionInfo;
            Assert.NotNull(fieldsResult);
        }
    }
}