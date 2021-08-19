using Mavim.Manager.Api.Topic.Controllers.v1;
using Mavim.Manager.Api.Topic.Queries.Interfaces;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.v1.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Controllers.v1
{
    public class TopicsMetaControllerTest
    {
        const string topicId = "d5926266c2v0";

        #region RetrieveTopicTypes
        [Fact]
        [Trait("Category", "TopicsMeta")]
        public async Task RetrieveTopicTypes_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IQueryRetrieveTopicTypesCommand>();
            var controller = new TopicsMetaController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.GetTopicTypes(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<ActionResult<IDictionary<string, ElementTypeInfo>>>(actionResult);
        }

        [Fact]
        [Trait("Category", "TopicsMeta")]
        public async Task RetrieveTopicTypes_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new TopicsMetaController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.GetTopicTypes(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region RetrieveTopicIcons
        [Fact]
        [Trait("Category", "TopicsMeta")]
        public async Task RetrieveTopicIcons_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IQueryRetrieveTopicIconsCommand>();
            var controller = new TopicsMetaController();
            var dbid = Guid.Empty;
            var dataLanguage = string.Empty;

            // Act
            var actionResult = await controller.GetTopicIcons(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<ActionResult<IDictionary<string, string>>>(actionResult);
        }

        [Fact]
        [Trait("Category", "TopicsMeta")]
        public async Task RetrieveTopicIcons_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new TopicsMetaController();
            var dbid = Guid.Empty;
            var dataLanguage = string.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.GetTopicIcons(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion
    }
}
