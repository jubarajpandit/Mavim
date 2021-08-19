using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Business.v1;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;
using RepoEnums = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.Businesss.Test.v1
{
    public class RelationBusinessTest
    {
        private const string DCVID = "d12950883c414v0";

        [Theory, MemberData(nameof(relationValues))]
        [Trait("Category", "RelationBusiness")]
        public async Task GetRelationships_ValidArguments_RelationshipList(IRepo.RelationShips.IRelationship request, IRelationship expected)
        {
            // Arrange
            Mock<ILogger<RelationshipBusiness>> loggerMock = new Mock<ILogger<RelationshipBusiness>>();
            Mock<IRepo.RelationShips.IRelationshipsRepository> repoMock = new Mock<IRepo.RelationShips.IRelationshipsRepository>();
            repoMock.Setup(x => x.GetRelationships(It.IsAny<string>())).ReturnsAsync(new List<IRepo.RelationShips.IRelationship>() { request });
            RelationshipBusiness relationshipBusiness = new RelationshipBusiness(repoMock.Object, loggerMock.Object);

            // Act
            IEnumerable<IRelationship> result = await relationshipBusiness.GetRelationships(DCVID);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IRelationship>(result.First());
            Assert.True(IsEqual(result.First(), expected));
        }

        [Theory, MemberData(nameof(InvalidRelationValues))]
        [Trait("Category", "RelationBusiness")]
        public async Task GetRelationships_InValidRelations_EmptyCollection(IRepo.RelationShips.IRelationship request)
        {
            // Arrange
            Mock<ILogger<RelationshipBusiness>> loggerMock = new Mock<ILogger<RelationshipBusiness>>();
            Mock<IRepo.RelationShips.IRelationshipsRepository> repoMock = new Mock<IRepo.RelationShips.IRelationshipsRepository>();
            repoMock.Setup(x => x.GetRelationships(It.IsAny<string>())).ReturnsAsync(new List<IRepo.RelationShips.IRelationship>() { request });
            RelationshipBusiness relationshipBusiness = new RelationshipBusiness(repoMock.Object, loggerMock.Object);

            // Act
            IEnumerable<IRelationship> result = await relationshipBusiness.GetRelationships(DCVID);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public static IEnumerable<object[]> InvalidDcvIdValues
        {
            get
            {
                yield return new object[] { "test" };
                yield return new object[] { "d12950abc883c414v0" };
            }
        }

        public static IEnumerable<object[]> InvalidRelationValues
        {
            get
            {
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.From, DCVID, "relationTopicName", "relationTopicIcon", false, true).Object };
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.From, DCVID, "relationTopicName", "relationTopicIcon", true, false).Object };
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.Unknown, DCVID, "relationTopicName", "relationTopicIcon", false, false).Object };
            }
        }

        public static IEnumerable<object[]> relationValues
        {
            get
            {
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.From, DCVID, "relationTopicName", "relationTopicIcon", false, false).Object, GetBusinessRelationMock("test", RelationshipType.Where, DCVID, "relationTopicName", "relationTopicIcon").Object };
            }
        }

        private bool IsEqual(IRelationship actual, IRelationship expected) =>
            (actual.Dcv == expected.Dcv &&
            actual.Category == expected.Category &&
            actual.RelationshipType == expected.RelationshipType &&
            actual.Icon == expected.Icon &&
            IsEqual(actual.UserInstruction, expected.UserInstruction) &&
            IsEqual(actual.DispatchInstructions.First(), expected.DispatchInstructions.First()) &&
            IsEqual(actual.Characteristic, expected.Characteristic) &&
            IsEqual(actual.WithElement, expected.WithElement) &&
            IsEqual(actual.WithElementParent, expected.WithElementParent));

        private bool IsEqual(IRelationshipElement actual, IRelationshipElement expected) =>
            (actual.Name == expected.Name &&
            actual.Icon == expected.Icon &&
            actual.Dcv == expected.Dcv);

        private bool IsEqual(ISimpleDispatchInstruction actual, ISimpleDispatchInstruction expected) =>
            (actual.TypeName == expected.TypeName &&
            actual.Name == expected.Name &&
            actual.Icon == expected.Icon &&
            actual.Dcv == expected.Dcv);

        private static Mock<IRepo.RelationShips.IRelationship> GetRepoRelationMock(string category, RepoEnums.RelationshipType relationshipType, RepoEnums.CategoryType categoryType, string relationTopicDcv, string relationTopicName, string relationTopicIcon, bool isAttributeRelation, bool isDestroyed)
        {
            Mock<IRepo.RelationShips.IRelationship> mock = new Mock<IRepo.RelationShips.IRelationship>();
            mock.Setup(x => x.Dcv).Returns(DCVID);
            mock.Setup(x => x.Category).Returns(category);
            mock.Setup(x => x.RelationshipType).Returns(relationshipType);
            mock.Setup(x => x.CategoryType).Returns(categoryType);
            mock.Setup(x => x.Icon).Returns("icon");
            mock.Setup(x => x.UserInstruction).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);
            mock.Setup(x => x.DispatchInstructions).Returns(new List<IRepo.RelationShips.ISimpleDispatchInstruction>() { (GetRepoDispatchInstruction("typename", "dcv", "name", "icon")).Object });
            mock.Setup(x => x.Characteristic).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);
            mock.Setup(x => x.WithElement).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);
            mock.Setup(x => x.WithElementParent).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);
            mock.Setup(x => x.IsAttributeRelation).Returns(isAttributeRelation);
            mock.Setup(x => x.IsDestroyed).Returns(isDestroyed);

            return mock;
        }

        private static Mock<IRepo.RelationShips.IRelationshipElement> GetRepoRelationshipTopicMock(string dcv, string name, string icon)
        {
            Mock<IRepo.RelationShips.IRelationshipElement> mock = new Mock<IRepo.RelationShips.IRelationshipElement>();
            mock.Setup(x => x.Dcv).Returns(dcv);
            mock.Setup(x => x.Name).Returns(name);
            mock.Setup(x => x.Icon).Returns(icon);

            return mock;
        }

        private static Mock<IRepo.RelationShips.ISimpleDispatchInstruction> GetRepoDispatchInstruction(string typeName, string dcv, string name, string icon)
        {
            Mock<IRepo.RelationShips.ISimpleDispatchInstruction> mock = new Mock<IRepo.RelationShips.ISimpleDispatchInstruction>();
            mock.Setup(x => x.TypeName).Returns(typeName);
            mock.Setup(x => x.Dcv).Returns(dcv);
            mock.Setup(x => x.Name).Returns(name);
            mock.Setup(x => x.Icon).Returns(icon);

            return mock;
        }

        private static Mock<IRelationship> GetBusinessRelationMock(string category, RelationshipType relationshipType, string relationTopicDcv, string relationTopicName, string relationTopicIcon)
        {
            Mock<IRelationship> mock = new Mock<IRelationship>();
            mock.Setup(x => x.Dcv).Returns(DCVID);
            mock.Setup(x => x.Category).Returns(category);
            mock.Setup(x => x.RelationshipType).Returns(relationshipType);
            mock.Setup(x => x.Icon).Returns("icon");
            mock.Setup(x => x.UserInstruction).Returns(GetBusinessRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);
            mock.Setup(x => x.DispatchInstructions).Returns(new List<ISimpleDispatchInstruction>() { (GetBusinessDispatchInstruction("typename", "dcv", "name", "icon")).Object });
            mock.Setup(x => x.Characteristic).Returns(GetBusinessRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);
            mock.Setup(x => x.WithElement).Returns(GetBusinessRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);
            mock.Setup(x => x.WithElementParent).Returns(GetBusinessRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon).Object);

            return mock;
        }

        private static Mock<IRelationshipElement> GetBusinessRelationshipTopicMock(string dcv, string name, string icon)
        {
            Mock<IRelationshipElement> mock = new Mock<IRelationshipElement>();
            mock.Setup(x => x.Dcv).Returns(dcv);
            mock.Setup(x => x.Name).Returns(name);
            mock.Setup(x => x.Icon).Returns(icon);

            return mock;
        }

        private static Mock<ISimpleDispatchInstruction> GetBusinessDispatchInstruction(string typeName, string dcv, string name, string icon)
        {
            Mock<ISimpleDispatchInstruction> mock = new Mock<ISimpleDispatchInstruction>();
            mock.Setup(x => x.TypeName).Returns(typeName);
            mock.Setup(x => x.Dcv).Returns(dcv);
            mock.Setup(x => x.Name).Returns(name);
            mock.Setup(x => x.Icon).Returns(icon);

            return mock;
        }
    }
}
