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
    public class QueryRetrieveTopicIconsCommandTest
    {
        const string modelElementType = "MvmSRV_ELEtpe_mav";

        [Theory, MemberData(nameof(ValidElementTypes))]
        public async Task Execute_ValidArguments_DictionaryWithValues(string elementType)
        {
            // Arrange
            var typeValue = "typeName";
            var typeMock = new Mock<IIconType>();
            typeMock.Setup(x => x.IconResourceID).Returns(TreeIconID.TreeIconID_Activity);
            typeMock.Setup(x => x.Name).Returns(typeValue);
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedIconTypes(It.IsAny<IElementType>())).Returns(new List<IIconType> { typeMock.Object });
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedIconTypes(It.IsAny<IElement>())).Returns(new List<IIconType> { typeMock.Object });

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicIconsCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(elementType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeValue, result[TreeIconID.TreeIconID_Activity.ToString()]);
        }

        [Fact]
        public async Task Execute_CustomIcons_Empty()
        {
            // Arrange
            var typeValue = "typeName";
            var typeMock = new Mock<IIconType>();
            typeMock.Setup(x => x.IconResourceID).Returns(TreeIconID.TreeIconID_Activity);
            typeMock.Setup(x => x.Name).Returns(typeValue);
            typeMock.Setup(x => x.IsCustomIcon).Returns(true);
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedIconTypes(It.IsAny<IElementType>())).Returns(new List<IIconType> { typeMock.Object });
            mavimDataModelMock.Setup(model => model.Queries.GetAllowedIconTypes(It.IsAny<IElement>())).Returns(new List<IIconType> { typeMock.Object });

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicIconsCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(modelElementType);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
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

            var command = new QueryRetrieveTopicIconsCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(modelElementType);

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

            var command = new QueryRetrieveTopicIconsCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(modelElementType);

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

            var command = new QueryRetrieveTopicIconsCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var result = await command.Execute(modelElementType);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Execute_UnknowTopicType_BadRequestException()
        {
            // Arrange
            var expected = "Unknown topic type supplied: unknown";
            var unknowTopicType = "unknown";
            var topicMock = new Mock<IElement>();
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguage = new Mock<IDataLanguage>();
            dataLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);

            var command = new QueryRetrieveTopicIconsCommand(dataAccessMock.Object, dataLanguage.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await command.Execute(unknowTopicType));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(expected, exception.Message);
        }

        public static IEnumerable<object[]> ValidElementTypes
        {
            get
            {
                yield return new object[] { modelElementType };
                yield return new object[] { modelElementType.ToLower() };
            }
        }
    }
}
