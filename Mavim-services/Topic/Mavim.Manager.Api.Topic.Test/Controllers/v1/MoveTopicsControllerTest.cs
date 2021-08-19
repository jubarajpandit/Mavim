using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Api.Topic.Controllers.v1;
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
    public class MoveTopicsControllerTest
    {
        const string topicId = "d5926266c2v0";

        #region MoveToTop
        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveToTop_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IMoveToTopCommand>();
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.MoveTopicToTop(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveToTop_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.MoveTopicToTop(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region MoveToBottom
        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveToBottom_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IMoveToBottomCommand>();
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.MoveTopicToBottom(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveToBottom_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.MoveTopicToBottom(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region MoveUp
        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveUp_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IMoveUpCommand>();
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.MoveTopicUp(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveUp_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.MoveTopicUp(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region MoveDown
        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveDown_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IMoveDownCommand>();
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.MoveTopicDown(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveDown_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.MoveTopicDown(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region MoveLevelUp
        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveLevelUp_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IMoveLevelUpCommand>();
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.MoveTopicLevelUp(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveLevelUp_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.MoveTopicLevelUp(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region MoveLevelDown
        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveLevelDown_ValidArguments_OkResult()
        {
            // Arrange
            var commandMock = new Mock<IMoveLevelDownCommand>();
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.MoveTopicLevelDown(dbid, DataLanguages.en, topicId, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task MoveLevelDown_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.MoveTopicLevelDown(dbid, DataLanguages.en, topicId, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region ChangeParentTopic
        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task ChangeParentTopic_ValidArguments_NoContentResult()
        {
            // Arrange
            var commandMock = new Mock<IChangeParentCommand>();
            var body = new SaveTopicParent
            {
                ParentId = topicId
            };
            var controller = new MoveTopicsController();
            var dbid = Guid.Empty;

            // Act
            var actionResult = await controller.ChangeParentTopic(dbid, DataLanguages.en, topicId, body, commandMock.Object);

            // Assert
            commandMock.Verify(mock => mock.Execute(topicId, body.ParentId), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task ChangeParentTopic_CommandNull_ArgumentNullException()
        {
            // Arrange
            var controller = new MoveTopicsController();
            var body = new SaveTopicParent
            {
                ParentId = topicId
            };
            var dbid = Guid.Empty;

            // Act
            var exception = await Record.ExceptionAsync(async () => await controller.ChangeParentTopic(dbid, DataLanguages.en, topicId, body, null));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "MoveTopic")]
        public async Task ChangeParentTopic_ModelStateInvalid_BadRequest()
        {
            // Arrange
            var commandMock = new Mock<IChangeParentCommand>();
            var controller = new MoveTopicsController();
            controller.ModelState.AddModelError("Error", "Error");
            var body = new SaveTopicParent
            {
                ParentId = topicId
            };
            var dbid = Guid.Empty;

            // Act
            var response = await controller.ChangeParentTopic(dbid, DataLanguages.en, topicId, body, commandMock.Object);

            // Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        #endregion

        #region ModelState
        [Theory, MemberData(nameof(ValidTopics))]
        public void ModelState_ValidSaveTopic_ValidModalState(string parentId)
        {
            // Arrange
            var saveTopicParent = new SaveTopicParent
            {
                ParentId = parentId
            };
            var context = new ValidationContext(saveTopicParent, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(SaveTopicParent), typeof(SaveTopicParent)), typeof(SaveTopicParent));

            // Act
            var isModelStateValid = Validator.TryValidateObject(saveTopicParent, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidTopics))]
        public void ModelState_InvalidSaveTopic_ValidModalState(string parentId)
        {
            // Arrange
            var saveTopicParent = new SaveTopicParent
            {
                ParentId = parentId
            };
            var context = new ValidationContext(saveTopicParent, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(SaveTopicParent), typeof(SaveTopicParent)), typeof(SaveTopicParent));

            // Act
            var isModelStateValid = Validator.TryValidateObject(saveTopicParent, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
        }
        #endregion

        #region input parms
        public static IEnumerable<object[]> ValidTopics
        {
            get
            {
                yield return new object[] { "d5926266c8330v0" };
                yield return new object[] { "d5926266c12v0" };
                yield return new object[] { "d5926266c14v0" };
            }
        }

        public static IEnumerable<object[]> InvalidTopics
        {
            get
            {
                yield return new object[] { "test" };
                yield return new object[] { "d123456789c123456789v123456789" };
            }
        }
        #endregion
    }
}
