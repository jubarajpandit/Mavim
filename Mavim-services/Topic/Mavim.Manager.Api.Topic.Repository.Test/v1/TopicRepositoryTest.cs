using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;
using Mavim.Manager.Api.Topic.Repository.v1;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using Microsoft.FeatureManagement;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Repository.Test.v1
{
    public class TopicRepositoryTest
    {
        [Theory, MemberData(nameof(DcvIdValues))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetPathToRoot_ValidArguments_ITopicPath(string dcvId)
        {
            //Arrange
            const string rootDcv = "d0c2v0";
            var rootMock = GetMockTopic(true);
            var topicMock = GetMockTopic();
            var internalMock = GetMockTopic(false, true, false);
            var objectMock = GetMockTopic(false, false, true);
            var chartMock = GetMockTopic(false, true, false, true);
            var children = new List<IElement>() { topicMock.Object, internalMock.Object, objectMock.Object, chartMock.Object };

            rootMock.SetupGet(topic => topic.DcvID).Returns(DcvId.FromDcvKey(rootDcv));
            topicMock.SetupGet(topic => topic.DcvID).Returns(DcvId.FromDcvKey(dcvId));
            topicMock.SetupGet(topic => topic.Parent).Returns(rootMock.Object);
            topicMock.SetupGet(topic => topic.Children).Returns(children);
            internalMock.SetupGet(topic => topic.DcvID).Returns(DcvId.FromDcvKey("d1c2v0"));
            objectMock.SetupGet(topic => topic.DcvID).Returns(DcvId.FromDcvKey("d2c2v0"));
            chartMock.SetupGet(topic => topic.DcvID).Returns(DcvId.FromDcvKey("d3c2v0"));

            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(DcvId.FromDcvKey(rootDcv))).Returns(rootMock.Object);
            mavimDataModelMock.SetupGet(model => model.RootElement).Returns(rootMock.Object);

            var featureManagerMock = new Mock<IFeatureManager>();

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var topicRepository = new TopicRepository(dataAccessMock.Object, dataLanguageMock.Object, featureManagerMock.Object);

            // Act
            var result = await topicRepository.GetPathToRoot(dcvId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITopicPath>(result);
            Assert.NotNull(result.Path);
            Assert.NotEmpty(result.Path);
            Assert.True(result.Path.Find(item => item.Order == 0).DcvId == rootDcv);
            Assert.True(result.Path.Find(item => item.Order == 1).DcvId == dcvId);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data[0].OrderNumber == 1);
            Assert.True(result.Data.FindAll(item => item.Dcv.ToString() == dcvId).Count == 1);
            Assert.True(result.Data.FindAll(item => item.Dcv.ToString() == chartMock.Object.DcvID.ToString()).Count == 1);
        }

        [Theory, MemberData(nameof(EmptyOrNullDcvIdValues))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetPathToRoot_EmptyOrNullDcvIdValues_ArgumentNullException(string dcvId)
        {
            //Arrange
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManagerMock = new Mock<IFeatureManager>();

            var topicRepository = new TopicRepository(dataAccessMock.Object, dataLanguageMock.Object, featureManagerMock.Object);


            // Act
            var result = await Record.ExceptionAsync(async () => await topicRepository.GetPathToRoot(dcvId));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        [Theory, MemberData(nameof(InvalidDcvIdValues))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetPathToRoot_InvalidDcvIdValues_ArgumentException(string dcvId)
        {
            //Arrange
            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManagerMock = new Mock<IFeatureManager>();

            var topicRepository = new TopicRepository(dataAccessMock.Object, dataLanguageMock.Object, featureManagerMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await topicRepository.GetPathToRoot(dcvId));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentException>(result);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetPathToRoot_ArrangedInfiniteParents_InvalidOperationException()
        {
            //Arrange
            const string rootDcv = "d0c2v0";
            var rootMock = GetMockTopic(true);
            var topicMock = GetMockTopic();
            var children = new List<IElement>();
            children.Add(topicMock.Object);

            topicMock.SetupGet(topic => topic.Parent).Returns(topicMock.Object);
            topicMock.SetupGet(topic => topic.Children).Returns(children);

            var mavimDataModelMock = new Mock<IMavimDatabaseModel>();
            mavimDataModelMock.Setup(model => model.ElementRepository.GetElement(It.IsAny<IDcvId>())).Returns(topicMock.Object);
            mavimDataModelMock.SetupGet(model => model.RootElement).Returns(rootMock.Object);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManagerMock = new Mock<IFeatureManager>();

            var topicRepository = new TopicRepository(dataAccessMock.Object, dataLanguageMock.Object, featureManagerMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await topicRepository.GetPathToRoot(rootDcv));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TimeoutException>(result);
        }

        public static IEnumerable<object[]> DcvIdValues
        {
            get
            {
                yield return new object[] { "d5926266c1v0" };
                yield return new object[] { "d12950883c414v0" };
            }
        }

        public static IEnumerable<object[]> InvalidDcvIdValues
        {
            get
            {
                yield return new object[] { "test" };
                yield return new object[] { "d12950abc883c414v0" };
            }
        }

        public static IEnumerable<object[]> EmptyOrNullDcvIdValues
        {
            get
            {
                yield return new object[] { "" };
                yield return new object[] { null };
            }
        }

        public static IEnumerable<object[]> ForbiddenTopicValues
        {
            get
            {
                yield return new object[] { false, true, false };
                yield return new object[] { false, false, true };
                yield return new object[] { false, true, true };
            }
        }

        private Mock<IDataLanguage> GetDataLanguageMock()
        {
            var mock = new Mock<IDataLanguage>();
            mock.Setup(x => x.Type).Returns(DataLanguageType.English);

            return mock;
        }

        private static Mock<IElement> GetMockTopic(bool isRoot = false, bool isInternal = false, bool isObjectType = false, bool isChart = false)
        {
            Mock<IElement> topicMock = new Mock<IElement>();
            Mock<IMavimDatabaseModel> dbMock = new Mock<IMavimDatabaseModel>();
            Mock<IDcvId> dcvIdMock = new Mock<IDcvId>();
            dcvIdMock.SetupGet(dcvId => dcvId.Dbs).Returns(12950883);
            dcvIdMock.SetupGet(dcvId => dcvId.Cde).Returns(414);
            dcvIdMock.SetupGet(dcvId => dcvId.Ver).Returns(0);

            Mock<IElementType> typeMock = new Mock<IElementType>();
            typeMock.SetupGet(type => type.HasSystemName).Returns(false);
            typeMock.SetupGet(type => type.IsImportedVersionsRoot).Returns(false);
            typeMock.SetupGet(type => type.IsImportedVersion).Returns(false);
            typeMock.SetupGet(type => type.IsCreatedVersionsRoot).Returns(false);
            typeMock.SetupGet(type => type.IsCreatedVersion).Returns(false);
            typeMock.SetupGet(type => type.IsRecycleBin).Returns(false);
            typeMock.SetupGet(type => type.IsRelationshipsCategoriesRoot).Returns(false);
            typeMock.SetupGet(type => type.IsExternalReferencesRoot).Returns(false);
            typeMock.SetupGet(type => type.IsObjectsRoot).Returns(isRoot);
            typeMock.SetupGet(type => type.IsChart).Returns(isChart);

            if (isObjectType)
                typeMock.SetupGet(type => type.Type).Returns(ELEBuffer.MvmSrv_ELEtpe.mvmsrv_eletpe_Objm);

            topicMock
                .SetupGet(x => x.Model).Returns(dbMock.Object);
            topicMock
                .SetupGet(x => x.Model.MavimHandle).Returns(1);
            topicMock
                .SetupProperty(topic => topic.Model.DataLanguage).SetReturnsDefault(new Mock<ILanguage>().Object);
            topicMock.SetupGet(topic => topic.DcvID).Returns(dcvIdMock.Object);
            topicMock.SetupGet(topic => topic.Type).Returns(typeMock.Object);
            topicMock.SetupGet(topic => topic.IsDeleted).Returns(false);
            topicMock.SetupGet(topic => topic.IsInternal).Returns(isInternal);
            topicMock.SetupGet(topic => topic.OrderNumber).Returns(1);
            topicMock.SetupGet(topic => topic.Bizz.CanDelete).Returns(true);

            return topicMock;
        }
    }
}
