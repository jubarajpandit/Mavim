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
    public class DeleteTopicCommandTest
    {
        const string topicId = "d5926266c2v0";

        [Fact]
        public async Task Execute_ValidArguments_OkObjectResult()
        {
            // Arrange
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<DcvId>())).Returns(topicMock.Object);
            var modelCommandMock = new Mock<IMavimDatabaseModelCommand>();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(true);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateDeleteElementsCommand(It.IsAny<IElement[]>(), It.IsAny<bool>())).Returns(modelCommandMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new DeleteTopicCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(topicId));

            // Assert
            modelCommandMock.Verify(mock => mock.Execute(It.IsAny<IProgress>()), Times.Once);
            Assert.Null(exception);
        }

        [Fact]
        public async Task Execute_InvalidTopicId_BadRequestException()
        {
            // Arrange
            var exceptionMessage = "Supplied topicId format is invalid: incorrectTopicId";
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            var modelCommandMock = new Mock<IMavimDatabaseModelCommand>();

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new DeleteTopicCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute("incorrectTopicId"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_TopicNotFound_BadRequestException()
        {
            // Arrange
            var exceptionMessage = $"Topic to delete not found: {topicId}";
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<DcvId>())).Returns((IElement)null);
            var modelCommandMock = new Mock<IMavimDatabaseModelCommand>();

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new DeleteTopicCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_CannotExecute_ForbiddenRequestException()
        {
            // Arrange
            var exceptionMessage = $"Unable to delete topic: {topicId}";
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<DcvId>())).Returns(topicMock.Object);
            var modelCommandMock = new Mock<IMavimDatabaseModelCommand>();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(false);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateDeleteElementsCommand(It.IsAny<IElement[]>(), It.IsAny<bool>())).Returns(modelCommandMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new DeleteTopicCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
