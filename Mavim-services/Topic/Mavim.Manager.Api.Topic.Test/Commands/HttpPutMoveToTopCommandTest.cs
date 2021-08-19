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
    public class HttpPutMoveToTopCommandTest
    {
        const string topicId = "d5926266c2v0";

        [Fact]
        public async Task Execute_ValidArguments_OkObjectResult()
        {
            // Arrange
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            var modelCommandMock = new Mock<IMavimDatabaseModelCommand>();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(true);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateMoveElementToFirstPositionInBranchCommand(It.IsAny<IElement>())).Returns(modelCommandMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new MoveToTopCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            await command.Execute(topicId);

            // Assert
            modelCommandMock.Verify(mock => mock.Execute(It.IsAny<IProgress>()), Times.Once);
        }

        [Fact]
        public async Task Execute_TopicNotFound_BadRequestException()
        {
            // Arrange
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns((IElement)null);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new MoveToTopCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Fact]
        public async Task Execute_CanExecuteFalse_ForbiddenRequestException()
        {
            // Arrange
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            var modelCommandMock = new Mock<IMavimDatabaseModelCommand>();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(false);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateMoveElementToFirstPositionInBranchCommand(It.IsAny<IElement>())).Returns(modelCommandMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new MoveToTopCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }
    }
}
