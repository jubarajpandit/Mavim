using Mavim.Manager.Api.ChangelogTitle.Controllers.v1;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.ChangelogTitle.Tests.Controllers.v1
{
    public class TitleControllerTests
    {
        #region Private Members
        private Mock<ITitleService> _mockService;
        private TitleController _titleController;
        private Mock<ILogger<TitleController>> _mockLogger;
        #endregion

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task RejectTitle_ValidArguments_OkObjectResult()
        {
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.RejectTitle(dbid, dcvid)).Returns(() => Task.FromResult(new Mock<ITitle>().Object));

            ActionResult<ITitle> actionResult = await _titleController.RejectTitle(dbid, dcvid);

            _mockService.Verify(mock => mock.RejectTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            ITitle title = okObjectResult.Value as ITitle;
            Assert.NotNull(title);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetTitles_ValidArguments_OkObjectResult()
        {
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.GetTitles(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(new Mock<IEnumerable<ITitle>>().Object));

            ActionResult<IEnumerable<ITitle>> actionResult = await _titleController.GetTitles(dbid, dcvid);

            _mockService.Verify(mock => mock.GetTitles(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<ITitle> titles = okObjectResult.Value as IEnumerable<ITitle>;
            Assert.NotNull(titles);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetPendingTitle_ValidArguments_OkObjectResult()
        {
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.GetPendingTitle(dbid, dcvid)).Returns(() => Task.FromResult(new Mock<ITitle>().Object));

            ActionResult<ITitle> actionResult = await _titleController.GetPendingTitle(dbid, dcvid);

            _mockService.Verify(mock => mock.GetPendingTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            ITitle title = okObjectResult.Value as ITitle;
            Assert.NotNull(title);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetAllPendingTitles_ValidArguments_OkObjectResult()
        {
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            _mockService.Setup(x => x.GetAllPendingTitles(dbid)).Returns(() => Task.FromResult(new Mock<IEnumerable<ITitle>>().Object));

            ActionResult<IEnumerable<ITitle>> actionResult = await _titleController.GetAllPendingTitles(dbid);

            _mockService.Verify(mock => mock.GetAllPendingTitles(dbid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<ITitle> titles = okObjectResult.Value as IEnumerable<ITitle>;
            Assert.NotNull(titles);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task ApproveTitle_ValidArguments_OkObjectResult()
        {
            ArrangeCommonObjects();
            Guid dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
            string dcvid = "d12950883c414v0";
            _mockService.Setup(x => x.ApproveTitle(dbid, dcvid)).Returns(() => Task.FromResult(new Mock<ITitle>().Object));

            ActionResult<ITitle> actionResult = await _titleController.ApproveTitle(dbid, dcvid);

            _mockService.Verify(mock => mock.ApproveTitle(dbid, dcvid), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            ITitle title = okObjectResult.Value as ITitle;
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

        #region Private Methods
        private void ArrangeCommonObjects()
        {
            _mockService = new Mock<ITitleService>();
            _mockLogger = new Mock<ILogger<TitleController>>();
            _titleController = new TitleController(_mockService.Object, _mockLogger.Object);
        }
        #endregion
    }
}
