using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.ChangelogTitle.Repository.v1;
using Mavim.Manager.Api.ChangelogTitle.Repository.v1.Model;
using Mavim.Manager.ChangelogTitle.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using DbModel = Mavim.Manager.ChangelogTitle.DbModel;

namespace Mavim.Manager.Api.ChangelogTitle.Repository.Tests.v1
{
    public class TitleRepositoryTests
    {
        #region Private Members
        private TitleDbContext _dbContext;
        private Mock<ILogger<TitleRepository>> _logger;
        private Mock<IDataLanguage> _dataLanguage;
        private TitleRepository _titleRepository;
        private ITitle _mockTitle;
        #endregion

        [Theory, MemberData(nameof(TenantIdAndDbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetTitles_OneOrMoreEmptyArguments_ArgumentNullException(Guid tenantId, Guid dbid, string dcvid)
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleRepository.GetTitles(tenantId, dbid, dcvid));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public void Constructor_NullDbContext_ArgumentNullException()
        {
            // Arrange
            ArrangeCommonObjects("Constructor_InvalidArguments_ArgumentNullException");

            // Act
            var result = Record.Exception(() => new TitleRepository(null, _logger.Object, _dataLanguage.Object));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public void Constructor_NullLogger_ArgumentNullException()
        {
            // Arrange
            ArrangeCommonObjects("Constructor_InvalidArguments_ArgumentNullException");

            // Act
            var result = Record.Exception(() => new TitleRepository(_dbContext, null, _dataLanguage.Object));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public void Constructor_NullDataLanguage_ArgumentException()
        {
            // Arrange
            ArrangeCommonObjects("Constructor_InvalidArguments_ArgumentNullException");
            _dataLanguage.Setup(p => p.Type).Returns((Libraries.Middlewares.Language.Enums.DataLanguageType)(-1));

            // Act
            var result = Record.Exception(() => new TitleRepository(_dbContext, _logger.Object, null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public void Constructor_InvalidDataLanguage_ArgumentException()
        {
            // Arrange
            ArrangeCommonObjects("Constructor_InvalidArguments_ArgumentNullException");
            _dataLanguage.Setup(p => p.Type).Returns((Libraries.Middlewares.Language.Enums.DataLanguageType)(-1));

            // Act
            var result = Record.Exception(() => new TitleRepository(_dbContext, _logger.Object, _dataLanguage.Object));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentException>(result);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetTitles_ValidArguments_TitleCollection()
        {
            ArrangeCommonObjects("GetTitles_ValidArguments_TitleCollection");
            Guid tenantId = Guid.NewGuid();
            Guid dbid = Guid.NewGuid();
            string dcvid = "d12950883c414v0";

            IEnumerable<ITitle> result = await _titleRepository.GetTitles(tenantId, dbid, dcvid);

            Assert.Empty(result);
        }

        [Theory, MemberData(nameof(TenantIdAndDbIdAndDcvIdValues))]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetPendingTitle_OneOrMoreEmptyArguments_ArgumentNullException(Guid tenantId, Guid dbid, string dcvid)
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleRepository.GetPendingTitle(tenantId, dbid, dcvid));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetPendingTitle_ValidArguments_TitleCollection()
        {
            ArrangeCommonObjects("GetPendingTitle_ValidArguments_TitleCollection");

            ITitle result = await _titleRepository.GetPendingTitle(_mockTitle.TenantId, _mockTitle.DatabaseId, _mockTitle.TopicDcv);

            Assert.NotNull(result);
            AreTitlePropertiesEqual(result, _mockTitle);
        }

        [Theory, MemberData(nameof(TenantIdAndDbIdValues))]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetAllPendingTitles_OneOrMoreEmptyArguments_ArgumentNullException(Guid tenantId, Guid dbid)
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleRepository.GetAllPendingTitles(tenantId, dbid));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task GetAllPendingTitles_ValidArguments_TitleCollection()
        {
            ArrangeCommonObjects("GetAllPendingTitles_ValidArguments_TitleCollection");

            IEnumerable<ITitle> result = await _titleRepository.GetAllPendingTitles(_mockTitle.TenantId, _mockTitle.DatabaseId);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task SaveTitle_Null_ArgumentNullException()
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleRepository.SaveTitle(null));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task SaveTitle_ValidArguments_Title()
        {
            ArrangeCommonObjects("SaveTitle_ValidArguments_Title");
            ITitle mockTitle = GetSaveMockTitle();

            await _titleRepository.SaveTitle(mockTitle);
            ITitle result = await _titleRepository.GetPendingTitle(mockTitle.TenantId, mockTitle.DatabaseId, mockTitle.TopicDcv);

            Assert.NotNull(result);
            AreTitlePropertiesEqual(result, mockTitle);
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task UpdateTitleState_Null_ArgumentNullException()
        {
            ArrangeCommonObjects();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _titleRepository.UpdateTitleState(null));
        }

        [Fact]
        [Trait("Category", "ChangelogTitle")]
        public async Task UpdateTitleState_ValidArguments_Title()
        {
            ArrangeCommonObjects("UpdateTitleState_ValidArguments_Title");
            _mockTitle.Status = ChangeStatus.Approved;

            await _titleRepository.UpdateTitleState(_mockTitle);
            IEnumerable<ITitle> result = await _titleRepository.GetTitles(_mockTitle.TenantId, _mockTitle.DatabaseId, _mockTitle.TopicDcv);

            Assert.NotNull(result);
            Assert.Collection(result, item => Assert.True(ChangeStatus.Approved == item.Status));
        }

        public static IEnumerable<object[]> TenantIdAndDbIdAndDcvIdValues
        {
            get
            {
                yield return new object[] { null, Guid.Empty, "d12950883c414v0" };
                yield return new object[] { Guid.Empty, null, "d12950883c414v0" };
                yield return new object[] { null, new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), "" };
                yield return new object[] { Guid.Empty, new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), null };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), Guid.Empty, "d12950883c414v0" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), null, "d12950883c414v0" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), "" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), null };
            }
        }

