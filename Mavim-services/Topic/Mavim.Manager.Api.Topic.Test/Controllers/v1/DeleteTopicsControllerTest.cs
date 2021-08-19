using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Api.Topic.Controllers.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Controllers.v1
{
    public class DeleteTopicsControllerTest
    {
        const string topicId = "d5926266c2v0";

        #region DeleteTopic
        [Fact]
        [Trait("Category", "DeleteTopic")]
        public async Task DeleteTopic_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IDeleteTopicCommand>();
            var controller = new DeleteTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.DeleteTopic(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        [Trait("Category", "DeleteTopic")]
        public async Task RetrieveTopicTypes_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new DeleteTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.DeleteTopic(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion
    }
}
