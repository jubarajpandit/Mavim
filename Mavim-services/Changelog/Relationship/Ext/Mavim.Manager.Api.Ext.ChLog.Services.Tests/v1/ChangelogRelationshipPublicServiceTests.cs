using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Interfaces;
using Mavim.Manager.Api.Ext.ChLog.Services.v1;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ILibChangelog = Mavim.Libraries.Changelog.Interfaces;
using ILibChangelogEnum = Mavim.Libraries.Changelog.Enums;

namespace Mavim.Manager.Api.Ext.ChLog.Services.Tests.v1
{
    public class ChangelogRelationshipPublicServiceTests
    {
        #region Private Members
        private Mock<ILibChangelog.IChangelogRelationshipClient> _mockClient;
        private ChangelogRelationshipPublicService _relationshipService;
        private Mock<ILogger<ChangelogRelationshipPublicService>> _mockLogger;
        #endregion

        #region GetRelations tests
        [Theory, MemberData(nameof(InvalidDbIdAndTopicIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetRelations_OneOrMoreEmptyArguments_BadRequestException(Guid dbId, string topicId)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _relationshipService.GetRelations(dbId, topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetRelations_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string topicId = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogRelationship> mock = GetChangelogRelationshipMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            IEnumerable<ILibChangelog.IChangelogRelationship> mockList = new List<ILibChangelog.IChangelogRelationship> { mock.Object };
            Services.v1.Models.ChangelogRelation expected = GetResponseChangelogRelationship(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.GetRelations(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(mockList));

            // Act
            IEnumerable<IChangelogRelationship> result = await _relationshipService.GetRelations(dbid, topicId);

            // Assert
            _mockClient.Verify(mock => mock.GetRelations(dbid, topicId), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result.FirstOrDefault());
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }
        #endregion

        #region GetPendingRelations tests
        [Theory, MemberData(nameof(InvalidDbIdAndTopicIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetPendingRelations_OneOrMoreEmptyArguments_BadRequestException(Guid dbId, string topicId)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _relationshipService.GetPendingRelations(dbId, topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetPendingRelations_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string topicId = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogRelationship> mock = GetChangelogRelationshipMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            IEnumerable<ILibChangelog.IChangelogRelationship> mockList = new List<ILibChangelog.IChangelogRelationship> { mock.Object };
            Services.v1.Models.ChangelogRelation expected = GetResponseChangelogRelationship(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.GetPendingRelations(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(mockList));

            // Act
            IEnumerable<IChangelogRelationship> result = await _relationshipService.GetPendingRelations(dbid, topicId);

            // Assert
            _mockClient.Verify(mock => mock.GetPendingRelations(dbid, topicId), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result.FirstOrDefault());
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }
        #endregion

        #region GetAllPendingRelations tests
        [Theory, MemberData(nameof(InvalidDbIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetAllPendingRelations_OneOrMoreEmptyArguments_BadRequestException(Guid dbId)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _relationshipService.GetAllPendingRelations(dbId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetAllPendingRelations_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            Mock<ILibChangelog.IChangelogRelationship> mock = GetChangelogRelationshipMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            IEnumerable<ILibChangelog.IChangelogRelationship> mockList = new List<ILibChangelog.IChangelogRelationship> { mock.Object };
            Services.v1.Models.ChangelogRelation expected = GetResponseChangelogRelationship(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.GetAllPendingRelations(It.IsAny<Guid>())).Returns(() => Task.FromResult(mockList));

            // Act
            IEnumerable<IChangelogRelationship> result = await _relationshipService.GetAllPendingRelations(dbId);

            // Assert
            _mockClient.Verify(mock => mock.GetAllPendingRelations(dbId), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result.FirstOrDefault());
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }
        #endregion

        #region GetRelationStatus tests
        [Theory, MemberData(nameof(InvalidDbIdAndTopicIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetRelationStatus_OneOrMoreEmptyArguments_BadRequestException(Guid dbId, string topicId)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _relationshipService.GetRelationStatus(dbId, topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetRelationStatus_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string topicId = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogRelationship> mock = GetChangelogRelationshipMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            Services.v1.Models.ChangelogRelation expected = GetResponseChangelogRelationship(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.GetRelationStatus(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(mock.Object.Status));

            // Act
            ChangeStatus result = await _relationshipService.GetRelationStatus(dbId, topicId);

            // Assert
            _mockClient.Verify(mock => mock.GetRelationStatus(dbId, topicId), Times.Once);
            ChangeStatus mockStatus = Map(mock.Object.Status);
            Assert.True(result == mockStatus);
        }
        #endregion

        #region ApproveRelation tests
        [Theory, MemberData(nameof(InvalidDbIdAndTopicIdAndRelationIdCombinations))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task ApproveRelation_OneOrMoreEmptyArguments_BadRequestException(Guid dbId, Guid changelogId)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _relationshipService.ApproveRelation(dbId, changelogId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task ApproveRelation_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            Guid changelogId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");

            Mock<ILibChangelog.IChangelogRelationship> mock = GetChangelogRelationshipMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            Services.v1.Models.ChangelogRelation expected = GetResponseChangelogRelationship(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.ApproveRelation(dbId, changelogId)).Returns(() => Task.FromResult(mock.Object));

            // Act
            IChangelogRelationship result = await _relationshipService.ApproveRelation(dbId, changelogId);

            // Assert
            _mockClient.Verify(changelogRelationshipClient => changelogRelationshipClient.ApproveRelation(dbId, changelogId), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result);
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }
        #endregion

        #region RejectRelation tests
        [Theory, MemberData(nameof(InvalidDbIdAndTopicIdAndRelationIdCombinations))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task RejectRelation_OneOrMoreEmptyArguments_BadRequestException(Guid dbId, Guid changelogId)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _relationshipService.RejectRelation(dbId, changelogId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task RejectRelation_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            Guid changelogId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            Mock<ILibChangelog.IChangelogRelationship> mock = GetChangelogRelationshipMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            Services.v1.Models.ChangelogRelation expected = GetResponseChangelogRelationship(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.RejectRelation(dbId, changelogId)).Returns(() => Task.FromResult(mock.Object));

            // Act
            IChangelogRelationship result = await _relationshipService.RejectRelation(dbId, changelogId);

            // Assert
            _mockClient.Verify(changelogRelationshipClient => changelogRelationshipClient.RejectRelation(dbId, changelogId), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result);
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }
        #endregion

        public static IEnumerable<object[]> InvalidDbIdValues
        {
            get
            {
                yield return new object[] { Guid.Empty };
                yield return new object[] { null };
            }
        }

        public static IEnumerable<object[]> InvalidDbIdAndTopicIdValues
        {
            get
            {
                yield return new object[] { Guid.Empty, "d12950883c414v0" };
                yield return new object[] { null, "d12950883c414v0" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), "" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), null };
            }
        }

        public static IEnumerable<object[]> InvalidDbIdAndTopicIdAndRelationIdCombinations
        {
            get
            {
                yield return new object[] { Guid.Empty, Guid.NewGuid() };
                yield return new object[] { Guid.NewGuid(), Guid.Empty };
            }
        }

        private Mock<ILibChangelog.IChangelogRelationship> GetChangelogRelationshipMock()
        {
            Mock<ILibChangelog.IChangelogRelationship> mock = new Mock<ILibChangelog.IChangelogRelationship>();
            mock.SetupGet(p => p.ChangelogId).Returns(new Guid());
            mock.SetupGet(p => p.InitiatorUserEmail).Returns("InitiatorUserEmail");
            mock.SetupGet(p => p.ReviewerUserEmail).Returns("ReviewerUserEmail");
            mock.SetupGet(p => p.TimestampChanged).Returns(DateTime.Today);
            mock.SetupGet(p => p.TimestampApproved).Returns(DateTime.Today);
            mock.SetupGet(p => p.TopicDcv).Returns("d12950883c414v0");
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Pending);
            mock.SetupGet(p => p.FromCategory).Returns("FromCategory");
            mock.SetupGet(p => p.FromTopicDcv).Returns("FromTopicDcv");
            mock.SetupGet(p => p.ToCategory).Returns("ToCategory");
            mock.SetupGet(p => p.ToTopicDcv).Returns("ToTopicDcv");

            return mock;
        }

        private Services.v1.Models.ChangelogRelation GetResponseChangelogRelationship(ChangeStatus status) =>
            new Services.v1.Models.ChangelogRelation()
            {
                ChangelogId = new Guid(),
                InitiatorUserEmail = "InitiatorUserEmail",
                ReviewerUserEmail = "ReviewerUserEmail",
                TimestampChanged = DateTime.Today,
                TimestampApproved = DateTime.Today,
                TopicDcv = "d12950883c414v0",
                Status = status,
                FromCategory = "FromCategory",
                FromTopicDcv = "FromTopicDcv",
                ToCategory = "ToCategory",
                ToTopicDcv = "ToTopicDcv"
            };

        private static ChangeStatus Map(ILibChangelogEnum.ChangeStatus titleStatus)
        {
            return titleStatus switch
            {
                ILibChangelogEnum.ChangeStatus.Pending => ChangeStatus.Pending,
                ILibChangelogEnum.ChangeStatus.Approved => ChangeStatus.Approved,
                ILibChangelogEnum.ChangeStatus.Rejected => ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(titleStatus), titleStatus, null)
            };
        }

        #region Private Methods
        private void ArrangeCommonObjects()
        {
            _mockClient = new Mock<ILibChangelog.IChangelogRelationshipClient>();
            _mockLogger = new Mock<ILogger<ChangelogRelationshipPublicService>>();
            _relationshipService = new ChangelogRelationshipPublicService(_mockClient.Object, _mockLogger.Object);
        }
        #endregion
    }
}
