using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Api.Topic.Controllers.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.v1.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Controllers.v1
{
    public class CreateTopicsControllerTest
    {
        const string topicId = "d5926266c2v0";

        #region CreateTopicAfter
        [Fact]
        [Trait("Category", "CreateTopic")]
        public async Task RetrieveTopicTypes_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<ICreateTopicAfterCommand>();
            commandMock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Mock<ITopic>().Object);
            var createTopic = new CreateTopic() { Name = "test", Type = "type", Icon = "icon" };
            var controller = new CreateTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.CreateTopicAfter(dbid, DataLanguages.en, topicId, createTopic, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId, createTopic.Name, createTopic.Type, createTopic.Icon), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var topicResult = okObjectResult.Value as ITopic;
            Assert.NotNull(topicResult);
        }

        [Fact]
        [Trait("Category", "CreateTopic")]
        public async Task RetrieveTopicTypes_CommandNull_ArgumentNullException()
        {
            // Arrange
            var createTopic = new CreateTopic() { Name = "test" };
            var controller = new CreateTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.CreateTopicAfter(dbid, DataLanguages.en, topicId, createTopic, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region CreateChildTopic
        [Fact]
        [Trait("Category", "CreateTopic")]
        public async Task RetrieveChildsTopicTypes_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<ICreateChildTopicCommand>();
            commandMock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Mock<ITopic>().Object);
            var createTopic = new CreateTopic() { Name = "test", Type = "type", Icon = "Icon" };
            var controller = new CreateTopicsController();
            var dbId = Guid.Empty;

            // Act
            var actionResult = await controller.CreateChildTopic(dbId, DataLanguages.en, topicId, createTopic, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId, createTopic.Name, createTopic.Type, createTopic.Icon), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var topicResult = okObjectResult.Value as ITopic;
            Assert.NotNull(topicResult);
        }

        [Fact]
        [Trait("Category", "CreateTopic")]
        public async Task RetrieveChildsTopicTypes_CommandNull_ArgumentNullException()
        {
            // Arrange
            var createTopic = new CreateTopic() { Name = "test" };
            var controller = new CreateTopicsController();
            var dbId = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.CreateChildTopic(dbId, DataLanguages.en, topicId, createTopic, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region ModelState
        [Fact]
        public void ModelState_ValidCreateTopic_ValidModalState()
        {
            // Arrange
            var createTopic = new CreateTopic();
            createTopic.Name = "test";
            createTopic.Type = "type";
            createTopic.Icon = "icon";
            var context = new ValidationContext(createTopic, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(CreateTopic), typeof(CreateTopic)), typeof(CreateTopic));

            // Act
            var isModelStateValid = Validator.TryValidateObject(createTopic, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Fact]
        public void ModelState_InValidCreateTopic_InValidModalState()
        {
            // Arrange
            var createTopic = new CreateTopic();
            var context = new ValidationContext(createTopic, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(CreateTopic), typeof(CreateTopic)), typeof(CreateTopic));

            // Act
            var isModelStateValid = Validator.TryValidateObject(createTopic, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
        }
        #endregion
    }
}
