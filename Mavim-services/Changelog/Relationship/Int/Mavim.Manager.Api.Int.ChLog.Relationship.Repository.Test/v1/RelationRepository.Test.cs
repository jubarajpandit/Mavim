using FluentAssertions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.v1.Model;
using Mavim.Manager.ChLog.Relationship.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DbModel = Mavim.Manager.ChLog.Relationship.DbModel;


namespace Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Test.v1
{
    public class RelationRepositoryTest
    {
        #region private vars
        private static readonly string Email1 = "test1@mavim.nl";
        private static readonly string Email2 = "test2@mavim.nl";
        private static readonly string Email3 = "test3@mavim.nl";

        private static readonly Guid ChangelogId1 = new Guid("f10c65d2-df9f-4ad3-bce9-8412634bed9d");
        private static readonly Guid ChangelogId2 = new Guid("4b6bc440-20e3-4c0e-8398-a583c7815e8e");
        private static readonly Guid ChangelogId3 = new Guid("2e71bb0d-31ee-4cc4-badf-be6ec822c2ff");
        private static readonly Guid ChangelogId4 = new Guid("e1fa3988-3cdf-4f0e-bb5b-394ca38552a2");

        private static readonly Guid TenantId1 = new Guid("2b2e85b8-6cac-4ea0-addd-0c99d4fa4332");
        private static readonly Guid TenantId2 = new Guid("2b2e85b8-6cac-4ea0-addd-0c99d4fa4123");
        private static readonly Guid DbId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");

        private static readonly string TopicId = "d12950883c414v0";
        private static readonly string RelationId1 = "d12950883c414v1";
        private static readonly string RelationId2 = "d12950883c414v1";
        private static readonly string RelationId3 = "d12950883c414v1";
        private static readonly string RelationId4 = "d12950883c414v1";
        private static readonly string FromCategory = "when";
        private static readonly string FromTopicId = "d12950883c414v3";
        private static readonly string ToCategory = "who";
        private static readonly string ToTopicId = "d12950883c414v5";

        private readonly List<IRelation> _repoRelations = new List<IRelation>
        {
            new Relation
            {
                ChangelogId = ChangelogId1,
                TenantId = TenantId1,
                DatabaseId = DbId,
                DataLanguage = DataLanguageType.Dutch,
                InitiatorUserEmail = Email1,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId1,
                Status = ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            },
            new Relation
            {
                ChangelogId = ChangelogId2,
                TenantId = TenantId1,
                DatabaseId = DbId,
                DataLanguage = DataLanguageType.Dutch,
                InitiatorUserEmail = Email2,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId2,
                Status = ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            },
            new Relation
            {
                ChangelogId = ChangelogId3,
                TenantId = TenantId1,
                DatabaseId = DbId,
                DataLanguage = DataLanguageType.Dutch,
                InitiatorUserEmail = Email3,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId3,
                Status = ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            },
            new Relation
            {
                ChangelogId = ChangelogId4,
                TenantId = TenantId2,
                DatabaseId = DbId,
                DataLanguage = DataLanguageType.Dutch,
                InitiatorUserEmail = Email3,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId4,
                Status = ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            }
        };

        private readonly List<DbModel.Relation> _dbModelRelations = new List<DbModel.Relation>
        {
            new DbModel.Relation
            {
                ChangelogId = ChangelogId1,
                TenantId = TenantId1,
                DatabaseId = DbId,
                DataLanguage = DbModel.Enums.DataLanguageType.Dutch,
                InitiatorUserEmail = Email1,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId1,
                Status = DbModel.Enums.ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            },
            new DbModel.Relation
            {
                ChangelogId = ChangelogId2,
                TenantId = TenantId1,
                DatabaseId = DbId,
                DataLanguage = DbModel.Enums.DataLanguageType.Dutch,
                InitiatorUserEmail = Email2,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId2,
                Status = DbModel.Enums.ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            },
            new DbModel.Relation
            {
                ChangelogId = ChangelogId3,
                TenantId = TenantId1,
                DatabaseId = DbId,
                DataLanguage = DbModel.Enums.DataLanguageType.Dutch,
                InitiatorUserEmail = Email3,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId3,
                Status = DbModel.Enums.ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            },
            new DbModel.Relation
            {
                ChangelogId = ChangelogId4,
                TenantId = TenantId2,
                DatabaseId = DbId,
                DataLanguage = DbModel.Enums.DataLanguageType.Dutch,
                InitiatorUserEmail = Email3,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId4,
                Status = DbModel.Enums.ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            }
        };
        #endregion

        #region GetRelationsById
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetRelationById_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<RelationRepository>>();
            await using var context = await GetMockContext();
            var repository = new RelationRepository(context, logger.Object);

            //Act
            var result = await repository.GetRelationById(TenantId1, ChangelogId1);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(_repoRelations.First(r => r.RelationId == RelationId1));
        }
        #endregion

        #region GetRelationByTopic
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetRelationsByTopic_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<RelationRepository>>();

            await using var context = await GetMockContext();

            var repository = new RelationRepository(context, logger.Object);
            var language = DataLanguageType.Dutch;

