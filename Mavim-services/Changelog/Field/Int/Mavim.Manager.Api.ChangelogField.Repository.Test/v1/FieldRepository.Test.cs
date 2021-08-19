using FluentAssertions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.ChangelogField.Repository.v1;
using Mavim.Manager.ChangelogField.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DbModel = Mavim.Manager.ChangelogField.DbModel;
using IRepo = Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1;
using Repo = Mavim.Manager.Api.ChangelogField.Repository.v1.Model;

namespace Mavim.Manager.Api.ChangelogField.Repository.Test.v1
{
    public class FieldRepositoryTest
    {
        #region private vars
        private static readonly string _email1 = "test1@mavim.nl";
        private static readonly string _email2 = "test2@mavim.nl";
        private static readonly string _email3 = "test3@mavim.nl";

        private static readonly Guid _changelogId1 = new Guid("f10c65d2-df9f-4ad3-bce9-8412634bed9d");
        private static readonly Guid _changelogId2 = new Guid("4b6bc440-20e3-4c0e-8398-a583c7815e8e");
        private static readonly Guid _changelogId3 = new Guid("2e71bb0d-31ee-4cc4-badf-be6ec822c2ff");
        private static readonly Guid _changelogId4 = new Guid("e1fa3988-3cdf-4f0e-bb5b-394ca38552a2");

        private static readonly Guid _tenantId1 = new Guid("2b2e85b8-6cac-4ea0-addd-0c99d4fa4332");
        private static readonly Guid _tenantId2 = new Guid("2b2e85b8-6cac-4ea0-addd-0c99d4fa4123");
        private static readonly Guid _dbId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");

        private static readonly string _topicId = "d12950883c414v0";
        private static readonly string _fieldId1 = "d12950883c414v1";
        private static readonly string _fieldId2 = "d12950883c414v1";
        private static readonly string _fieldId3 = "d12950883c414v1";
        private static readonly string _fieldId4 = "d12950883c414v1";
        private static readonly string _oldTextValue = "oldValue";
        private static readonly string _newTextValue = "newValue";
        private static readonly string _oldBooleanValue = true.ToString();
        private static readonly string _newBooleanValue = false.ToString();

        private readonly List<IRepo.IChangelogField> RepoFields = new List<IRepo.IChangelogField>
        {
            new Repo.ChangelogField
            {
                Id = _changelogId1,
                TenantId = _tenantId1,
                DatabaseId = _dbId,
                DataLanguage = IRepo.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email1,
                ReviewerEmail = _email1,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = IRepo.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId1,
                FieldId = _fieldId1,
                Type = IRepo.Enum.FieldType.Text,
                OldFieldValue = _oldTextValue,
                NewFieldValue = _newTextValue
            },
            new Repo.ChangelogField
            {
                Id = _changelogId2,
                TenantId = _tenantId2,
                DatabaseId = _dbId,
                DataLanguage = IRepo.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email2,
                ReviewerEmail = _email2,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = IRepo.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId2,
                FieldId = _fieldId2,
                Type = IRepo.Enum.FieldType.Boolean,
                OldFieldValue = _oldBooleanValue,
                NewFieldValue = _newBooleanValue
            },
            new Repo.ChangelogField
            {
                Id = _changelogId3,
                TenantId = _tenantId1,
                DatabaseId = _dbId,
                DataLanguage = IRepo.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email3,
                ReviewerEmail = _email3,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = IRepo.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId3,
                FieldId = _fieldId3,
                Type = IRepo.Enum.FieldType.Text,
                OldFieldValue = _oldTextValue,
                NewFieldValue = _newTextValue
            },
            new Repo.ChangelogField
            {
                Id = _changelogId4,
                TenantId = _tenantId2,
                DatabaseId = _dbId,
                DataLanguage = IRepo.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email2,
                ReviewerEmail = _email2,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = IRepo.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId4,
                FieldId = _fieldId4,
                Type = IRepo.Enum.FieldType.Boolean,
                OldFieldValue = _oldBooleanValue,
                NewFieldValue = _newBooleanValue
            }
        };

