using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogField.Services.v1;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IRepo = Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1;
using IRepoEnum = Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1.Enum;
using Repo = Mavim.Manager.Api.ChangelogField.Repository.v1.Model;
using Service = Mavim.Manager.Api.ChangelogField.Services.v1.Model;

namespace Mavim.Manager.api.ChangelogField.Services.Test.v1
{
    public class FieldServiceTest
    {
        #region Private properties
        private const string _validTopicId = "d0c2v0";
        private const string _validFieldSetId = "d0c3v0";
        private const string _validFieldId = "d0c4v0";
        private const string _validInitiatorEmail = "initiator@mavim.com";
        private const string _validReviewerEmail = "reviewer@mavim.com";
        private static readonly Guid _validChangelogId = Guid.NewGuid();
        private static readonly Guid _validTenantId = Guid.NewGuid();
        private static readonly Guid _validDatabaseId = Guid.NewGuid();
        #endregion

        #region GetFields
        [Theory]
        [MemberData(nameof(RequestAndResponseChangelogFields))]
        public async Task GetFields_ValidArguments_ListOfChangelogFieldObjects(IRepo.IChangelogField response, IChangelogField request)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            repositoryMock.Setup(x => x.GetFieldsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(new List<IRepo.IChangelogField> { response });
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var result = await service.GetFields(request.DatabaseId, request.DataLanguage, request.TopicId);

            // Assert
            repositoryMock.Verify(x => x.GetFieldsByTopic(_validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validTopicId));
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var resultAsString = JsonConvert.SerializeObject(result.First());
            var requestAsString = JsonConvert.SerializeObject(request);
            Assert.Equal(requestAsString, resultAsString);
        }

        [Theory]
        [MemberData(nameof(InvalidDatabaseIdAndTopicIdArguments))]
        public async Task GetFields_InvalidArguments_ArgumentException(Guid databaseId, string topicId, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.GetFields(databaseId, DataLanguageType.Dutch, topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(error, exception.Message);
        }
        #endregion

        #region GetPendingFieldsByTopic
        [Theory]
        [MemberData(nameof(RequestAndResponseChangelogFields))]
        public async Task GetPendingFieldsByTopic_ValidArguments_ListOfChangelogFieldObjects(IRepo.IChangelogField response, IChangelogField request)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            repositoryMock.Setup(x => x.GetFieldsByTopicAndStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), It.IsAny<string>(), IRepoEnum.ChangeStatus.Pending)).ReturnsAsync(new List<IRepo.IChangelogField> { response });
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var result = await service.GetPendingFieldsByTopic(request.DatabaseId, request.DataLanguage, request.TopicId);