            //Act
            var result = await repository.GetRelationsByTopic(TenantId1, DbId, language, TopicId);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(_repoRelations.FindAll(r => r.TenantId == TenantId1));

            // No data leak from other tenants
            Assert.Null(result.FirstOrDefault(r => r.TenantId == TenantId2));
        }

        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetRelationsByTopic_InvalidTopicId_RequestNotFoundException(string topicId)
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<RelationRepository>>();

            await using var context = await GetMockContext();
            var language = DataLanguageType.Dutch;

            var repository = new RelationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.GetRelationsByTopic(TenantId1, DbId, language, topicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'topicId')", exception.Message);
        }
        #endregion

        #region GetAllRelationsByState
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetAllRelationsByState_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<RelationRepository>>();

            await using var context = await GetMockContext();

            var repository = new RelationRepository(context, logger.Object);
            var language = DataLanguageType.Dutch;
            var status = ChangeStatus.Approved;

            //Act
            var result = await repository.GetAllRelationsByStatus(TenantId1, DbId, language, status);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(_repoRelations.FindAll(r => r.TenantId == TenantId1));

            // No data leak from other tenants
            Assert.Null(result.FirstOrDefault(r => r.TenantId == TenantId2));
        }
        #endregion

        #region GetAllTopicRelationsByState
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetAllTopicRelationsByState_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<RelationRepository>>();

            await using var context = await GetMockContext();

            var repository = new RelationRepository(context, logger.Object);
            var language = DataLanguageType.Dutch;
            var status = ChangeStatus.Approved;

            //Act
            var result = await repository.GetAllTopicRelationsByStatus(TenantId1, DbId, language, TopicId, status);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(_repoRelations.FindAll(r => r.TenantId == TenantId1));

            // No data leak from other tenants
            Assert.Null(result.FirstOrDefault(r => r.TenantId == TenantId2));
        }

        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetAllTopicRelationsByState_InvalidTopicId_RequestNotFoundException(string topicId)
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<RelationRepository>>();

            await using var context = await GetMockContext();
            var language = DataLanguageType.Dutch;
            var status = ChangeStatus.Approved;

            var repository = new RelationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.GetAllTopicRelationsByStatus(TenantId1, DbId, language, topicId, status));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'topicId')", exception.Message);
        }
        #endregion

        #region SaveRelation
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveRelation_ValidArguments_OkObjectResult()
        {
            //Arrange
            var logger = new Mock<ILogger<RelationRepository>>();

            var requestRelation = new Relation
            {
                TenantId = TenantId1,
                DatabaseId = DbId,
                DataLanguage = DataLanguageType.Dutch,
                InitiatorUserEmail = Email1,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId1,
                Status = ChangeStatus.Approved,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            };

            await using var context = await GetMockContext();

            var repository = new RelationRepository(context, logger.Object);

            //Act
            var result = await repository.SaveRelation(requestRelation);

            //Assert
            Assert.NotNull(result);
            var repoRelation = _repoRelations.First();
            repoRelation.ChangelogId = result.ChangelogId;
            result.Should().BeEquivalentTo(repoRelation);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetAllTopicRelationsByState_InvalidRelation_RequestNotFoundException()
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<RelationRepository>>();

            await using var context = await GetMockContext();

            var repository = new RelationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.SaveRelation(null));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'relation')", exception.Message);
        }
        #endregion

        #region UpdateRelationState
        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData(ChangeStatus.Approved, DbModel.Enums.ChangeStatus.Approved)]
        [InlineData(ChangeStatus.Pending, DbModel.Enums.ChangeStatus.Pending)]
        [InlineData(ChangeStatus.Rejected, DbModel.Enums.ChangeStatus.Rejected)]
        public async Task UpdateRelationState_ValidArguments_OkObjectResult(ChangeStatus repoStatus, DbModel.Enums.ChangeStatus dbStatus)
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<RelationRepository>>();

            var dbResponseRelation = _dbModelRelations.First();

            await using var context = await GetMockContext();

            var repository = new RelationRepository(context, logger.Object);

            //Act
            await repository.UpdateRelationStatus(TenantId1, ChangelogId1, Email1, approvedTime, repoStatus);

            var dbRelation = context.Relations.FirstOrDefault(r => r.ChangelogId == dbResponseRelation.ChangelogId);
            dbResponseRelation.Status = dbStatus;
            dbRelation.Should().BeEquivalentTo(dbResponseRelation);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task UpdateRelationState_ChangelogIdNotExists_RequestNotFoundException()
        {
            //Arrange
            var approvedTime = DateTime.Now;
            var logger = new Mock<ILogger<RelationRepository>>();

            await using var context = await GetMockContext(withData: false);

            var repository = new RelationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repository.UpdateRelationStatus(TenantId1, ChangelogId1, Email1, approvedTime, ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"Changelog id {ChangelogId1} not found", exception.Message);
        }
        #endregion

        private async Task<RelationDbContext> GetMockContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder<RelationDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new RelationDbContext(options);

            if (withData)
            {
                await context.AddRangeAsync(_dbModelRelations);
                await context.SaveChangesAsync();
            }

            return context;
        }
    }
}