        private readonly List<DbModel.ChangelogField> DbModelFields = new List<DbModel.ChangelogField>
        {
            new DbModel.ChangelogField
            {
                Id = _changelogId1,
                TenantId = _tenantId1,
                DatabaseId = _dbId,
                DataLanguage = DbModel.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email1,
                ReviewerEmail = _email1,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = DbModel.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId1,
                FieldId = _fieldId1,
                Type = DbModel.Enum.FieldType.Text,
                OldFieldValue = _oldTextValue,
                NewFieldValue = _newTextValue
            },
            new DbModel.ChangelogField
            {
                Id = _changelogId2,
                TenantId = _tenantId2,
                DatabaseId = _dbId,
                DataLanguage = DbModel.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email2,
                ReviewerEmail = _email2,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = DbModel.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId2,
                FieldId = _fieldId2,
                Type = DbModel.Enum.FieldType.Boolean,
                OldFieldValue = _oldBooleanValue,
                NewFieldValue = _newBooleanValue
            },
            new DbModel.ChangelogField
            {
                Id = _changelogId3,
                TenantId = _tenantId1,
                DatabaseId = _dbId,
                DataLanguage = DbModel.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email3,
                ReviewerEmail = _email3,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = DbModel.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId3,
                FieldId = _fieldId3,
                Type = DbModel.Enum.FieldType.Text,
                OldFieldValue = _oldTextValue,
                NewFieldValue = _newTextValue
            },
            new DbModel.ChangelogField
            {
                Id = _changelogId4,
                TenantId = _tenantId2,
                DatabaseId = _dbId,
                DataLanguage = DbModel.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email2,
                ReviewerEmail = _email2,
                TimestampChanged = DateTime.MinValue,
                TimestampReviewed = DateTime.MaxValue,
                TopicId = _topicId,
                Status = DbModel.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId4,
                FieldId = _fieldId4,
                Type = DbModel.Enum.FieldType.Boolean,
                OldFieldValue = _oldBooleanValue,
                NewFieldValue = _newBooleanValue
            }
        };
        #endregion

        #region GetField
        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task GetField_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<FieldRepository>>();
            using var context = await GetMockContext();
            var repository = new FieldRepository(context, logger.Object);

            //Act
            var result = await repository.GetField(_changelogId1, _tenantId1);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(RepoFields.First(r => r.Id == _changelogId1));
        }
        #endregion

        #region GetFields
        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task GetFields_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<FieldRepository>>();
            using var context = await GetMockContext();
            var repository = new FieldRepository(context, logger.Object);
            var language = IRepo.Enum.DataLanguageType.Dutch;

            //Act
            var result = await repository.GetFields(_tenantId1, _dbId, language);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            result.First().Should().BeEquivalentTo(RepoFields.First(r => r.Id == _changelogId1));

