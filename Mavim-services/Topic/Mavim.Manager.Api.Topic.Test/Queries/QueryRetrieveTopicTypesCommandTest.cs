using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Queries;
using Mavim.Manager.Model;
using Mavim.Manager.Resources;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Test.Queries
{
    public class QueryRetrieveTopicTypesCommandTest
    {
        const string topicId = "d5926266c2v0";

        [Fact]
        public async Task Execute_ValidArguments_TypeValue()
        {
            // Arrange
            var typeValue = "typeName";
            var resourceId = TreeIconID.TreeIconID_Activity.ToString("G");
            var typeMock = new Mock<IElementType>();
            typeMock.Setup(x => x.Type).Returns(Server.ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_sub);
            typeMock.Setup(x => x.GetName(It.IsAny<IMavimDatabaseModel>())).Returns(typeValue);
            typeMock.Setup(x => x.GetIcon(It.IsAny<IMavimDatabaseModel>()).IconResourceID).Returns(TreeIconID.TreeIconID_Activity);
            var topicMock = new Mock<IElement>();
            topicMock.Setup(x => x.Type).Returns(typeMock.Object);
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedChildElementTypes(It.IsAny<IElement>())).Returns(new List<IElementType> { typeMock.Object });

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicTypesCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeValue, result[Server.ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_sub.ToString()].Name);
            Assert.False(result[Server.ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_sub.ToString()].IsSystemName);
            Assert.Equal(resourceId, result[Server.ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_sub.ToString()].ResourceId);
        }

        [Fact]
        public async Task Execute_EmptyTypes_EmptyList()
        {
            // Arrange
            var typeValue = "typeName";
            var typeMock = new Mock<IElementType>();
            typeMock.Setup(x => x.GetName(It.IsAny<IMavimDatabaseModel>())).Returns(typeValue);
            var topicMock = new Mock<IElement>();
            topicMock.Setup(x => x.Type).Returns(typeMock.Object);
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedElementTypes(It.IsAny<IElementType>())).Returns(new List<IElementType>());

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicTypesCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Execute_ListWithNull_Empty()
        {
            // Arrange
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedElementTypes(It.IsAny<IElementType>())).Returns(new List<IElementType> { null });

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicTypesCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Execute_NullList_Empty()
        {
            // Arrange
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedElementTypes(It.IsAny<IElementType>())).Returns((List<IElementType>)null);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicTypesCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Execute_InvalidTopicId_BadRequestException()
        {
            // Arrange
            var exceptionMessage = "Supplied topicId format is invalid: invalidTopicId";
            var invalidTopicId = "invalidTopicId";
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicTypesCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(invalidTopicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Execute_TopicNotFound_BadRequestException()
        {
            // Arrange
            var exceptionMessage = $"Supplied parent topic to get topic types not found: {topicId}";
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns((IElement)null);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicTypesCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
