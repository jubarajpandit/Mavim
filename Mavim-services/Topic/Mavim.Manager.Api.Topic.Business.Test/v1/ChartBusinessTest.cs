using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.v1;
using Mavim.Manager.Api.Topic.Business.v1.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;
using RepoEnums = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.Businesss.Test.v1
{
    public class ChartBusinessTest
    {
        private const string DCVID = "d12950883c414v0";

        [Theory, MemberData(nameof(relationValuesToChart))]
        [Trait("Category", "RelationBusiness")]
        public async Task GetTopicCharts_ValidRelations_Charts(IRepo.RelationShips.IRelationship request, IEnumerable<IChart> expected)
        {
            // Arrange
            Mock<ILogger<ChartBusiness>> loggerMock = new Mock<ILogger<ChartBusiness>>();
            Mock<IRepo.RelationShips.IRelationshipsRepository> repoMock = new Mock<IRepo.RelationShips.IRelationshipsRepository>();
            repoMock.Setup(x => x.GetRelationships(It.IsAny<string>())).ReturnsAsync(new List<IRepo.RelationShips.IRelationship>() { request });
            ChartBusiness chartBusiness = new ChartBusiness(repoMock.Object, loggerMock.Object);

            // Act
            var result = await chartBusiness.GetTopicCharts(DCVID);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IChart>>(result);
            var resultString = JsonConvert.SerializeObject(result);
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }

        [Theory, MemberData(nameof(InvalidRelationValues))]
        [Trait("Category", "RelationBusiness")]
        public async Task GetTopicCharts_InValidRelations_EmptyCollection(IRepo.RelationShips.IRelationship request)
        {
            // Arrange
            Mock<ILogger<ChartBusiness>> loggerMock = new Mock<ILogger<ChartBusiness>>();
            Mock<IRepo.RelationShips.IRelationshipsRepository> repoMock = new Mock<IRepo.RelationShips.IRelationshipsRepository>();
            repoMock.Setup(x => x.GetRelationships(It.IsAny<string>())).ReturnsAsync(new List<IRepo.RelationShips.IRelationship>() { request });
            ChartBusiness chartBusiness = new ChartBusiness(repoMock.Object, loggerMock.Object);

            // Act
            var result = await chartBusiness.GetTopicCharts(DCVID);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IChart>>(result);
            Assert.Empty(result);
        }

        [Theory, MemberData(nameof(InvalidDcvIdValues))]
        [Trait("Category", "RelationBusiness")]
        public async Task GetTopicCharts_InvalidDcvIdValues_BadRequestException(string dcvId)
        {
            // Arrange
            Mock<ILogger<ChartBusiness>> loggerMock = new Mock<ILogger<ChartBusiness>>();
            Mock<IRepo.RelationShips.IRelationshipsRepository> repoMock = new Mock<IRepo.RelationShips.IRelationshipsRepository>();
            ChartBusiness chartBusiness = new ChartBusiness(repoMock.Object, loggerMock.Object);

            //Act
            var result = await Record.ExceptionAsync(async () => await chartBusiness.GetTopicCharts(dcvId));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestException>(result);
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
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.From, DCVID, "relationTopicName", "relationTopicIcon", false, true, true).Object };
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.From, DCVID, "relationTopicName", "relationTopicIcon", true, false, true).Object };
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.Unknown, DCVID, "relationTopicName", "relationTopicIcon", false, false, true).Object };
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.Chart, DCVID, "relationTopicName", "relationTopicIcon", false, false, false).Object };
            }
        }

        public static IEnumerable<object[]> FeatureFlagRelationValues
        {
            get
            {
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.Chart, DCVID, "relationTopicName", "relationTopicIcon", false, false, false).Object };
            }
        }

        public static IEnumerable<object[]> relationValuesToChart
        {
            get
            {
                yield return new object[] { GetRepoRelationMock("test", RepoEnums.RelationshipType.Where, RepoEnums.CategoryType.Chart, DCVID, "relationTopicName", "relationTopicIcon", false, false, true).Object, new List<IChart> { new Chart() { Dcv = DCVID, Name = "relationTopicName" } } };
            }
        }

        private static Mock<IRepo.RelationShips.IRelationship> GetRepoRelationMock(string category, RepoEnums.RelationshipType relationshipType, RepoEnums.CategoryType categoryType, string relationTopicDcv, string relationTopicName, string relationTopicIcon, bool isAttributeRelation, bool isDestroyed, bool isPublic)
        {
            Mock<IRepo.RelationShips.IRelationship> mock = new Mock<IRepo.RelationShips.IRelationship>();
            mock.Setup(x => x.Dcv).Returns(DCVID);
            mock.Setup(x => x.Category).Returns(category);
            mock.Setup(x => x.RelationshipType).Returns(relationshipType);
            mock.Setup(x => x.CategoryType).Returns(categoryType);
            mock.Setup(x => x.Icon).Returns("icon");
            mock.Setup(x => x.UserInstruction).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon, isPublic).Object);
            mock.Setup(x => x.DispatchInstructions).Returns(new List<IRepo.RelationShips.ISimpleDispatchInstruction>() { (GetRepoDispatchInstruction("typename", "dcv", "name", "icon")).Object });
            mock.Setup(x => x.Characteristic).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon, isPublic).Object);
            mock.Setup(x => x.WithElement).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon, isPublic).Object);
            mock.Setup(x => x.WithElementParent).Returns(GetRepoRelationshipTopicMock(relationTopicDcv, relationTopicName, relationTopicIcon, isPublic).Object);
            mock.Setup(x => x.IsAttributeRelation).Returns(isAttributeRelation);
            mock.Setup(x => x.IsDestroyed).Returns(isDestroyed);

            return mock;
        }

        private static Mock<IRepo.RelationShips.IRelationshipElement> GetRepoRelationshipTopicMock(string dcv, string name, string icon, bool isPublic)
        {
            Mock<IRepo.RelationShips.IRelationshipElement> mock = new Mock<IRepo.RelationShips.IRelationshipElement>();
            mock.Setup(x => x.Dcv).Returns(dcv);
            mock.Setup(x => x.Name).Returns(name);
            mock.Setup(x => x.Icon).Returns(icon);
            mock.Setup(x => x.IsPublic).Returns(isPublic);

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
    }
}
