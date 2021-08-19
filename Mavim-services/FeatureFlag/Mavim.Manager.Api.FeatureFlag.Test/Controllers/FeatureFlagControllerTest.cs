using Mavim.Manager.Api.FeatureFlag.Controllers;
using Mavim.Manager.Api.FeatureFlag.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.FeatureFlag.Test.Controllers
{
    public class FeatureFlagControllerTest
    {
        [Fact]
        [Trait("Category", "FeatureFlag")]
        public async Task GetFeatureFlag_ValidArguments_OkObjectResult()
        {
            // Arrange
            var controller = new FeatureFlagController();

            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(query => query.Send(It.IsAny<GetActiveFeatureFlags.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new List<string> {
                "featureA",
                "featureB"
              }
            );

            // Act
            var actionResult = await controller.GetFeatureFlag(mockMediatr.Object);

            // Assert
            mockMediatr.Verify(mock => mock.Send(It.IsAny<GetActiveFeatureFlags.Query>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var featureflagResult = okObjectResult.Value as List<string>;
            Assert.NotNull(featureflagResult);
            Assert.NotEmpty(featureflagResult);
        }
    }
}
