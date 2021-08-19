using Mavim.Manager.Api.FeatureFlag.Queries;
using Microsoft.FeatureManagement;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.FeatureFlag.Test.Queries
{
    public class HttpGetFeatureFlagCommand
    {
        [Fact]
        [Trait("Category", "FeatureFlag")]
        public async Task Execute_ValidArguments_ListOfFeatureFlags()
        {
            // Arrange
            var featureManagerMock = new Mock<IFeatureManager>();
            featureManagerMock.Setup(fm => fm.GetFeatureNamesAsync()).Returns(GetTestValues());
            featureManagerMock.Setup(fm => fm.IsEnabledAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => IsEnable(name));
            var handler = new GetActiveFeatureFlags.Handler(featureManagerMock.Object);
            var request = new GetActiveFeatureFlags.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            featureManagerMock.Verify(mock => mock.GetFeatureNamesAsync(), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        private static bool IsEnable(string name) => name != "featureA";

        private static async IAsyncEnumerable<string> GetTestValues()
        {

            yield return "featureA";
            yield return "featureB";
            yield return "featureC";

            await Task.CompletedTask;
        }
    }
}