            // No data leak from other tenants
            Assert.DoesNotContain(result, r => r.TenantId == _tenantId2);
        }
        #endregion

        #region GetFieldsByTopic
        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task GetFieldsByTopic_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<FieldRepository>>();

            using var context = await GetMockContext();

            var repository = new FieldRepository(context, logger.Object);
            var language = IRepo.Enum.DataLanguageType.Dutch;

            //Act
            var result = await repository.GetFieldsByTopic(_tenantId1, _dbId, language, _topicId);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(RepoFields.FindAll(r => r.TenantId == _tenantId1));

            // No data leak from other tenants
            Assert.DoesNotContain(result, r => r.TenantId == _tenantId2);
        }

        [Theory]
        [Trait("Category", "ChangelogField")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetFieldsByTopic_InvalidTopicId_RequestNotFoundException(string topicId)
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<FieldRepository>>();

            using var context = await GetMockContext();
            var language = IRepo.Enum.DataLanguageType.Dutch;

            var repository = new FieldRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.GetFieldsByTopic(_tenantId1, _dbId, language, topicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'topicId')", exception.Message);
        }
        #endregion

        #region GetFieldsByStatus
        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task GetFieldsByStatus_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<FieldRepository>>();

            using var context = await GetMockContext();

            var repository = new FieldRepository(context, logger.Object);
            var language = IRepo.Enum.DataLanguageType.Dutch;
            var status = IRepo.Enum.ChangeStatus.Approved;

            //Act
            var result = await repository.GetFieldsByStatus(_tenantId1, _dbId, language, status);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(RepoFields.FindAll(r => r.TenantId == _tenantId1));

            // No data leak from other tenants
            Assert.Null(result.FirstOrDefault(r => r.TenantId == _tenantId2));
        }
        #endregion

        #region GetFieldsByTopicAndStatus
        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task GetFieldsByTopicAndStatus_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<FieldRepository>>();

            using var context = await GetMockContext();

            var repository = new FieldRepository(context, logger.Object);
            var language = IRepo.Enum.DataLanguageType.Dutch;
            var status = IRepo.Enum.ChangeStatus.Approved;

            //Act
            var result = await repository.GetFieldsByTopicAndStatus(_tenantId1, _dbId, language, _topicId, status);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(RepoFields.FindAll(r => r.TenantId == _tenantId1));

            // No data leak from other tenants
            Assert.Null(result.FirstOrDefault(r => r.TenantId == _tenantId2));
        }

        [Theory]
        [Trait("Category", "ChangelogField")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetFieldsByTopicAndStatus_InvalidTopicId_RequestNotFoundException(string topicId)
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<FieldRepository>>();

            using var context = await GetMockContext();
            var language = IRepo.Enum.DataLanguageType.Dutch;
            var status = IRepo.Enum.ChangeStatus.Approved;

            var repository = new FieldRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.GetFieldsByTopicAndStatus(_tenantId1, _dbId, language, topicId, status));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'topicId')", exception.Message);
        }
        #endregion

        #region SaveField
        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task SaveField_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<FieldRepository>>();

            var saveField = new Repo.SaveFieldChange
            {
                TenantId = _tenantId1,
                DatabaseId = _dbId,
                DataLanguage = IRepo.Enum.DataLanguageType.Dutch,
                InitiatorEmail = _email1,
                TimestampChanged = DateTime.MinValue,
                TopicId = _topicId,
                Status = IRepo.Enum.ChangeStatus.Approved,
                FieldSetId = _fieldId1,
                FieldId = _fieldId1,
                Type = IRepo.Enum.FieldType.Text,
                OldFieldValue = _oldTextValue,
                NewFieldValue = _newTextValue
            };

            using var context = await GetMockContext();

            var repository = new FieldRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.SaveField(saveField));


            //Assert
            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task SaveField_InvalidRelation_RequestNotFoundException()
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<FieldRepository>>();

            using var context = await GetMockContext();

            var repository = new FieldRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.SaveField(null));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'field')", exception.Message);
        }
        #endregion

        #region UpdateFieldStatus
        [Theory]
        [Trait("Category", "ChangelogField")]
        [InlineData(IRepo.Enum.ChangeStatus.Approved, DbModel.Enum.ChangeStatus.Approved)]
        [InlineData(IRepo.Enum.ChangeStatus.Pending, DbModel.Enum.ChangeStatus.Pending)]
        [InlineData(IRepo.Enum.ChangeStatus.Rejected, DbModel.Enum.ChangeStatus.Rejected)]
        public async Task UpdateFieldStatus_ValidArguments_OkObjectResult(IRepo.Enum.ChangeStatus repoStatus, DbModel.Enum.ChangeStatus dbStatus)
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<FieldRepository>>();

            var dbResponseField = DbModelFields.First();

            using var context = await GetMockContext();

            var repository = new FieldRepository(context, logger.Object);

            //Act
            await repository.UpdateFieldStatus(_changelogId1, _tenantId1, _email1, repoStatus);

            var dbField = context.Fields.FirstOrDefault(r => r.Id == dbResponseField.Id);
            dbResponseField.Status = dbStatus;
            dbResponseField.Should().BeEquivalentTo(dbField);
        }

        [Fact]
        [Trait("Category", "ChangelogField")]
        public async Task UpdateFieldStatus_ChangelogIdNotExists_RequestNotFoundException()
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<FieldRepository>>();

            using var context = await GetMockContext(withData: false);

            var repository = new FieldRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.UpdateFieldStatus(_changelogId1, _tenantId1, _email1, IRepo.Enum.ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"No field change found with arguments: --changelogId: {_changelogId1} --tenantId: {_tenantId1}", exception.Message);
        }
        #endregion

        #region private functions

        private async Task<FieldDbContext> GetMockContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder<FieldDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new FieldDbContext(options);

            if (withData)
            {
                await context.AddRangeAsync(DbModelFields);
                await context.SaveChangesAsync();
            }

            return context;
        }
        #endregion
    }
}
