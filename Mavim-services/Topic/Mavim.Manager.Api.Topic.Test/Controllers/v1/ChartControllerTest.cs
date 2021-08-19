using Mavim.Manager.Api.Topic.Controllers.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Controllers.v1
{
    public class ChartControllerTest
    {
        const string DCVID = "d5926266c2v0";

        #region GetPathToRoot
        [Fact]
        [Trait("Category", "Chart")]
        public async Task GetTopicCharts_ValidArguments_OkObjectResult()
        {
            // Arrange
            var serviceMock = new Mock<IChartService>();
            serviceMock.Setup(service => service.GetTopicCharts(It.IsAny<string>())).ReturnsAsync(new List<IChart> { new Mock<IChart>().Object });
            var controller = new ChartController(serviceMock.Object);
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.GetTopicCharts(dbid, DataLanguages.en, DCVID);

            // Assert
            serviceMock.Verify(mock => mock.GetTopicCharts(DCVID), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IEnumerable<IChart>;
            Assert.NotNull(fieldsResult?.FirstOrDefault());
        }
        #endregion
    }
}
