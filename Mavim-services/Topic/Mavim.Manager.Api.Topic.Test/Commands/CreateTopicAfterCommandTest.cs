using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Commands;
using Mavim.Manager.Model;
using Mavim.Manager.Utils;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Commands
{
    public class CreateTopicAfterCommandTest
    {
        const string referenceId = "d5926266c2v0";
        const string modelElementType = "MvmSRV_ELEtpe_mav";
        const string topicIcon = "TreeIconID_PrimarySubject";

        [Fact]
        public async Task Execute_ValidArguments_OkObjectResult()
        {
            // Arrange
            var topicName = "test";
            var topicMock = new Mock<IElement>();
            topicMock.Setup(mock => mock.Name).Returns(topicName);
            topicMock.Setup(mock => mock.Bizz.CanDelete).Returns(true);
            topicMock.Setup(mock => mock.Visual.IconResourceID).Returns(Resources.TreeIconID.TreeIconID_PrimarySubject);
            var elementTypeMock = new Mock<IElementType>();
            elementTypeMock.Setup(mock => mock.Type).Returns(Server.ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_mav);
            var iconMock = new Mock<IIconType>();
            iconMock.Setup(icon => icon.IconResourceID).Returns(Resources.TreeIconID.TreeIconID_PrimarySubject);
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedChildElementTypes(It.IsAny<IElement>())).Returns(new List<IElementType> { elementTypeMock.Object });
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedIconTypes(It.IsAny<IElementType>())).Returns(new List<IIconType> { iconMock.Object });
            var modelCommandMock = new Mock<IMavimDatabaseModelResultCommand>();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(true);
            modelCommandMock.Setup(mock => mock.Execute(It.IsAny<IProgress>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateCreateElementAfterCommand(It.IsAny<IElement>())).Returns(modelCommandMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new CreateTopicAfterCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(referenceId, topicName, modelElementType, topicIcon);

            // Assert
            modelCommandMock.Verify(mock => mock.Execute(It.IsAny<IProgress>()), Times.Once);
            Assert.NotNull(result);
            Assert.False(result.HasChildren);
            Assert.Equal(topicName, result.Name);
            Assert.Equal(topicIcon, result.Icon);
            Assert.False(result.Business?.IsReadOnly);
            Assert.Equal(0, result.OrderNumber);
            Assert.Equal(Services.Interfaces.v1.enums.TopicType.Unknown, result.TypeCategory);
        }

        [Fact]
        public async Task Execute_TopicIdIncorrect_BadRequestException()
        {
            // Arrange
            var exceptionMessage = "Supplied topicId format is invalid: incorrectTopicId";
            var incorrectTopicId = "incorrectTopicId";
            var topicName = "test";
            var topicIcon = "test";
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns((IElement)null);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new CreateTopicAfterCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(incorrectTopicId, topicName, modelElementType, topicIcon));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_TopicNotFound_BadRequestException()
        {
            // Arrange
            var exceptionMessage = "Supplied reference topic to create topic below not found: d5926266c2v0";
            var topicName = "test";
            var topicIcon = "test";
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns((IElement)null);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new CreateTopicAfterCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(referenceId, topicName, modelElementType, topicIcon));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_UnknownType_BadRequestException()
        {
            // Arrange
            var exceptionMessage = "Unknown topic type supplied: invalidElementType";
            var invalidElementType = "invalidElementType";
            var topicName = "test";
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new CreateTopicAfterCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(referenceId, topicName, invalidElementType, topicIcon));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_InvalidType_BadRequestException()
        {
            // Arrange
            var exceptionMessage = "Invalid topic type supplied for this topic: MvmSRV_ELEtpe_mav";
            var topicName = "test";
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedChildElementTypes(It.IsAny<IElement>())).Returns(new List<IElementType> { null });

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new CreateTopicAfterCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(referenceId, topicName, modelElementType, topicIcon));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_InvalidIcon_BadRequestException()
        {
            // Arrange
            var exceptionMessage = "Invalid topic icon supplied for this topic: TreeIconID_PrimarySubject";
            var topicName = "test";
            var topicMock = new Mock<IElement>();
            var elementTypeMock = new Mock<IElementType>();
            elementTypeMock.Setup(mock => mock.Type).Returns(Server.ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_mav);
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedChildElementTypes(It.IsAny<IElement>())).Returns(new List<IElementType> { elementTypeMock.Object });
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedIconTypes(It.IsAny<IElementType>())).Returns(new List<IIconType> { null });

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new CreateTopicAfterCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(referenceId, topicName, modelElementType, topicIcon));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_CanExecuteFalse_ForbiddenRequestException()
        {
            // Arrange
            var exceptionMessage = "Cannot create topic below reference topic";
            var topicName = "test";
            var topicMock = new Mock<IElement>();
            var elementTypeMock = new Mock<IElementType>();
            elementTypeMock.Setup(mock => mock.Type).Returns(Server.ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_mav);
            var iconMock = new Mock<IIconType>();
            iconMock.Setup(icon => icon.IconResourceID).Returns(Resources.TreeIconID.TreeIconID_PrimarySubject);
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedChildElementTypes(It.IsAny<IElement>())).Returns(new List<IElementType> { elementTypeMock.Object });
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedIconTypes(It.IsAny<IElementType>())).Returns(new List<IIconType> { iconMock.Object });
            var modelCommandMock = new Mock<IMavimDatabaseModelResultCommand>();
            modelCommandMock.Setup(mock => mock.CanExecute()).Returns(false);
            mavimDataModelMock.Setup(model => model.Factories.CommandFactory.CreateCreateElementAfterCommand(It.IsAny<IElement>())).Returns(modelCommandMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new CreateTopicAfterCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(referenceId, topicName, modelElementType, topicIcon));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
