using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Commands;
using Mavim.Manager.Model;
using Mavim.Manager.Utils;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Commands
{
    public class ChangeParentCommandTest
    {
        const string topicId = "d5926266c57v0";
        const string topicParentId = "d5926266c664v0";

        [Fact]
        public async Task Execute_ValidArguments_ModelCommandCalled()
        {
            // Arrange
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            Mock<IElement> topicMock = new();
            var topicDcv = DcvId.FromDcvKey(topicId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicDcv)).Returns(topicMock.Object);
            Mock<IElement> topicParentMock = new();
            var topicParentDcv = DcvId.FromDcvKey(topicParentId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicParentDcv)).Returns(topicParentMock.Object);
            topicMock.Setup(x => x.Bizz.CanCut).Returns(true);
            topicParentMock.Setup(x => x.Bizz.CanCutPasteLower(new[] { topicMock.Object })).Returns(true);
            Mock<IMavimDatabaseModelCommand> modelCommandMock = new();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(true);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateMoveElementsToParentCommand(new[] { topicMock.Object }, topicParentMock.Object)).Returns(modelCommandMock.Object);
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            await command.Execute(topicId, topicParentId);

            // Assert
            modelCommandMock.Verify(mock => mock.Execute(It.IsAny<IProgress>()), Times.Once);
        }

        [Fact]
        public async Task Execute_InvalidTopicId_BadRequestException()
        {
            // Arrange
            var invalidTopic = "test";
            var message = $"Supplied topicId format is invalid: {invalidTopic}";
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var response = await Record.ExceptionAsync(async () => await command.Execute(invalidTopic, topicParentId));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestException>(response);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Execute_InvalidTopicParentId_BadRequestException()
        {
            // Arrange
            var invalidTopic = "test";
            var message = $"Supplied topicId format is invalid: {invalidTopic}";
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var response = await Record.ExceptionAsync(async () => await command.Execute(topicId, invalidTopic));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestException>(response);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Execute_EqualTopicAndTopicParent_BadRequestException()
        {
            // Arrange
            var message = "The topic id's cannot be the same value";
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var response = await Record.ExceptionAsync(async () => await command.Execute(topicId, topicId));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestException>(response);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Execute_TopicNull_RequestNotFoundException()
        {
            // Arrange
            var message = "Topic not found";
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            var topicDcv = DcvId.FromDcvKey(topicId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicDcv)).Returns((IElement)null);
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var response = await Record.ExceptionAsync(async () => await command.Execute(topicId, topicParentId));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<RequestNotFoundException>(response);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Execute_TopicParentNull_RequestNotFoundException()
        {
            // Arrange
            var message = "Parent topic not found";
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            Mock<IElement> topicMock = new();
            var topicDcv = DcvId.FromDcvKey(topicId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicDcv)).Returns(topicMock.Object);
            var topicParentDcv = DcvId.FromDcvKey(topicParentId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicParentDcv)).Returns((IElement)null);
            Mock<IMavimDatabaseModelCommand> modelCommandMock = new();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(true);
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var response = await Record.ExceptionAsync(async () => await command.Execute(topicId, topicParentId));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<RequestNotFoundException>(response);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Execute_CanMoveToElementLowerFalse_ForbiddenRequestException()
        {
            // Arrange
            var message = "Cannot move topic";
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            Mock<IElement> topicMock = new();
            var topicDcv = DcvId.FromDcvKey(topicId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicDcv)).Returns(topicMock.Object);
            Mock<IElement> topicParentMock = new();
            var topicParentDcv = DcvId.FromDcvKey(topicParentId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicParentDcv)).Returns(topicParentMock.Object);
            topicMock.Setup(x => x.Bizz.CanMoveToElementLower(topicParentMock.Object)).Returns(false);
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var response = await Record.ExceptionAsync(async () => await command.Execute(topicId, topicParentId));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<ForbiddenRequestException>(response);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Execute_CanExecuteFalse_ForbiddenRequestException()
        {
            // Arrange
            var message = "Cannot move topic";
            Mock<IMavimDatabaseModel> mavimDataModelMock = new();
            Mock<IElement> topicMock = new();
            var topicDcv = DcvId.FromDcvKey(topicId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicDcv)).Returns(topicMock.Object);
            Mock<IElement> topicParentMock = new();
            var topicParentDcv = DcvId.FromDcvKey(topicParentId);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(topicParentDcv)).Returns(topicParentMock.Object);
            topicMock.Setup(x => x.Bizz.CanMoveToElementLower(topicParentMock.Object)).Returns(true);
            Mock<IMavimDatabaseModelCommand> modelCommandMock = new();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(false);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateMoveElementsToParentCommand(new[] { topicMock.Object }, topicParentMock.Object)).Returns(modelCommandMock.Object);
            Mock<IMavimDbDataAccess> dataAccessMock = new();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);
            Mock<IDataLanguage> dataLanguage = new();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            ChangeParentCommand command = new(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var response = await Record.ExceptionAsync(async () => await command.Execute(topicId, topicParentId));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<ForbiddenRequestException>(response);
            Assert.Equal(message, response.Message);
        }
    }
}
