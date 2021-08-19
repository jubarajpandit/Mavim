using Mavim.Manager.Api.ChangelogTitle.Public.Controllers.v1;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.ChangelogTitle.Tests.Controllers.v1
{
    public class ChangelogTitlePublicControllerTests
    {
        #region Private Members
        private Mock<IChangelogTitlePublicService> _mockService;
        private ChangelogTitlePublicController _titleController;
        private Mock<ILogger<ChangelogTitlePublicController>> _mockLogger;
        #endregion

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task RejectTitle_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.RejectTitle(dbid, dcvid)).Returns(() => Task.FromResult(new Mock<IChangelogTitle>().Object));

            // Act
            ActionResult<IChangelogTitle> actionResult = await _titleController.RejectTitle(dbid, dcvid);

            // Assert
            _mockService.Verify(mock => mock.RejectTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IChangelogTitle title = okObjectResult.Value as IChangelogTitle;
            Assert.NotNull(title);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetTitleStatus_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            Mock<IChangelogTitle> mock = new Mock<IChangelogTitle>();
            mock.SetupGet(p => p.Status).Returns(ChangeStatus.Rejected);
            _mockService.Setup(x => x.GetTitleStatus(dbid, dcvid)).Returns(() => Task.FromResult(mock.Object.Status));

            // Act
            ActionResult<ChangeStatus> actionResult = await _titleController.GetTitleStatus(dbid, dcvid);

            // Assert
            _mockService.Verify(mock => mock.GetTitleStatus(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            ChangeStatus status = Map(okObjectResult.Value.ToString());
            Assert.True(status == mock.Object.Status);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetTitles_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.GetTitles(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(new Mock<IEnumerable<IChangelogTitle>>().Object));

            // Act
            ActionResult<IEnumerable<IChangelogTitle>> actionResult = await _titleController.GetTitles(dbid, dcvid);

            // Assert
            _mockService.Verify(mock => mock.GetTitles(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogTitle> titles = okObjectResult.Value as IEnumerable<IChangelogTitle>;
            Assert.NotNull(titles);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetPendingTitle_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.GetPendingTitle(dbid, dcvid)).Returns(() => Task.FromResult(new Mock<IChangelogTitle>().Object));

            // Act
            ActionResult<IChangelogTitle> actionResult = await _titleController.GetPendingTitle(dbid, dcvid);

            // Assert
            _mockService.Verify(mock => mock.GetPendingTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IChangelogTitle title = okObjectResult.Value as IChangelogTitle;
            Assert.NotNull(title);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task GetAllPendingTitles_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            _mockService.Setup(x => x.GetAllPendingTitles(dbid)).Returns(() => Task.FromResult(new Mock<IEnumerable<IChangelogTitle>>().Object));

            // Act
            ActionResult<IEnumerable<IChangelogTitle>> actionResult = await _titleController.GetAllPendingTitles(dbid);

            // Assert
            _mockService.Verify(mock => mock.GetAllPendingTitles(dbid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogTitle> titles = okObjectResult.Value as IEnumerable<IChangelogTitle>;
            Assert.NotNull(titles);
        }

        [Fact]
        [Trait("Category", "ChangelogTitlePublic")]
        public async Task ApproveTitle_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.ApproveTitle(dbid, dcvid)).Returns(() => Task.FromResult(new Mock<IChangelogTitle>().Object));

            // Act
            ActionResult<IChangelogTitle> actionResult = await _titleController.ApproveTitle(dbid, dcvid);

            // Assert
            _mockService.Verify(mock => mock.ApproveTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IChangelogTitle title = okObjectResult.Value as IChangelogTitle;
            Assert.NotNull(title);
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

        private static ChangeStatus Map(string titleStatus)
        {
            return titleStatus switch
            {
                "Pending" => ChangeStatus.Pending,
                "Approved" => ChangeStatus.Approved,
                "Rejected" => ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(titleStatus), titleStatus, null)
            };
        }

        #region Private Methods
        private void ArrangeCommonObjects()
        {
            _mockService = new Mock<IChangelogTitlePublicService>();
            _mockLogger = new Mock<ILogger<ChangelogTitlePublicController>>();
            _titleController = new ChangelogTitlePublicController(_mockService.Object, _mockLogger.Object);
        }
        #endregion
    }
}
