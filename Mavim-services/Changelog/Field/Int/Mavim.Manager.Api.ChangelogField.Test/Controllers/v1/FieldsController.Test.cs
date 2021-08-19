using Mavim.Libraries.Globalization.Culture;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.ChangelogField.Controllers.v1;
using Mavim.Manager.Api.ChangelogField.Models;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogField.Services.v1.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.ChangelogField.Test.Controllers.v1
{
    public class FieldsControllerTest
    {
        #region Private properties
        private const string _validTopicId = "d0c2v0";
        private const string _validFieldSetId = "d0c3v0";
        private const string _validFieldId = "d0c4v0";
        private readonly static CultureInfo _cultureInfo = CultureInfo.CreateSpecificCulture(CultureInfoConstant.Dutch);
        private readonly JsonSerializerSettings _jsonCultureSettings = new JsonSerializerSettings() { Culture = _cultureInfo };
        #endregion

        #region GetFields
        [Fact]
        public async Task GetFields_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            mockService.Setup(x => x.GetFields(It.IsAny<Guid>(), It.IsAny<DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(new List<IChangelogField> { GetServiceFieldMock().Object });
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;

            // Act
            var actionResult = await controller.GetFields(routeParams);

            // Assert
            mockService.Verify(mock => mock.GetFields(routeParams.DatabaseId, DataLanguageType.English, routeParams.TopicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogField> fields = okObjectResult.Value as IEnumerable<IChangelogField>;
            Assert.NotNull(fields);
        }

        [Fact]
        public async Task GetFields_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new GetByTopicRouteParams();

            // Act
            var actionResult = await controller.GetFields(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region GetPendingFieldsByTopic
        [Fact]
        public async Task GetPendingFieldsByTopic_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            mockService.Setup(x => x.GetPendingFieldsByTopic(It.IsAny<Guid>(), It.IsAny<DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(new List<IChangelogField> { GetServiceFieldMock().Object });
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;

            // Act
            var actionResult = await controller.GetPendingFieldsByTopic(routeParams);

            // Assert
            mockService.Verify(mock => mock.GetPendingFieldsByTopic(routeParams.DatabaseId, DataLanguageType.English, routeParams.TopicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogField> fields = okObjectResult.Value as IEnumerable<IChangelogField>;
            Assert.NotNull(fields);
        }

        [Fact]
        public async Task GetPendingFieldsByTopic_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new GetByTopicRouteParams();

            // Act
            var actionResult = await controller.GetPendingFields(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region GetTopicStatus
        [Fact]
        public async Task GetTopicStatus_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            mockService.Setup(x => x.GetTopicStatusByFields(It.IsAny<Guid>(), It.IsAny<DataLanguageType>(), It.IsAny<string>())).ReturnsAsync(ChangeStatus.Approved);
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;

            // Act
            var actionResult = await controller.GetTopicStatus(routeParams);

            // Assert
            mockService.Verify(mock => mock.GetTopicStatusByFields(routeParams.DatabaseId, DataLanguageType.English, routeParams.TopicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.True(Enum.IsDefined(typeof(ChangeStatus), okObjectResult.Value));
        }

        [Fact]
        public async Task GetTopicStatus_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new GetByTopicRouteParams();

            // Act
            var actionResult = await controller.GetTopicStatus(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region GetPendingFields
        [Fact]
        public async Task GetPendingFields_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            mockService.Setup(x => x.GetPendingFields(It.IsAny<Guid>(), It.IsAny<DataLanguageType>())).ReturnsAsync(new List<IChangelogField> { GetServiceFieldMock().Object });
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();

            // Act
            var actionResult = await controller.GetPendingFields(routeParams);

            // Assert
            mockService.Verify(mock => mock.GetPendingFields(routeParams.DatabaseId, DataLanguageType.English), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogField> fields = okObjectResult.Value as IEnumerable<IChangelogField>;
            Assert.NotNull(fields);
        }

        [Fact]
        public async Task GetPendingFields_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new BaseRouteParam();

            // Act
            var actionResult = await controller.GetPendingFields(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region ApproveField
        [Fact]
        public async Task ApproveField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            mockService.Setup(x => x.ApproveField(It.IsAny<Guid>(), It.IsAny<DataLanguageType>(), It.IsAny<Guid>())).ReturnsAsync(GetServiceFieldMock().Object);
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.ChangelogId = Guid.NewGuid();

            // Act
            var actionResult = await controller.ApproveField(routeParams);

            // Assert
            mockService.Verify(mock => mock.ApproveField(routeParams.DatabaseId, DataLanguageType.English, routeParams.ChangelogId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IChangelogField field = okObjectResult.Value as IChangelogField;
            Assert.NotNull(field);
        }

        [Fact]
        public async Task ApproveField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();

            // Act
            var actionResult = await controller.ApproveField(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region RejectField
        [Fact]
        public async Task RejectField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            mockService.Setup(x => x.RejectField(It.IsAny<Guid>(), It.IsAny<DataLanguageType>(), It.IsAny<Guid>())).ReturnsAsync(GetServiceFieldMock().Object);
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.ChangelogId = Guid.NewGuid();

            // Act
            var actionResult = await controller.RejectField(routeParams);

            // Assert
            mockService.Verify(mock => mock.RejectField(routeParams.DatabaseId, DataLanguageType.English, routeParams.ChangelogId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IChangelogField field = okObjectResult.Value as IChangelogField;
            Assert.NotNull(field);
        }

        [Fact]
        public async Task RejectField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();

            // Act
            var actionResult = await controller.RejectField(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveBooleanField
        [Fact]
        public async Task SaveBooleanField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(true, false);

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.Boolean, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue, _jsonCultureSettings), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue, _jsonCultureSettings), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveBooleanField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveBooleanField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(true, false);

            // Act
            var actionResult = await controller.SaveBooleanField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveTextField
        [Fact]
        public async Task SaveTextField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject("test1", "test2");

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.Text, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue, _jsonCultureSettings), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue, _jsonCultureSettings), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveTextField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveTextField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject("test1", "test2");

            // Act
            var actionResult = await controller.SaveTextField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveMultiTextField
        [Fact]
        public async Task SaveMultiTextField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(new List<string> { "test1", "test2" }, new List<string> { "test3", "test4" });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.MultiText, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveMultiTextField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveMultiTextField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new List<string> { "test1", "test2" }, new List<string> { "test3", "test4" });

            // Act
            var actionResult = await controller.SaveMultiTextField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveNumberField
        [Fact]
        public async Task SaveNumberField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(1, 2);

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.Number, actual.Type);
                Assert.Equal(change.OldFieldValue.ToString(), actual.OldFieldValue);
                Assert.Equal(change.NewFieldValue.ToString(), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveNumberField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveNumberField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(1, 2);

            // Act
            var actionResult = await controller.SaveNumberField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveMultiNumberField
        [Fact]
        public async Task SaveMultiNumberField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(new List<int> { 1, 2 }, new List<int> { 3, 4 });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.MultiNumber, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveMultiNumberField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveMultiNumberField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new List<int> { 1, 2 }, new List<int> { 3, 4 });

            // Act
            var actionResult = await controller.SaveMultiNumberField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveDecimalField
        [Fact]
        public async Task SaveDecimalField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(1.2m, 3.4m);

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.Decimal, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue, _jsonCultureSettings), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue, _jsonCultureSettings), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveDecimalField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveDecimalField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(1.2m, 3.4m);

            // Act
            var actionResult = await controller.SaveDecimalField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveMultiDecimalField
        [Fact]
        public async Task SaveMultiDecimalField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(new List<decimal> { 1.2m, 3.4m }, new List<decimal> { 5.6m, 7.8m });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.MultiDecimal, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue, _jsonCultureSettings), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue, _jsonCultureSettings), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveMultiDecimalField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveMultiDecimalField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new List<decimal> { 1.2m, 3.4m }, new List<decimal> { 5.6m, 7.8m });

            // Act
            var actionResult = await controller.SaveMultiDecimalField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveDateField
        [Fact]
        public async Task SaveDateField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject<DateTime?>(DateTime.Now, DateTime.Now);

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.Date, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue, _jsonCultureSettings), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue, _jsonCultureSettings), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveDateField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveDateField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject<DateTime?>(DateTime.Now, DateTime.Now);

            // Act
            var actionResult = await controller.SaveDateField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveMultiDateField
        [Fact]
        public async Task SaveMultiDateField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(new List<DateTime?> { DateTime.Now, DateTime.Now }, new List<DateTime?> { DateTime.Now, DateTime.Now });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.MultiDate, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue, _jsonCultureSettings), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue, _jsonCultureSettings), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveMultiDateField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveMultiDateField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new List<DateTime?> { DateTime.Now, DateTime.Now }, new List<DateTime?> { DateTime.Now, DateTime.Now });

            // Act
            var actionResult = await controller.SaveMultiDateField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveListField
        [Fact]
        public async Task SaveListField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } }, new Dictionary<string, string> { { "key3", "value3" }, { "key4", "value4" } });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.List, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveListField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveListField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new Dictionary<string, string> { }, new Dictionary<string, string> { });

            // Act
            var actionResult = await controller.SaveListField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveRelationshipField
        [Fact]
        public async Task SaveRelationshipField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var change = getFieldChangeObject(new RelationshipElement() { Dcv = _validTopicId, Name = "name1", Icon = "icon1" }, new RelationshipElement() { Dcv = _validTopicId, Name = "name2", Icon = "icon2" });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.Relationship, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveRelationshipField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveRelationshipField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new RelationshipElement(), new RelationshipElement());

            // Act
            var actionResult = await controller.SaveRelationshipField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveMultiRelationshipField
        [Fact]
        public async Task SaveMultiRelationshipField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var fromRel1 = new RelationshipElement { Dcv = "d0c2v0", Name = "name1", Icon = "icon1" };
            var fromRel2 = new RelationshipElement { Dcv = "d0c3v0", Name = "name2", Icon = "icon2" };
            var toRel1 = new RelationshipElement { Dcv = "d0c4v0", Name = "name3", Icon = "icon3" };
            var toRel2 = new RelationshipElement { Dcv = "d0c5v0", Name = "name4", Icon = "icon4" };
            var change = getFieldChangeObject(new List<RelationshipElement> { fromRel1, fromRel2 }, new List<RelationshipElement> { toRel1, toRel2 });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.MultiRelationship, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveMultiRelationshipField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveMultiRelationshipField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new List<RelationshipElement> { new RelationshipElement() }, new List<RelationshipElement> { new RelationshipElement() });

            // Act
            var actionResult = await controller.SaveMultiRelationshipField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region SaveRelationshipListField
        [Fact]
        public async Task SaveRelationshipListField_ValidArguments_OkObjectResult()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var fromRel1 = new RelationshipElement { Dcv = "d0c2v0", Name = "name1", Icon = "icon1" };
            var fromRel2 = new RelationshipElement { Dcv = "d0c3v0", Name = "name2", Icon = "icon2" };
            var toRel1 = new RelationshipElement { Dcv = "d0c4v0", Name = "name3", Icon = "icon3" };
            var toRel2 = new RelationshipElement { Dcv = "d0c5v0", Name = "name4", Icon = "icon4" };
            var change = getFieldChangeObject(new Dictionary<string, RelationshipElement> { { "key1", fromRel1 }, { "key2", fromRel2 } }, new Dictionary<string, RelationshipElement> { { "key1", toRel1 }, { "key2", toRel2 } });

            Func<SaveFieldChange, bool> validate = actual =>
            {
                Assert.Equal(routeParams.DatabaseId, actual.DatabaseId);
                Assert.Equal(DataLanguageType.Dutch, actual.DataLanguage);
                Assert.Equal(change.TopicId, actual.TopicId);
                Assert.Equal(ChangeStatus.Pending, actual.Status);
                Assert.Equal(change.FieldSetId, actual.FieldSetId);
                Assert.Equal(change.FieldId, actual.FieldId);
                Assert.Equal(FieldType.RelationshipList, actual.Type);
                Assert.Equal(JsonConvert.SerializeObject(change.OldFieldValue), actual.OldFieldValue);
                Assert.Equal(JsonConvert.SerializeObject(change.NewFieldValue), actual.NewFieldValue);
                return true;
            };

            // Act
            var actionResult = await controller.SaveRelationshipListField(routeParams, change);

            // Assert
            mockService.Verify(mock => mock.SaveField(It.Is<SaveFieldChange>(f => validate(f))), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task SaveRelationshipListField_InvalidModalState_BadRequestException()
        {
            // Arrange
            var mockService = new Mock<IFieldService>();
            var mockLanguage = new Mock<IDataLanguage>();
            mockLanguage.Setup(x => x.Type).Returns(Libraries.Middlewares.Language.Enums.DataLanguageType.English);
            var controller = new FieldsController(mockService.Object, mockLanguage.Object);
            controller.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();
            var change = getFieldChangeObject(new Dictionary<string, RelationshipElement> { }, new Dictionary<string, RelationshipElement> { });

            // Act
            var actionResult = await controller.SaveRelationshipListField(routeParams, change);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region ModelState
        [Fact]
        public void ModelState_ValidGetByTopicRouteParams_ValidModalState()
        {
            // Arrange
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfTopicRouteParams))]
        public void ModelState_InvalidGetByTopicRouteParams_InvalidModalState(Guid dbId, string topicId, string modalStateError)
        {
            // Arrange
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = dbId;
            routeParams.TopicId = topicId;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }

        [Fact]
        public void ModelState_ValidBaseRouteParam_ValidModalState()
        {
            // Arrange
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Fact]
        public void ModelState_InvalidBaseRouteParam_ValidModalState()
        {
            // Arrange
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.Empty;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == "Cannot use empty Guid");
        }

        [Fact]
        public void ModelState_ValidPatchRouteParams_ValidModalState()
        {
            // Arrange
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.ChangelogId = Guid.NewGuid();
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfPatchRouteParams))]
        public void ModelState_InvalidPatchRouteParams_InvalidModalState(Guid dbId, Guid changelogId, string modalStateError)
        {
            // Arrange
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = dbId;
            routeParams.ChangelogId = changelogId;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }

        [Fact]
        public void ModelState_ValidPostBody_ValidModalState()
        {
            // Arrange
            var postBody = new FieldChange<string>();
            postBody.TopicId = _validTopicId;
            postBody.FieldSetId = _validTopicId;
            postBody.FieldId = _validTopicId;
            var context = new ValidationContext(postBody, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(postBody, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfPostBodyProperties))]
        public void ModelState_InvalidPostBody_InvalidModalState(string topicId, string fieldSetId, string fieldId, string modalStateError)
        {
            // Arrange
            var postBody = new FieldChange<string>();
            postBody.TopicId = topicId;
            postBody.FieldSetId = fieldSetId;
            postBody.FieldId = fieldId;
            var context = new ValidationContext(postBody, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(postBody, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }
        #endregion

        #region input parms
        public static IEnumerable<object[]> InvalidCombinationOfTopicRouteParams
        {
            get
            {
                yield return new object[] { Guid.Empty, _validTopicId, "Cannot use empty Guid" };
                yield return new object[] { Guid.NewGuid(), "test", "Invalid topicId format" };
                yield return new object[] { Guid.NewGuid(), null, "The TopicId field is required." };
            }
        }

        public static IEnumerable<object[]> InvalidCombinationOfPatchRouteParams
        {
            get
            {
                yield return new object[] { Guid.Empty, Guid.NewGuid(), "Cannot use empty Guid" };
                yield return new object[] { Guid.NewGuid(), Guid.Empty, "Cannot use empty Guid" };
            }
        }

        public static IEnumerable<object[]> InvalidCombinationOfPostBodyProperties
        {
            get
            {
                yield return new object[] { "", _validTopicId, _validTopicId, "The TopicId field is required." };
                yield return new object[] { _validTopicId, "", _validTopicId, "The FieldSetId field is required." };
                yield return new object[] { _validTopicId, _validTopicId, "", "The FieldId field is required." };
                yield return new object[] { "test", _validTopicId, _validTopicId, "Invalid topicId format" };
                yield return new object[] { _validTopicId, "test", _validTopicId, "Invalid fieldSetId format" };
                yield return new object[] { _validTopicId, _validTopicId, "test", "Invalid fieldId format" };
            }
        }
        #endregion

        #region private functions
        private Mock<IChangelogField> GetServiceFieldMock()
        {
            var mock = new Mock<IChangelogField>();

            return mock;
        }

        private FieldChange<T> getFieldChangeObject<T>(T oldFieldValue, T newFieldValue)
        {
            var change = new FieldChange<T>();
            change.TopicId = _validTopicId;
            change.FieldSetId = _validFieldSetId;
            change.FieldId = _validFieldId;
            change.OldFieldValue = oldFieldValue;
            change.NewFieldValue = newFieldValue;

            return change;
        }
        #endregion
    }
}
