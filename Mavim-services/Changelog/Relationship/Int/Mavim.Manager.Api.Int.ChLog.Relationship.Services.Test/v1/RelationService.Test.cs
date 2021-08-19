using FluentAssertions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1.Model;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Action = Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum.Action;
using ILibrary = Mavim.Libraries.Authorization.Interfaces;
using IRepo = Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1;
using Library = Mavim.Libraries.Authorization.Models;
using Repo = Mavim.Manager.Api.Int.ChLog.Relationship.Repository.v1.Model;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Services.Test.v1
{
    public class RelationServiceTest
    {
        #region private vars
        private static readonly Guid AppId = Guid.NewGuid();

        private static readonly Guid UserId1 = Guid.NewGuid();
        private static readonly Guid UserId2 = Guid.NewGuid();
        private static readonly Guid UserId3 = Guid.NewGuid();

        private static readonly string Email1 = "test1@mavim.nl";
        private static readonly string Email3 = "test3@mavim.nl";


        private static readonly Guid ChangelogId1 = new Guid("f10c65d2-df9f-4ad3-bce9-8412634bed9d");
        private static readonly Guid ChangelogId2 = new Guid("4b6bc440-20e3-4c0e-8398-a583c7815e8e");
        private static readonly Guid ChangelogId3 = new Guid("2e71bb0d-31ee-4cc4-badf-be6ec822c2ff");

        private static readonly Guid TenantId = new Guid("2b2e85b8-6cac-4ea0-addd-0c99d4fa4332");
        private static readonly Guid Dbid = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");

        private static readonly string TopicId = "d12950883c414v0";
        private static readonly string RelationId = "d12950883c414v1";
        private static readonly string FromCategory = "when";
        private static readonly string FromTopicId = "d12950883c414v3";
        private static readonly string ToCategory = "who";
        private static readonly string ToTopicId = "d12950883c414v5";

        private ILibrary.IJwtSecurityToken GetSecurityToken(bool isAdmin = false) => new Library.JwtToken
        {
            AppId = AppId,
            Email = isAdmin ? Email3 : Email1,
            Name = "Test User",
            UserID = isAdmin ? UserId3 : UserId1,
            TenantId = TenantId,
            Token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken()
        };

        private readonly List<IRepo.Interface.IRelation> _repoRelations = new List<IRepo.Interface.IRelation>
        {
            new Repo.Relation
            {
                ChangelogId = ChangelogId1,
                TenantId = TenantId,
                DatabaseId = Dbid,
                DataLanguage = IRepo.Enum.DataLanguageType.Dutch,
                InitiatorUserEmail = Email1,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId,
                Action = IRepo.Enum.Action.Create,
                Status = IRepo.Enum.ChangeStatus.Pending,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            }
        };

        private readonly List<IRelation> _serviceRelations = new List<IRelation>
        {
            new Relation
            {
                ChangelogId = ChangelogId1,
                InitiatorUserEmail = Email1,
                TimestampChanged = DateTime.MinValue,
                TimestampApproved = DateTime.MaxValue,
                TopicId = TopicId,
                RelationId = RelationId,
                Action = Action.Create,
                Status = ChangeStatus.Pending,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            }
        };
        #endregion

        #region GetRelations
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetRelations_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;
            var repositoryLanguageType = IRepo.Enum.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            var result = await service.GetRelationsByTopic(Dbid, TopicId);


            //Assert
            mockService.Verify(mock => mock.GetRelationsByTopic(TenantId, Dbid, It.Is<IRepo.Enum.DataLanguageType>(Type => Type == repositoryLanguageType), TopicId), Times.Once);

            Assert.NotEmpty(result);
            result.ToList().ForEach(relation =>
            {
                var relationId = relation.RelationId;
                relation.Should().BeEquivalentTo(_serviceRelations.First(r => r.RelationId == relationId));
            });

        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetRelations_EmptyDbId_ArgumentException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetRelationsByTopic(Guid.Empty, TopicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("DbId argument empty", exception.Message);
        }

        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task GetRelations_EmptyTopicId_ArgumentException(string topicId)
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetRelationsByTopic(Dbid, topicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("TopicID is invalid", exception.Message);
        }
        #endregion

        #region GetAllTopicRelationsByState
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetPendingRelations_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetAllTopicRelationsByStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>(), It.IsAny<IRepo.Enum.ChangeStatus>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;
            var repositoryLanguageType = IRepo.Enum.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            var result = await service.GetPendingRelationsByTopic(Dbid, TopicId);


            //Assert
            mockService.Verify(mock => mock.GetAllTopicRelationsByStatus(TenantId, Dbid, It.Is<IRepo.Enum.DataLanguageType>(Type => Type == repositoryLanguageType), TopicId, IRepo.Enum.ChangeStatus.Pending), Times.Once);

            Assert.NotEmpty(result);
            result.ToList().ForEach(relation =>
            {
                var relationId = relation.RelationId;
                relation.Should().BeEquivalentTo(_serviceRelations.First(r => r.RelationId == relationId));
            });

        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetPendingRelations_EmptyDbId_ArgumentException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetPendingRelationsByTopic(Guid.Empty, TopicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("DbId argument empty", exception.Message);
        }

        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task GetPendingRelations_EmptyTopicId_ArgumentException(string topicId)
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetPendingRelationsByTopic(Dbid, topicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("TopicID is invalid", exception.Message);
        }
        #endregion

        #region GetAllPendingRelations
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetAllPendingRelations_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetAllRelationsByStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<IRepo.Enum.ChangeStatus>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;
            var repositoryLanguageType = IRepo.Enum.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            var result = await service.GetAllPendingRelations(Dbid);

            mockService.Verify(mock => mock.GetAllRelationsByStatus(TenantId, Dbid, It.Is<IRepo.Enum.DataLanguageType>(Type => Type == repositoryLanguageType), It.IsAny<IRepo.Enum.ChangeStatus>()), Times.Once);

            Assert.NotEmpty(result);
            result.ToList().ForEach(relation =>
            {
                var relationId = relation.RelationId;
                relation.Should().BeEquivalentTo(_serviceRelations.First(r => r.RelationId == relationId));
            });
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetAllPendingRelations_EmptyDbId_ArgumentException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetAllPendingRelations(Guid.Empty));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("DbId argument empty", exception.Message);
        }
        #endregion

        #region GetRelationStatus
        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData(IRepo.Enum.ChangeStatus.Approved, ChangeStatus.Approved)]
        [InlineData(IRepo.Enum.ChangeStatus.Pending, ChangeStatus.Pending)]
        [InlineData(IRepo.Enum.ChangeStatus.Rejected, ChangeStatus.Rejected)]
        public async Task GetRelationStatus_ValidArguments_OkObjectResult(IRepo.Enum.ChangeStatus repoStatus, ChangeStatus serviceStatus)
        {
            //Arrange

            var relations = _repoRelations.Select(r =>
            {
                r.Status = repoStatus;
                return r;
            });

            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => relations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;
            var repositoryLanguageType = IRepo.Enum.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            ChangeStatus result = await service.GetRelationStatus(Dbid, TopicId);

            mockService.Verify(mock => mock.GetRelationsByTopic(TenantId, Dbid, It.Is<IRepo.Enum.DataLanguageType>(Type => Type == repositoryLanguageType), TopicId), Times.Once);

            Assert.Equal(serviceStatus, result);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetRelationStatus_EmptyDbId_ArgumentException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetRelationStatus(Guid.Empty, TopicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("DbId argument empty", exception.Message);
        }

        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task GetRelationStatus_EmptyTopicId_ArgumentException(string topicId)
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetRelationStatus(Dbid, topicId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("TopicID is invalid", exception.Message);
        }
        #endregion

        #region SaveRelation
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveRelation_ValidArguments_OkObjectResult()
        {
            //Arrange
            var repoRequestRelation = _repoRelations.First();
            repoRequestRelation.Status = IRepo.Enum.ChangeStatus.Pending;

            var serviceRequestRelation = _serviceRelations.First();
            serviceRequestRelation.Status = ChangeStatus.Pending;

            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.SaveRelation(It.IsAny<IRepo.Interface.IRelation>()))
                .Callback((IRepo.Interface.IRelation relation) =>
                {
                    relation.TimestampChanged = repoRequestRelation.TimestampChanged;
                    relation.TimestampApproved = repoRequestRelation.TimestampApproved;
                    relation.ChangelogId = repoRequestRelation.ChangelogId;
                }).ReturnsAsync(() => _repoRelations.First());

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;



            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            var saveRelation = new SaveRelation
            {
                TopicId = TopicId,
                RelationId = RelationId,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            };

            Func<IRepo.Interface.IRelation, bool> validate = relation =>
            {
                relation.Should().BeEquivalentTo(repoRequestRelation);
                return true;
            };

            //Act
            var result = await service.SaveRelation(Dbid, saveRelation);

            mockService.Verify(mock => mock.SaveRelation(It.Is<IRepo.Interface.IRelation>(r => validate(r))), Times.Once);

            Assert.NotNull(result);

            result.Should().BeEquivalentTo(serviceRequestRelation);

        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveRelation_EmptyDbId_ArgumentException()
        {
            //Arrange
            var saveRelation = new SaveRelation
            {
                TopicId = TopicId,
                RelationId = RelationId,
                OldCategory = FromCategory,
                OldTopicId = FromTopicId,
                Category = ToCategory,
                ToTopicId = ToTopicId
            };

            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.SaveRelation(Guid.Empty, saveRelation));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("DbId argument empty", exception.Message);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveRelation_nullSaveRelation_ArgumentNullException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.SaveRelation(Dbid, null));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region UpdateRelationState
        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData(ChangeStatus.Approved)]
        [InlineData(ChangeStatus.Rejected)]
        public async Task UpdateRelationState_ValidArguments_OkObjectResult(ChangeStatus serviceStatus)
        {
            //Arrange
            var repoRequestRelation = _repoRelations.First();

            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.UpdateRelationStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IRepo.Enum.ChangeStatus>()));
            mockService.Setup(x => x.GetRelationById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => repoRequestRelation);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;



            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            Func<IRepo.Interface.IRelation, bool> validate = relation =>
            {
                relation.Should().BeEquivalentTo(repoRequestRelation);
                return true;
            };

            //Act
            await service.UpdateRelationStatus(Dbid, ChangelogId1, serviceStatus);

            mockService.Verify(mock => mock.UpdateRelationStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<IRepo.Enum.ChangeStatus>()), Times.Once);

            mockService.Verify(mock => mock.GetRelationById(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task UpdateRelationState_EmptyDbId_ArgumentException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.DataLanguageType>(), It.IsAny<string>()))
                       .ReturnsAsync(() => _repoRelations);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.UpdateRelationStatus(Guid.Empty, ChangelogId1, ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("DbId argument empty", exception.Message);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task UpdateRelationState_EmptyChangelogId_ArgumentException()
        {
            //Arrange
            Guid changelogId = Guid.Empty;
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                       .ReturnsAsync(() => _repoRelations.First());

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.UpdateRelationStatus(Dbid, changelogId, ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("ChangelogId argument empty", exception.Message);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task UpdateRelationState_InvalidLanguage_RequestNotFoundException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                       .ReturnsAsync(() => _repoRelations.First());

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.English;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.UpdateRelationStatus(Dbid, ChangelogId1, ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"Changelog id {ChangelogId1} not found", exception.Message);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task UpdateRelationState_InvalidDbId_RequestNotFoundException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                       .ReturnsAsync(() => _repoRelations.First());

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.UpdateRelationStatus(Guid.NewGuid(), ChangelogId1, ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"Changelog id {ChangelogId1} not found", exception.Message);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task UpdateRelationState_RelationNotExists_RequestNotFoundException()
        {
            //Arrange
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                       .ReturnsAsync(() => null);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.UpdateRelationStatus(Dbid, ChangelogId1, ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"Changelog id {ChangelogId1} not found", exception.Message);
        }

        [Theory]
        [Trait("Category", "ChangelogRelation")]
        [InlineData(IRepo.Enum.ChangeStatus.Approved)]
        [InlineData(IRepo.Enum.ChangeStatus.Rejected)]
        public async Task UpdateRelationState_RelationNotInPandingState_RequestNotFoundException(IRepo.Enum.ChangeStatus status)
        {
            //Arrange
            var relation = _repoRelations.First();
            relation.Status = status;
            var mockService = new Mock<IRepo.IRelationRepository>();
            mockService.Setup(x => x.GetRelationById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                       .ReturnsAsync(() => relation);

            var middlewareLanguageType = Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch;

            var language = new Mock<IDataLanguage>();
            language.SetupGet(d => d.Type).Returns(middlewareLanguageType);

            var logger = new Mock<ILogger<RelationService>>();
            var service = new RelationService(mockService.Object, GetSecurityToken(), language.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.UpdateRelationStatus(Dbid, ChangelogId1, ChangeStatus.Approved));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal("Cannot update non-pending relations", exception.Message);
        }
        #endregion
    }
}