        public static IEnumerable<object[]> TenantIdAndDbIdValues
        {
            get
            {
                yield return new object[] { null, Guid.Empty };
                yield return new object[] { Guid.Empty, null };
                yield return new object[] { Guid.Empty, new Guid("0d4dea97-487d-460e-898c-2b242432a3bb") };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), Guid.Empty };
            }
        }

        #region Private Methods
        private void ArrangeCommonObjects(string dbName = "test")
        {
            _mockTitle = GetMockTitle();
            SetupDatabase(dbName);
            _logger = new Mock<ILogger<TitleRepository>>();
            _dataLanguage = new Mock<IDataLanguage>();
            _dataLanguage.Setup(p => p.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            _titleRepository = new TitleRepository(_dbContext, _logger.Object, _dataLanguage.Object);
        }
        private void AreTitlePropertiesEqual(ITitle actual, ITitle expected)
        {
            Assert.True(actual.DatabaseId == expected.DatabaseId);
            Assert.True(actual.TopicDcv == expected.TopicDcv);
            Assert.True(actual.TenantId == expected.TenantId);
            Assert.True(actual.InitiatorUserEmail == expected.InitiatorUserEmail);
            Assert.True(actual.ReviewerUserEmail == expected.ReviewerUserEmail);
            Assert.True(actual.TimestampChanged == expected.TimestampChanged);
            Assert.True(actual.TimestampApproved == expected.TimestampApproved);
            Assert.True(actual.Status == expected.Status);
            Assert.True(actual.FromTitleValue == expected.FromTitleValue);
            Assert.True(actual.ToTitleValue == expected.ToTitleValue);
        }

        private ITitle GetMockTitle()
        {
            return new Title
            {
                ChangelogId = 1,
                TenantId = Guid.NewGuid(),
                DatabaseId = Guid.NewGuid(),
                InitiatorUserEmail = Guid.NewGuid().ToString(),
                TimestampChanged = DateTime.Now,
                TopicDcv = "dcvtest",
                Status = ChangeStatus.Pending,
                FromTitleValue = "FromTitleValue",
                ToTitleValue = "ToTitleValue"
            };
        }

        private ITitle GetSaveMockTitle()
        {
            return new Title
            {
                TenantId = Guid.NewGuid(),
                DatabaseId = Guid.NewGuid(),
                InitiatorUserEmail = Guid.NewGuid().ToString(),
                TimestampChanged = DateTime.Now,
                TopicDcv = "dcvtest",
                Status = ChangeStatus.Pending,
                FromTitleValue = "FromTitleValue",
                ToTitleValue = "ToTitleValue"
            };
        }

        private void SetupDatabase(string dbName)
        {
            var options = new DbContextOptionsBuilder<TitleDbContext>().UseInMemoryDatabase(databaseName: dbName).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;
            _dbContext = new TitleDbContext(options);
            DbModel.Title newEntry = new DbModel.Title
            {
                TenantId = _mockTitle.TenantId,
                DatabaseId = _mockTitle.DatabaseId,
                InitiatorUserEmail = _mockTitle.InitiatorUserEmail,
                TimestampChanged = _mockTitle.TimestampChanged,
                TopicDcv = _mockTitle.TopicDcv,
                Status = (DbModel.Enums.ChangeStatus)_mockTitle.Status,
                FromTitleValue = _mockTitle.FromTitleValue,
                ToTitleValue = _mockTitle.ToTitleValue
            };
            _dbContext.Add(newEntry);
            _dbContext.SaveChanges();
            _dbContext.Entry(newEntry).State = EntityState.Detached;

        }
        #endregion
    }
}