            // Assert
            repositoryMock.Verify(x => x.GetFieldsByTopicAndStatus(_validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validTopicId, IRepoEnum.ChangeStatus.Pending));
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var resultAsString = JsonConvert.SerializeObject(result.First());
            var requestAsString = JsonConvert.SerializeObject(request);
            Assert.Equal(requestAsString, resultAsString);
        }

        [Theory]
        [MemberData(nameof(InvalidDatabaseIdAndTopicIdArguments))]
        public async Task GetPendingFieldsByTopic_InvalidArguments_ArgumentException(Guid databaseId, string topicId, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.GetPendingFieldsByTopic(databaseId, DataLanguageType.Dutch, topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(error, exception.Message);
        }
        #endregion

        #region GetPendingFields
        [Theory]
        [MemberData(nameof(RequestAndResponseChangelogFields))]
        public async Task GetPendingFields_ValidArguments_ListOfChangelogFieldObjects(IRepo.IChangelogField response, IChangelogField request)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            repositoryMock.Setup(x => x.GetFieldsByStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), IRepoEnum.ChangeStatus.Pending)).ReturnsAsync(new List<IRepo.IChangelogField> { response });
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var result = await service.GetPendingFields(request.DatabaseId, request.DataLanguage);

            // Assert
            repositoryMock.Verify(x => x.GetFieldsByStatus(_validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, IRepoEnum.ChangeStatus.Pending));
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var resultAsString = JsonConvert.SerializeObject(result.First());
            var requestAsString = JsonConvert.SerializeObject(request);
            Assert.Equal(requestAsString, resultAsString);
        }

        [Theory]
        [MemberData(nameof(InvalidDatabaseId))]
        public async Task GetPendingFields_InvalidArguments_ArgumentException(Guid databaseId, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.GetPendingFields(databaseId, DataLanguageType.Dutch));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(error, exception.Message);
        }
        #endregion

        #region GetFieldStatus
        [Theory]
        [MemberData(nameof(RequestAndResponseChangelogFieldsWithStatus))]
        public async Task GetFieldStatus_ValidArguments_ListOfChangelogFieldObjects(IRepo.IChangelogField response, IChangelogField request, ChangeStatus status)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var responselist = response != null ? new List<IRepo.IChangelogField> { response } : new List<IRepo.IChangelogField>();
            repositoryMock.Setup(x => x.GetFieldsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(responselist);
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var result = await service.GetTopicStatusByFields(request.DatabaseId, request.DataLanguage, request.TopicId);

            // Assert
            repositoryMock.Verify(x => x.GetFieldsByTopic(_validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validTopicId));
            Assert.Equal(result, status);
        }

        [Theory]
        [MemberData(nameof(InvalidDatabaseIdAndTopicIdArguments))]
        public async Task GetFieldStatus_InvalidArguments_ArgumentException(Guid databaseId, string topicId, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.GetTopicStatusByFields(databaseId, DataLanguageType.Dutch, topicId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(error, exception.Message);
        }
        #endregion

        #region SaveField
        [Theory]
        [MemberData(nameof(RequestAndResponseSaveFields))]
        public async Task SaveField_ValidArguments_ListOfChangelogFieldObjects(ISaveFieldChange request, IRepo.ISaveFieldChange requestRepo)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            tokenMock.SetupGet(x => x.Email).Returns(_validInitiatorEmail);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            Func<IRepo.ISaveFieldChange, bool> validate = (actual) =>
            {
                Assert.Equal(requestRepo.TenantId, actual.TenantId);
                Assert.Equal(requestRepo.DatabaseId, actual.DatabaseId);
                Assert.Equal(requestRepo.DataLanguage, actual.DataLanguage);
                Assert.Equal(requestRepo.InitiatorEmail, actual.InitiatorEmail);
                Assert.Equal(requestRepo.Type, actual.Type);
                Assert.Equal(requestRepo.TopicId, actual.TopicId);
                Assert.Equal(requestRepo.Status, actual.Status);
                Assert.Equal(requestRepo.FieldSetId, actual.FieldSetId);
                Assert.Equal(requestRepo.FieldId, actual.FieldId);
                Assert.Equal(requestRepo.OldFieldValue, actual.OldFieldValue);
                Assert.Equal(requestRepo.NewFieldValue, actual.NewFieldValue);

                return true;
            };

            // Act
            await service.SaveField(request);

            // Assert
            repositoryMock.Verify(x => x.SaveField(It.Is<IRepo.ISaveFieldChange>(x => validate(x))));
        }

        [Theory]
        [MemberData(nameof(InvalidSaveFieldChange))]
        public async Task SaveField_InvalidArguments_ArgumentException(ISaveFieldChange request, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.SaveField(request));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(error, exception.Message);
        }

        [Theory]
        [MemberData(nameof(SaveFieldChangeWithEqualValues))]
        public async Task SaveField_InvalidChange_ForbiddenException(ISaveFieldChange request, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.SaveField(request));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
            Assert.Equal(error, exception.Message);
        }
        #endregion

        #region ApproveField
        [Theory]
        [MemberData(nameof(RequestAndResponseChangelogFields))]
        public async Task ApproveField_ValidArguments_ChangelogField(IRepo.IChangelogField response, IChangelogField request)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            repositoryMock.Setup(x => x.GetFieldsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(new List<IRepo.IChangelogField> { response });
            repositoryMock.Setup(x => x.GetField(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);
            repositoryMock.Setup(x => x.UpdateFieldStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IRepoEnum.ChangeStatus>())).ReturnsAsync(response);
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            tokenMock.SetupGet(x => x.Email).Returns(_validReviewerEmail);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var result = await service.ApproveField(request.DatabaseId, request.DataLanguage, request.Id);

            // Assert
            repositoryMock.Verify(x => x.GetField(request.Id, _validTenantId));
            repositoryMock.Verify(x => x.UpdateFieldStatus(request.Id, _validTenantId, _validReviewerEmail, IRepoEnum.ChangeStatus.Approved));
            Assert.NotNull(result);
            var resultAsString = JsonConvert.SerializeObject(result);
            var requestAsString = JsonConvert.SerializeObject(request);
            Assert.Equal(requestAsString, resultAsString);
        }

        [Theory]
        [MemberData(nameof(RequestAndInvalidResponseChangelogFields))]
        public async Task ApproveField_InvalidRequestedField_RequestNotFoundException(IRepo.IChangelogField response, IChangelogField request)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            repositoryMock.Setup(x => x.GetFieldsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(new List<IRepo.IChangelogField> { response });
            repositoryMock.Setup(x => x.GetField(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);
            repositoryMock.Setup(x => x.UpdateFieldStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IRepoEnum.ChangeStatus>())).ReturnsAsync(response);
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            tokenMock.SetupGet(x => x.Email).Returns(_validReviewerEmail);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await service.ApproveField(request.DatabaseId, request.DataLanguage, request.Id));

            // Assert
            repositoryMock.Verify(x => x.GetField(request.Id, _validTenantId));
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"No Pending field found with arguments: --dbId: {request.DatabaseId} --dataLanguage: {request.DataLanguage} --changelogId: {request.Id}", exception.Message);
        }

        [Theory]
        [MemberData(nameof(InvalidDatabaseIdAndChangelogIdArguments))]
        public async Task ApproveField_InvalidArguments_ArgumentException(Guid databaseId, Guid changelogId, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.ApproveField(databaseId, DataLanguageType.Dutch, changelogId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(error, exception.Message);
        }
        #endregion

        #region RejectField
        [Theory]
        [MemberData(nameof(RequestAndResponseChangelogFields))]
        public async Task RejectField_ValidArguments_ChangelogField(IRepo.IChangelogField response, IChangelogField request)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            repositoryMock.Setup(x => x.GetFieldsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(new List<IRepo.IChangelogField> { response });
            repositoryMock.Setup(x => x.GetField(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);
            repositoryMock.Setup(x => x.UpdateFieldStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IRepoEnum.ChangeStatus>())).ReturnsAsync(response);
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            tokenMock.SetupGet(x => x.Email).Returns(_validReviewerEmail);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var result = await service.RejectField(request.DatabaseId, request.DataLanguage, request.Id);

            // Assert
            repositoryMock.Verify(x => x.GetField(request.Id, _validTenantId));
            repositoryMock.Verify(x => x.UpdateFieldStatus(request.Id, _validTenantId, _validReviewerEmail, IRepoEnum.ChangeStatus.Rejected));
            Assert.NotNull(result);
            var resultAsString = JsonConvert.SerializeObject(result);
            var requestAsString = JsonConvert.SerializeObject(request);
            Assert.Equal(requestAsString, resultAsString);
        }

        [Theory]
        [MemberData(nameof(RequestAndInvalidResponseChangelogFields))]
        public async Task RejectField_InvalidRequestedField_RequestNotFoundException(IRepo.IChangelogField response, IChangelogField request)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            repositoryMock.Setup(x => x.GetFieldsByTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepoEnum.DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(new List<IRepo.IChangelogField> { response });
            repositoryMock.Setup(x => x.GetField(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);
            repositoryMock.Setup(x => x.UpdateFieldStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IRepoEnum.ChangeStatus>())).ReturnsAsync(response);
            var tokenMock = new Mock<IJwtSecurityToken>();
            tokenMock.SetupGet(x => x.TenantId).Returns(_validTenantId);
            tokenMock.SetupGet(x => x.Email).Returns(_validReviewerEmail);
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () => await service.RejectField(request.DatabaseId, request.DataLanguage, request.Id));

            // Assert
            repositoryMock.Verify(x => x.GetField(request.Id, _validTenantId));
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"No Pending field found with arguments: --dbId: {request.DatabaseId} --dataLanguage: {request.DataLanguage} --changelogId: {request.Id}", exception.Message);
        }

        [Theory]
        [MemberData(nameof(InvalidDatabaseIdAndChangelogIdArguments))]
        public async Task RejectField_InvalidArguments_ArgumentException(Guid databaseId, Guid changelogId, string error)
        {
            // Arrange
            var repositoryMock = new Mock<IRepo.IFieldRepository>();
            var tokenMock = new Mock<IJwtSecurityToken>();
            var service = new FieldService(repositoryMock.Object, tokenMock.Object);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await service.RejectField(databaseId, DataLanguageType.Dutch, changelogId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(error, exception.Message);
        }
        #endregion

        #region input params
        public static IEnumerable<object[]> RequestAndResponseChangelogFields
        {
            get
            {
                yield return new object[]
                    {
                        GetRepoChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, IRepoEnum.ChangeStatus.Pending, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue"),
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Pending, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue")
                    };
            }
        }

        public static IEnumerable<object[]> RequestAndInvalidResponseChangelogFields
        {
            get
            {
                yield return new object[]
                    {
                        null,
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Pending, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue")
                    };
                yield return new object[]
                    {
                        GetRepoChangelogField(_validChangelogId, _validTenantId, Guid.NewGuid(), IRepoEnum.DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, IRepoEnum.ChangeStatus.Pending, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue"),
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Pending, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue")
                    };
                yield return new object[]
                    {
                        GetRepoChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, IRepoEnum.ChangeStatus.Approved, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue"),
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Pending, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue")
                    };
                yield return new object[]
                    {
                        GetRepoChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.English, _validInitiatorEmail, _validReviewerEmail, _validTopicId, IRepoEnum.ChangeStatus.Pending, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue"),
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Pending, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue")
                    };
            }
        }

        public static IEnumerable<object[]> InvalidSaveFieldChange
        {
            get
            {
                yield return new object[]
                    {
                        null,
                        "field"
                    };
                yield return new object[]
                    {
                        GetServiceSaveFieldChange(Guid.Empty, DataLanguageType.Dutch, _validTopicId, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        "DatabaseId"
                    };
                yield return new object[]
                    {
                        GetServiceSaveFieldChange(_validDatabaseId, DataLanguageType.Dutch, "TopicId", _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        "TopicId"
                    };
                yield return new object[]
                    {
                        GetServiceSaveFieldChange(_validDatabaseId, DataLanguageType.Dutch, _validTopicId, "FieldSetId", _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        "FieldSetId"
                    };
                yield return new object[]
                    {
                        GetServiceSaveFieldChange(_validDatabaseId, DataLanguageType.Dutch, _validTopicId, _validFieldSetId, "FieldId", FieldType.Text, "oldValue", "newValue"),
                        "FieldId"
                    };
            }
        }

        public static IEnumerable<object[]> SaveFieldChangeWithEqualValues
        {
            get
            {
                yield return new object[]
                    {
                        GetServiceSaveFieldChange(_validDatabaseId, DataLanguageType.Dutch, _validTopicId, _validFieldSetId, _validFieldId, FieldType.Text, "value", "value"),
                        "Not allowed to have equal old and new values when saving a field"
                    };
            }
        }

        public static IEnumerable<object[]> RequestAndResponseSaveFields
        {
            get
            {
                yield return new object[]
                    {
                        GetServiceSaveFieldChange(_validDatabaseId, DataLanguageType.Dutch, _validTopicId, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        GetRepoSaveFieldChange(_validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validInitiatorEmail, _validTopicId, IRepoEnum.ChangeStatus.Pending, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue")
                    };
            }
        }

        public static IEnumerable<object[]> RequestAndResponseChangelogFieldsWithStatus
        {
            get
            {
                yield return new object[]
                    {
                        null,
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Approved, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        ChangeStatus.Approved
                    };
                yield return new object[]
                    {
                        GetRepoChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, IRepoEnum.ChangeStatus.Approved, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue"),
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Approved, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        ChangeStatus.Approved
                    };
                yield return new object[]
                    {
                        GetRepoChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, IRepoEnum.ChangeStatus.Rejected, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue"),
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Rejected, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        ChangeStatus.Rejected
                    };
                yield return new object[]
                    {
                        GetRepoChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, IRepoEnum.DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, IRepoEnum.ChangeStatus.Pending, _validFieldSetId, _validFieldId, IRepoEnum.FieldType.Text, "oldValue", "newValue"),
                        GetServiceChangelogField(_validChangelogId, _validTenantId, _validDatabaseId, DataLanguageType.Dutch, _validInitiatorEmail, _validReviewerEmail, _validTopicId, ChangeStatus.Pending, _validFieldSetId, _validFieldId, FieldType.Text, "oldValue", "newValue"),
                        ChangeStatus.Pending
                    };
            }
        }

        public static IEnumerable<object[]> InvalidDatabaseIdAndTopicIdArguments
        {
            get
            {
                yield return new object[] { Guid.Empty, _validTopicId, "dbId" };
                yield return new object[] { Guid.NewGuid(), "invalidTopicId", "topicId" };
            }
        }

        public static IEnumerable<object[]> InvalidDatabaseIdAndChangelogIdArguments
        {
            get
            {
                yield return new object[] { Guid.Empty, Guid.NewGuid(), "dbId" };
                yield return new object[] { Guid.NewGuid(), Guid.Empty, "changelogId" };
            }
        }

        public static IEnumerable<object[]> InvalidDatabaseId
        {
            get
            {
                yield return new object[] { Guid.Empty, "dbId" };
            }
        }
        #endregion

        #region private functions
        private static Repo.ChangelogField GetRepoChangelogField(Guid changelogId, Guid tenantId, Guid databaseId, IRepoEnum.DataLanguageType dataLanguage, string initiatorEmail, string reviewerEmail, string topicId, IRepoEnum.ChangeStatus status, string fieldSetId, string fieldId, IRepoEnum.FieldType type, string oldFieldValue, string newFieldValue)
        {
            var repoChangelogField = new Repo.ChangelogField()
            {
                Id = changelogId,
                TenantId = tenantId,
                DatabaseId = databaseId,
                DataLanguage = dataLanguage,
                InitiatorEmail = initiatorEmail,
                ReviewerEmail = reviewerEmail,
                Type = type,
                TopicId = topicId,
                Status = status,
                FieldSetId = fieldSetId,
                FieldId = fieldId,
                OldFieldValue = oldFieldValue,
                NewFieldValue = newFieldValue
            };

            return repoChangelogField;
        }

        private static Service.ChangelogField GetServiceChangelogField(Guid changelogId, Guid tenantId, Guid databaseId, DataLanguageType dataLanguage, string initiatorEmail, string reviewerEmail, string topicId, ChangeStatus status, string fieldSetId, string fieldId, FieldType type, string oldFieldValue, string newFieldValue)
        {
            var changelogField = new Service.ChangelogField()
            {
                Id = changelogId,
                TenantId = tenantId,
                DatabaseId = databaseId,
                DataLanguage = dataLanguage,
                InitiatorEmail = initiatorEmail,
                ReviewerEmail = reviewerEmail,
                Type = type,
                TopicId = topicId,
                Status = status,
                FieldSetId = fieldSetId,
                FieldId = fieldId,
                OldFieldValue = oldFieldValue,
                NewFieldValue = newFieldValue
            };

            return changelogField;
        }

        private static Service.SaveFieldChange GetServiceSaveFieldChange(Guid databaseId, DataLanguageType dataLanguage, string topicId, string fieldSetId, string fieldId, FieldType type, string oldFieldValue, string newFieldValue)
        {

            var changelogField = new Service.SaveFieldChange()
            {
                DatabaseId = databaseId,
                DataLanguage = dataLanguage,
                Type = type,
                TopicId = topicId,
                FieldSetId = fieldSetId,
                FieldId = fieldId,
                OldFieldValue = oldFieldValue,
                NewFieldValue = newFieldValue
            };

            return changelogField;
        }

        private static Repo.SaveFieldChange GetRepoSaveFieldChange(Guid tenantId, Guid databaseId, IRepoEnum.DataLanguageType dataLanguage, string initiatorEmail, string topicId, IRepoEnum.ChangeStatus status, string fieldSetId, string fieldId, IRepoEnum.FieldType type, string oldFieldValue, string newFieldValue)
        {
            var repoChangelogField = new Repo.SaveFieldChange()
            {
                TenantId = tenantId,
                DatabaseId = databaseId,
                DataLanguage = dataLanguage,
                InitiatorEmail = initiatorEmail,
                Type = type,
                TopicId = topicId,
                Status = status,
                FieldSetId = fieldSetId,
                FieldId = fieldId,
                OldFieldValue = oldFieldValue,
                NewFieldValue = newFieldValue
            };

            return repoChangelogField;
        }
        #endregion
    }
}
