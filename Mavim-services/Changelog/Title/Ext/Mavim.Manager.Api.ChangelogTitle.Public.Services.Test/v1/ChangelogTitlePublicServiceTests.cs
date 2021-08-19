using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Interfaces;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.v1;
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

namespace Mavim.Manager.Api.ChangelogTitle.Tests.Controllers.v1
{
    public class ChangelogTitlePublicControllerTests
    {
        #region Private Members
        private Mock<ILibChangelog.IChangelogTitleClient> _mockClient;
        private ChangelogTitlePublicService _titleService;
        private Mock<ILogger<ChangelogTitlePublicService>> _mockLogger;
        #endregion

        [Theory, MemberData(nameof(DbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task RejectTitle_OneOrMoreEmptyArguments_BadRequestException(Guid dbid, string dcvid)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _titleService.RejectTitle(dbid, dcvid));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task RejectTitle_ValidArguments_ChangelogTitle()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogTitle> mock = GetChangelogTitleMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            Public.Services.v1.Models.ChangelogTitle expected = GetResponseChangelogTitle(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.RejectTitle(dbid, dcvid)).Returns(() => Task.FromResult(mock.Object));

            // Act
            IChangelogTitle result = await _titleService.RejectTitle(dbid, dcvid);

            // Assert
            _mockClient.Verify(mock => mock.RejectTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result);
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetTitleStatus_ValidArguments_ChangeStatus()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogTitle> mock = new Mock<ILibChangelog.IChangelogTitle>();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            _mockClient.Setup(x => x.GetTitleStatus(dbid, dcvid)).Returns(() => Task.FromResult(mock.Object.Status));

            // Act
            ChangeStatus result = await _titleService.GetTitleStatus(dbid, dcvid);

            // Assert
            _mockClient.Verify(mock => mock.GetTitleStatus(dbid, dcvid), Times.Once);
            ChangeStatus mockStatus = Map(mock.Object.Status);
            Assert.True(result == mockStatus);
        }

        [Theory, MemberData(nameof(DbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetTitles_OneOrMoreEmptyArguments_BadRequestException(Guid dbid, string dcvid)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _titleService.GetTitles(dbid, dcvid));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetTitles_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogTitle> mock = GetChangelogTitleMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            IEnumerable<ILibChangelog.IChangelogTitle> mockList = new List<ILibChangelog.IChangelogTitle>() { mock.Object };
            Public.Services.v1.Models.ChangelogTitle expected = GetResponseChangelogTitle(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.GetTitles(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(mockList));

            // Act
            IEnumerable<IChangelogTitle> result = await _titleService.GetTitles(dbid, dcvid);

            // Assert
            _mockClient.Verify(mock => mock.GetTitles(dbid, dcvid), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result.FirstOrDefault());
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }

        [Theory, MemberData(nameof(DbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetPendingTitle_OneOrMoreEmptyArguments_BadRequestException(Guid dbid, string dcvid)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _titleService.GetPendingTitle(dbid, dcvid));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetPendingTitle_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogTitle> mock = GetChangelogTitleMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Pending);
            Public.Services.v1.Models.ChangelogTitle expected = GetResponseChangelogTitle(ChangeStatus.Pending);
            _mockClient.Setup(x => x.GetPendingTitle(dbid, dcvid)).Returns(() => Task.FromResult(mock.Object));

            // Act
            IChangelogTitle result = await _titleService.GetPendingTitle(dbid, dcvid);

            // Assert
            _mockClient.Verify(mock => mock.GetPendingTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result);
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }

        [Theory, MemberData(nameof(DbIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetAllPendingTitles_OneOrMoreEmptyArguments_BadRequestException(Guid dbid)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _titleService.GetAllPendingTitles(dbid));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetAllPendingTitles_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            Mock<ILibChangelog.IChangelogTitle> mock = GetChangelogTitleMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            IEnumerable<ILibChangelog.IChangelogTitle> mockList = new List<ILibChangelog.IChangelogTitle>() { mock.Object };
            Public.Services.v1.Models.ChangelogTitle expected = GetResponseChangelogTitle(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.GetAllPendingTitles(dbid)).Returns(() => Task.FromResult(mockList));

            // Act
            IEnumerable<IChangelogTitle> result = await _titleService.GetAllPendingTitles(dbid);

            // Assert
            _mockClient.Verify(mock => mock.GetAllPendingTitles(dbid), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result.FirstOrDefault());
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }

        [Theory, MemberData(nameof(DbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task ApproveTitle_OneOrMoreEmptyArguments_BadRequestException(Guid dbid, string dcvid)
        {
            // Arrange
            ArrangeCommonObjects();

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await _titleService.ApproveTitle(dbid, dcvid));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task ApproveTitle_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            Mock<ILibChangelog.IChangelogTitle> mock = GetChangelogTitleMock();
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Rejected);
            Public.Services.v1.Models.ChangelogTitle expected = GetResponseChangelogTitle(ChangeStatus.Rejected);
            _mockClient.Setup(x => x.ApproveTitle(dbid, dcvid)).Returns(() => Task.FromResult(mock.Object));

            // Act
            IChangelogTitle result = await _titleService.ApproveTitle(dbid, dcvid);

            // Assert
            _mockClient.Verify(mock => mock.ApproveTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(result);
            var resultString = JsonConvert.SerializeObject(result);
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }

        public static IEnumerable<object[]> DbIdValues
        {
            get
            {
                yield return new object[] { Guid.Empty };
            }
        }

        public static IEnumerable<object[]> DbIdAndDcvIdValues
        {
            get
            {
                yield return new object[] { Guid.Empty, "d12950883c414v0" };
                yield return new object[] { null, "d12950883c414v0" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), "" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), null };
            }
        }

        private Mock<ILibChangelog.IChangelogTitle> GetChangelogTitleMock()
        {
            Mock<ILibChangelog.IChangelogTitle> mock = new Mock<ILibChangelog.IChangelogTitle>();
            mock.SetupGet(p => p.ChangelogId).Returns(1);
            mock.SetupGet(p => p.InitiatorUserEmail).Returns("InitiatorUserEmail");
            mock.SetupGet(p => p.ReviewerUserEmail).Returns("ReviewerUserEmail");
            mock.SetupGet(p => p.TimestampChanged).Returns(DateTime.Today);
            mock.SetupGet(p => p.TimestampApproved).Returns(DateTime.Today);
            mock.SetupGet(p => p.TopicDcv).Returns("d12950883c414v0");
            mock.SetupGet(p => p.Status).Returns(ILibChangelogEnum.ChangeStatus.Pending);
            mock.SetupGet(p => p.FromTitleValue).Returns("FromTitleValue");
            mock.SetupGet(p => p.ToTitleValue).Returns("ToTitleValue");

            return mock;
        }

        private Public.Services.v1.Models.ChangelogTitle GetResponseChangelogTitle(ChangeStatus status) =>
            new Public.Services.v1.Models.ChangelogTitle()
            {
                ChangelogId = 1,
                InitiatorUserEmail = "InitiatorUserEmail",
                ReviewerUserEmail = "ReviewerUserEmail",
                TimestampChanged = DateTime.Today,
                TimestampApproved = DateTime.Today,
                TopicDcv = "d12950883c414v0",
                Status = status,
                FromTitleValue = "FromTitleValue",
                ToTitleValue = "ToTitleValue",
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
            _mockClient = new Mock<ILibChangelog.IChangelogTitleClient>();
            _mockLogger = new Mock<ILogger<ChangelogTitlePublicService>>();
            _titleService = new ChangelogTitlePublicService(_mockClient.Object, _mockLogger.Object);
        }
        #endregion
    }
}
