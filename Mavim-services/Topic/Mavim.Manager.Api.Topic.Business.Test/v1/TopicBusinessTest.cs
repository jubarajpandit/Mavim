using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.v1;
using Mavim.Manager.Model;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Enums = Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;
using RepoEnums = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.Businesss.Test.v1
{
    public class TopicBusinessTest
    {
        private const string FAULT_DCVID = "a9bee371-b30f-418d-aca6-1feffccce822";
        private const string DCVID = "d12950883c414v0";
        private const string DCVID_V1 = "d12950883c414v1";

        [Fact]
        [Trait("Category", "TopicBusiness")]
        public async Task GetRootTopic_ValidArguments_Topic()
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<IRepo.ITopicRepository> topicRepositoryMock = new Mock<IRepo.ITopicRepository>();
            Mock<IRepo.ITopic> topicMock = GetMockRepoTopic(DCVID, RepoEnums.ElementType.MavimElementContainer);
            topicRepositoryMock.Setup(x => x.GetRootTopic()).ReturnsAsync(topicMock.Object);
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();
            TopicBusiness business = new TopicBusiness(topicRepositoryMock.Object, loggerMock.Object, featureManagerMock.Object);

            //Act
            ITopic result = await business.GetRootTopic();

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITopic>(result);
        }

        [Theory, MemberData(nameof(TopicValues))]
        [Trait("Category", "TopicBusiness")]
        public async Task GetTopic_ValidArguments_Topic(ITopic expected, IRepo.ITopic requested)
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<IRepo.ITopicRepository> topicRepositoryMock = new Mock<IRepo.ITopicRepository>();
            topicRepositoryMock.Setup(x => x.GetTopicByDcv(It.IsAny<string>())).ReturnsAsync(requested);
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();
            ITopicBusiness business = new TopicBusiness(topicRepositoryMock.Object, loggerMock.Object, featureManagerMock.Object);

            //Act
            ITopic result = await business.GetTopic(DCVID);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITopic>(result);
            Assert.True(IsEqual(result, expected));
        }

        [Theory, MemberData(nameof(RequestedTopicValues))]
        [Trait("Category", "TopicBusiness")]
        public async Task GetTopic_InValidArguments_Topic(IRepo.ITopic requested)
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<IRepo.ITopicRepository> topicRepositoryMock = new Mock<IRepo.ITopicRepository>();
            topicRepositoryMock.Setup(x => x.GetTopicByDcv(It.IsAny<string>())).ReturnsAsync(requested);
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();
            ITopicBusiness business = new TopicBusiness(topicRepositoryMock.Object, loggerMock.Object, featureManagerMock.Object);

            //Act
            Exception result = await Record.ExceptionAsync(async () => await business.GetTopic(FAULT_DCVID));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestException>(result);
        }


        [Theory, MemberData(nameof(DcvIdValues))]
        [Trait("Category", "TopicBusiness")]
        public async Task GetPathToRoot_ValidArguments_TopicPath(string dcvId)
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<IRepo.ITopicRepository> repositoryMock = new Mock<IRepo.ITopicRepository>();
            Mock<IRepo.ITopicPath> topicPathMock = new Mock<IRepo.ITopicPath>();
            topicPathMock.Setup(x => x.Path).Returns(new List<IRepo.IPathItem>());
            topicPathMock.Setup(x => x.Data).Returns(new List<IRepo.ITopic>());
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();
            TopicBusiness business = new TopicBusiness(repositoryMock.Object, loggerMock.Object, featureManagerMock.Object);
            repositoryMock.Setup(repository => repository.GetPathToRoot(dcvId)).ReturnsAsync(topicPathMock.Object);

            //Act
            var result = await business.GetPathToRoot(dcvId);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITopicPath>(result);
        }

        [Theory, MemberData(nameof(InvalidDcvIdValues))]
        [Trait("Category", "TopicBusiness")]
        public async Task GetPathToRoot_InvalidArguments_BadRequestException(string dcvId)
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<IRepo.ITopicRepository> repositoryMock = new Mock<IRepo.ITopicRepository>();
            Mock<IRepo.ITopicPath> topicPathMock = new Mock<IRepo.ITopicPath>();
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();
            TopicBusiness business = new TopicBusiness(repositoryMock.Object, loggerMock.Object, featureManagerMock.Object);
            repositoryMock.Setup(repository => repository.GetPathToRoot(It.IsAny<string>())).ReturnsAsync(topicPathMock.Object);

            //Act
            var result = await Record.ExceptionAsync(async () => await business.GetPathToRoot(dcvId));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestException>(result);
        }

        [Theory, MemberData(nameof(ForbiddenTopicValues))]
        [Trait("Category", "TopicBusiness")]
        public async Task GetPathToRoot_InternalComponent_ForbiddenRequestException(RepoEnums.ElementType elementType, bool isInternal)
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = new Mock<IFeatureManager>();
            featureManagerMock.Setup(x => x.IsEnabledAsync(
                It.IsAny<string>())).Returns(Task.FromResult(true));

            Mock<IRepo.ITopicRepository> repositoryMock = new Mock<IRepo.ITopicRepository>();
            Mock<IRepo.ITopicPath> topicPathMock = new Mock<IRepo.ITopicPath>();
            Mock<IRepo.IPathItem> pathItemMock = new Mock<IRepo.IPathItem>();
            Mock<IRepo.ITopic> topicMock = GetMockRepoTopic(DCVID, elementType, isInternal);
            pathItemMock.Setup(x => x.DcvId).Returns(DCVID);
            topicPathMock.Setup(x => x.Path).Returns(new List<IRepo.IPathItem> { pathItemMock.Object });
            topicPathMock.Setup(x => x.Data).Returns(new List<IRepo.ITopic>() { topicMock.Object });
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();
            TopicBusiness business = new TopicBusiness(repositoryMock.Object, loggerMock.Object, featureManagerMock.Object);
            repositoryMock.Setup(repository => repository.GetPathToRoot(DCVID)).ReturnsAsync(topicPathMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.GetPathToRoot(DCVID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }

        [Fact]
        [Trait("Category", "TopicBusiness")]
        public async Task GetExternalReferencesTopic_HasNoChildrenAfterBusinessLogic_Topic()
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();

            Mock<IRepo.ITopic> parentTopicMock = GetMockRepoTopic(dcvid: DCVID_V1, elementType: RepoEnums.ElementType.Unknown, hasChildren: true, isExternalReferencesRoot: true);
            Mock<IRepo.ITopic> childTopicMock = GetMockRepoTopic(dcvid: DCVID_V1, elementType: RepoEnums.ElementType.Unknown, isInternal: true);

            Mock<IRepo.ITopicRepository> topicRepositoryMock = new Mock<IRepo.ITopicRepository>();
            topicRepositoryMock.Setup(x => x.GetTopicByDcv(It.IsAny<string>())).ReturnsAsync(parentTopicMock.Object);
            topicRepositoryMock.Setup(x => x.GetChildrenByDcv(It.IsAny<string>())).ReturnsAsync(new List<IRepo.ITopic>() { childTopicMock.Object });
            TopicBusiness business = new TopicBusiness(topicRepositoryMock.Object, loggerMock.Object, featureManagerMock.Object);

            //Act
            ITopic result = await business.GetTopic(DCVID);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.HasChildren, "Topic should not have children");
            Assert.DoesNotContain(Enums.TopicResource.SubTopics, result.Resource);
            Assert.IsAssignableFrom<ITopic>(result);
        }

        [Fact]
        [Trait("Category", "TopicBusiness")]
        public async Task GetExternalReferencesTopic_HasOneChildAfterBusinessLogic_Topic()
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();

            Mock<IRepo.ITopic> parentTopicMock = GetMockRepoTopic(dcvid: DCVID_V1, elementType: RepoEnums.ElementType.Unknown, hasChildren: true, isExternalReferencesRoot: true);
            Mock<IRepo.ITopic> childTopicMock = GetMockRepoTopic(dcvid: DCVID_V1, elementType: RepoEnums.ElementType.Unknown, isInternal: true);
            Mock<IRepo.ITopic> child2TopicMock = GetMockRepoTopic(dcvid: DCVID_V1, elementType: RepoEnums.ElementType.Unknown, isInternal: false);

            Mock<IRepo.ITopicRepository> topicRepositoryMock = new Mock<IRepo.ITopicRepository>();
            topicRepositoryMock.Setup(x => x.GetTopicByDcv(It.IsAny<string>())).ReturnsAsync(parentTopicMock.Object);
            topicRepositoryMock.Setup(x => x.GetChildrenByDcv(It.IsAny<string>())).ReturnsAsync(new List<IRepo.ITopic>() {
                childTopicMock.Object,
                child2TopicMock.Object
            });

            TopicBusiness business = new TopicBusiness(topicRepositoryMock.Object, loggerMock.Object, featureManagerMock.Object);

            //Act
            ITopic result = await business.GetTopic(DCVID);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.HasChildren, "Topic should have children");
            Assert.Contains(Enums.TopicResource.SubTopics, result.Resource);
            Assert.IsAssignableFrom<ITopic>(result);
        }

        [Fact]
        [Trait("Category", "TopicBusiness")]
        public async Task GetExternalReferencesSiblingsTopic_HasNoChildrenAfterBusinessLogic_Topic()
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();

            Mock<IRepo.ITopic> topicMock = GetMockRepoTopic(dcvid: DCVID_V1, elementType: RepoEnums.ElementType.Unknown, hasChildren: true, isExternalReferencesRoot: true);

            Mock<IRepo.ITopicRepository> topicRepositoryMock = new Mock<IRepo.ITopicRepository>();
            topicRepositoryMock.Setup(x => x.GetTopicByDcv(It.IsAny<string>())).ReturnsAsync(topicMock.Object);
            topicRepositoryMock.Setup(x => x.GetSiblingsByDcv(It.IsAny<string>())).ReturnsAsync(new List<IRepo.ITopic>() { topicMock.Object });
            TopicBusiness business = new TopicBusiness(topicRepositoryMock.Object, loggerMock.Object, featureManagerMock.Object);

            //Act
            IEnumerable<ITopic> results = await business.GetSiblings(DCVID);

            //Assert
            Assert.NotNull(results);
            Assert.False(results.FirstOrDefault().HasChildren, "Topic should not have children");
            Assert.DoesNotContain(Enums.TopicResource.SubTopics, results.FirstOrDefault().Resource);
            Assert.IsAssignableFrom<IEnumerable<ITopic>>(results);
        }

        [Fact]
        [Trait("Category", "TopicBusiness")]
        public async Task GetExternalReferencesPathTopic_HasNoChildrenAfterBusinessLogic_Topic()
        {
            //Arrange
            Mock<IFeatureManager> featureManagerMock = GetMockFeatureManagerAllActiveFeatures();
            Mock<ILogger<TopicBusiness>> loggerMock = new Mock<ILogger<TopicBusiness>>();

            Mock<IRepo.ITopic> topicMock = GetMockRepoTopic(dcvid: DCVID, elementType: RepoEnums.ElementType.Unknown, hasChildren: true, isExternalReferencesRoot: true);
            Mock<IRepo.IPathItem> pathItemMock = new Mock<IRepo.IPathItem>();
            pathItemMock.Setup(x => x.Order).Returns(0);
            pathItemMock.Setup(x => x.DcvId).Returns(DCVID);

            Mock<IRepo.ITopicPath> topicPathMock = new Mock<IRepo.ITopicPath>();
            topicPathMock.Setup(x => x.Data).Returns(new List<IRepo.ITopic>() { topicMock.Object });
            topicPathMock.Setup(x => x.Path).Returns(new List<IRepo.IPathItem>() { pathItemMock.Object });

            Mock<IRepo.ITopicRepository> topicRepositoryMock = new Mock<IRepo.ITopicRepository>();
            topicRepositoryMock.Setup(x => x.GetPathToRoot(It.IsAny<string>())).ReturnsAsync(topicPathMock.Object);
            TopicBusiness business = new TopicBusiness(topicRepositoryMock.Object, loggerMock.Object, featureManagerMock.Object);

            //Act
            ITopicPath result = await business.GetPathToRoot(DCVID);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Data.FirstOrDefault().HasChildren, "Topic should not have children");
            Assert.DoesNotContain(Enums.TopicResource.SubTopics, result.Data.FirstOrDefault().Resource);
            Assert.IsAssignableFrom<ITopicPath>(result);
        }

        public static IEnumerable<object[]> DcvIdValues
        {
            get
            {
                yield return new object[] { "d0c2v0" };
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

        public static IEnumerable<object[]> ForbiddenTopicValues
        {
            get
            {
                yield return new object[] { RepoEnums.ElementType.Object, false };
                yield return new object[] { RepoEnums.ElementType.Virtual, true };
            }
        }

        public static IEnumerable<object[]> TopicValues
        {
            get
            {
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, false).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID_V1, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID_V1, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, true, false, false, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, true, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, true, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, true, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, true, false, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, true, false, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, true, false, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, true, false, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, true, false).Object };
                yield return new object[] { GetMockBusinessTopic(DCVID, Enums.ElementType.Where, true).Object, GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, false, true).Object };
            }
        }

        public static IEnumerable<object[]> RequestedTopicValues
        {
            get
            {
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID_V1, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, true, false, false, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, true, false, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, true, false, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, true, false, false, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, true, false, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, true, false, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, true, false, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, true, false, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, true, false).Object };
                yield return new object[] { GetMockRepoTopic(DCVID, RepoEnums.ElementType.Where, false, false, false, false, false, false, false, false, false, false, false, true).Object };
            }
        }

        private bool IsEqual(ITopic actual, ITopic expected) =>
            (actual.Dcv == expected.Dcv.ToString() &&
            actual.Parent == expected.Parent &&
            actual.Name == expected.Name &&
            actual.Icon == expected.Icon &&
            actual.IsChart == expected.IsChart &&
            actual.IsReadOnly == expected.IsReadOnly &&
            actual.HasChildren == expected.HasChildren &&
            actual.OrderNumber == expected.OrderNumber &&
            actual.ElementType == expected.ElementType);

        /// <summary>
        /// Mock object of the IFeatureManager where all features are active.
        /// </summary>
        /// <returns></returns>
        private static Mock<IFeatureManager> GetMockFeatureManagerAllActiveFeatures()
        {
            Mock<IFeatureManager> featureManagerMock = new Mock<IFeatureManager>();

            featureManagerMock.Setup(x => x.IsEnabledAsync(
                It.IsAny<string>())).Returns(Task.FromResult(true));

            return featureManagerMock;
        }

        private static Mock<IRepo.ITopic> GetMockRepoTopic(string dcvid, RepoEnums.ElementType elementType, bool isInternal = false, bool isDeleted = false, bool isChart = false, bool hasSystemName = false, bool isImportedVersionsRoot = false, bool isImportedVersion = false, bool isCreatedVersionsRoot = false, bool isCreatedVersion = false, bool isRecycleBin = false, bool isRelationshipsCategoriesRoot = false, bool isExternalReferencesRoot = false, bool isObjectsRoot = false, bool hasChildren = false)
        {
            Mock<IRepo.ITopic> topicMock = new Mock<IRepo.ITopic>();
            topicMock.Setup(x => x.Dcv).Returns(DcvId.FromDcvKey(dcvid));
            topicMock.Setup(x => x.Parent).Returns(dcvid);
            topicMock.Setup(x => x.Name).Returns("name");
            topicMock.Setup(x => x.Icon).Returns("icon");
            topicMock.Setup(x => x.IsChart).Returns(isChart);
            topicMock.Setup(x => x.OrderNumber).Returns(1);
            topicMock.Setup(x => x.ElementType).Returns(elementType);
            topicMock.Setup(x => x.IsInternal).Returns(isInternal);
            topicMock.Setup(x => x.IsInRecycleBin).Returns(isDeleted);
            topicMock.Setup(x => x.Resource).Returns(new List<RepoEnums.TopicResource>()).Verifiable();
            topicMock.Setup(x => x.Type).Returns(GetTopicTypes(hasSystemName, isImportedVersionsRoot, isImportedVersion, isCreatedVersionsRoot, isCreatedVersion, isRecycleBin, isRelationshipsCategoriesRoot, isExternalReferencesRoot, isObjectsRoot).Object);
            topicMock.SetupProperty(x => x.HasChildren);

            if (hasChildren) topicMock.Object.Resource.Add(RepoEnums.TopicResource.SubTopics);
            if (hasChildren) topicMock.Object.HasChildren = true;

            return topicMock;
        }

        private static Mock<IRepo.ITopic> GetMockRepoTopicInternalTopic(string dcvid)
        {
            Mock<IRepo.ITopic> topicMock = new Mock<IRepo.ITopic>();

            topicMock.Setup(x => x.Dcv).Returns(DcvId.FromDcvKey(dcvid));
            topicMock.Setup(x => x.IsInternal).Returns(true);
            topicMock.Setup(x => x.ElementType).Returns(RepoEnums.ElementType.Where);

            return topicMock;
        }

        private static Mock<IRepo.ITopic> GetMockRepoTopicObjectElementType(string dcvid)
        {
            Mock<IRepo.ITopic> topicMock = new Mock<IRepo.ITopic>();

            topicMock.Setup(x => x.Dcv).Returns(DcvId.FromDcvKey(dcvid));
            topicMock.Setup(x => x.IsInternal).Returns(false);
            topicMock.Setup(x => x.ElementType).Returns(RepoEnums.ElementType.Object);

            return topicMock;
        }

        private static Mock<IRepo.ITopic> GetMockRepoTopicWithNonPublicChildren(string dcvid)
        {
            Mock<IRepo.ITopic> topicMock = new Mock<IRepo.ITopic>();
            topicMock.Setup(x => x.Dcv).Returns(DcvId.FromDcvKey(dcvid));
            topicMock.Setup(x => x.IsInternal).Returns(false);
            topicMock.Setup(x => x.Resource).Returns(new List<RepoEnums.TopicResource>() { RepoEnums.TopicResource.SubTopics });



            return topicMock;
        }

        private static Mock<IRepo.IType> GetTopicTypes(bool hasSystemName, bool isImportedVersionsRoot, bool isImportedVersion, bool isCreatedVersionsRoot, bool isCreatedVersion, bool isRecycleBin, bool isRelationshipsCategoriesRoot, bool isExternalReferencesRoot, bool isObjectsRoot)
        {
            Mock<IRepo.IType> type = new Mock<IRepo.IType>();
            type.Setup(x => x.HasSystemName).Returns(hasSystemName);
            type.Setup(x => x.IsImportedVersionsRoot).Returns(isImportedVersionsRoot);
            type.Setup(x => x.IsImportedVersion).Returns(isImportedVersion);
            type.Setup(x => x.IsCreatedVersionsRoot).Returns(isCreatedVersionsRoot);
            type.Setup(x => x.IsCreatedVersion).Returns(isCreatedVersion);
            type.Setup(x => x.IsRecycleBin).Returns(isRecycleBin);
            type.Setup(x => x.IsRelationshipsCategoriesRoot).Returns(isRelationshipsCategoriesRoot);
            type.Setup(x => x.IsExternalReferencesRoot).Returns(isExternalReferencesRoot);
            type.Setup(x => x.IsObjectsRoot).Returns(isObjectsRoot);

            return type;
        }

        private static Mock<ITopic> GetMockBusinessTopic(string dcvid, Enums.ElementType elementType, bool isReadOnly, bool isChart = false)
        {
            Mock<ITopic> topicMock = new Mock<ITopic>();
            topicMock.Setup(x => x.Dcv).Returns(dcvid);
            topicMock.Setup(x => x.Parent).Returns(dcvid);
            topicMock.Setup(x => x.Name).Returns("name");
            topicMock.Setup(x => x.Icon).Returns("icon");
            topicMock.Setup(x => x.IsChart).Returns(isChart);
            topicMock.Setup(x => x.HasChildren).Returns(false);
            topicMock.Setup(x => x.OrderNumber).Returns(1);
            topicMock.Setup(x => x.ElementType).Returns(elementType);
            topicMock.Setup(x => x.IsReadOnly).Returns(isReadOnly);

            return topicMock;
        }
    }
}
