using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Interface;
using Mavim.Manager.Api.ChangelogTitle.Services.v1;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.ChangelogTitle.Services.Tests.v1
{
    public class TitleServiceTests
    {
        #region Private Members
        private Mock<ITitleRepository> _mockRepository;
        private Mock<ILogger<TitleService>> _mockLogger;
        private Mock<IJwtSecurityToken> _mockToken;
        private TitleService _titleService;
        #endregion

        [Theory, MemberData(nameof(DbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetTitles_OneOrMoreEmptyArguments_ArgumentNullException(Guid dbid, string dcvid)
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleService.GetTitles(dbid, dcvid));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetTitles_ValidArguments_EmptyCollection()
        {
            ArrangeCommonObjects();
            Guid dbid = Guid.NewGuid();
            string dcvid = "d12950883c414v0";

            IEnumerable<ITitle> result = await _titleService.GetTitles(dbid, dcvid);

            _mockRepository.Verify(mock => mock.GetTitles(_mockToken.Object.TenantId, dbid, dcvid), Times.Once);
            Assert.Empty(result);
        }

        [Theory, MemberData(nameof(DbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetPendingTitle_OneOrMoreEmptyArguments_ArgumentNullException(Guid dbid, string dcvid)
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleService.GetPendingTitle(dbid, dcvid));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetPendingTitle_ValidArguments_Title()
        {
            ArrangeCommonObjects();
            Guid dbid = Guid.NewGuid();
            string dcvid = "d12950883c414v0";
            Repository.Interfaces.v1.Interface.ITitle mockTitle = GetMockTitle(dbid, dcvid);
            _mockRepository.Setup(m => m.GetPendingTitle(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(mockTitle));

            ITitle result = await _titleService.GetPendingTitle(dbid, dcvid);

            Assert.True(mockTitle.TenantId == _mockToken.Object.TenantId);
            Assert.True(mockTitle.DatabaseId == dbid);
            Assert.True(mockTitle.TopicDcv == dcvid);
            _mockRepository.Verify(mock => mock.GetPendingTitle(_mockToken.Object.TenantId, dbid, dcvid), Times.Once);
            AreTitlePropertiesEqual(result, mockTitle);
        }

        [Theory, MemberData(nameof(DbIdValues))]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetAllPendingTitles_OneOrMoreEmptyArguments_ArgumentNullException(Guid dbid)
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleService.GetAllPendingTitles(dbid));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task SaveTitle_ValidArguments_RepositoySaveTitleCalled()
        {
            ArrangeCommonObjects();

            Guid dbid = Guid.NewGuid();
            string dcvid = "d12950883c414v0";
            Interfaces.v1.Interface.ISaveTitle mockSaveTitle = GetMockSaveTitle(dbid, dcvid);

            await _titleService.SaveTitle(mockSaveTitle);

            _mockRepository.Verify(mock => mock.SaveTitle(It.Is<Repository.Interfaces.v1.Interface.ITitle>(p =>
                    p.TenantId == _mockToken.Object.TenantId &&
                    p.InitiatorUserEmail == _mockToken.Object.Email.ToString() &&
                    p.TopicDcv == dcvid &&
                    p.DatabaseId == dbid &&
                    p.Status == Repository.Interfaces.v1.Enum.ChangeStatus.Pending
                )), Times.Once);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetAllPendingTitles_ValidArguments_Title()
        {
            ArrangeCommonObjects();
            Guid dbid = Guid.NewGuid();

            IEnumerable<ITitle> result = await _titleService.GetAllPendingTitles(dbid);

            _mockRepository.Verify(mock => mock.GetAllPendingTitles(_mockToken.Object.TenantId, dbid), Times.Once);
            Assert.Empty(result);
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
            _mockRepository = new Mock<ITitleRepository>();
            _mockLogger = new Mock<ILogger<TitleService>>();
            _mockToken = new Mock<IJwtSecurityToken>();
            _mockToken.SetupProperty(m => m.TenantId).SetupProperty(m => m.UserID);
            _mockToken.Object.TenantId = Guid.NewGuid();
            _mockToken.SetupGet(p => p.Email).Returns("test@mavim.com");
            _titleService = new TitleService(_mockRepository.Object, _mockLogger.Object, _mockToken.Object);
        }

        private Repository.Interfaces.v1.Interface.ITitle GetMockTitle(Guid dbid, string dcvid)
        {
            return Mock.Of<Repository.Interfaces.v1.Interface.ITitle>(m =>
                m.ChangelogId == 3 &&
                m.DatabaseId == dbid &&
                m.TopicDcv == dcvid &&
                m.TenantId == _mockToken.Object.TenantId &&
                m.InitiatorUserEmail == _mockToken.Object.UserID.ToString() &&
                m.TimestampChanged == DateTime.Now &&
                m.Status == Repository.Interfaces.v1.Enum.ChangeStatus.Pending &&
                m.FromTitleValue == "test1" &&
                m.ToTitleValue == "test2");
        }

        private Interfaces.v1.Interface.ISaveTitle GetMockSaveTitle(Guid dbid, string dcvid)
        {
            return Mock.Of<Interfaces.v1.Interface.ISaveTitle>(m =>
                m.DatabaseId == dbid &&
                m.TopicDcv == dcvid &&
                m.TenantId == _mockToken.Object.TenantId &&
                m.InitiatorUserEmail == _mockToken.Object.UserID.ToString() &&
                m.TimestampChanged == DateTime.Now &&
                m.FromTitleValue == "test1" &&
                m.ToTitleValue == "test2");
        }

        private void AreTitlePropertiesEqual(ITitle actual, Repository.Interfaces.v1.Interface.ITitle expected)
        {
            Assert.True(actual.TopicDcv == expected.TopicDcv);
            Assert.True(actual.InitiatorUserEmail == expected.InitiatorUserEmail);
            Assert.True(actual.TimestampChanged == expected.TimestampChanged);
            Assert.True(actual.Status == (ChangeStatus)expected.Status);
            Assert.True(actual.FromTitleValue == expected.FromTitleValue);
            Assert.True(actual.ToTitleValue == expected.ToTitleValue);
        }
        #endregion
    }
}
