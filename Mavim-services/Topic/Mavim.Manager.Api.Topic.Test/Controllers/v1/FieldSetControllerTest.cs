using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Controllers.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using Mavim.Manager.Api.Topic.v1.Mappers.Interfaces;
using Mavim.Manager.Api.Topic.v1.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using MultiHyperlinkField = Mavim.Manager.Api.Topic.v1.Models.PatchMultiHyperlinkField;

namespace Mavim.Manager.Api.Topic.Test.Controllers.v1
{
    public class FieldSetControllerTest
    {
        private static readonly Guid DATABASEID = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
        private const string DCVID = "d12950883c414v0";
        private const string FIELDSETID = "d5926266c1352v0";
        private const string FIELDID = "d5926266c7221v0";

        #region GetFields
        [Fact]
        [Trait("Category", "TopicField")]
        public async Task GetTopicFields_ValidArguments_OkObjectResult()
        {
            //Arrange
            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.GetFields(It.IsAny<string>()))
                            .ReturnsAsync(new List<SingleTextField> { new SingleTextField() });
            var fieldMapper = new Mock<IFieldMapper>();

            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.GetTopicFields(DATABASEID, DataLanguages.en, DCVID);

            //Assert
            fieldServiceMock.Verify(mock => mock.GetFields(DCVID), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IEnumerable<IField>;
            Assert.NotNull(fieldsResult);
            Assert.True(fieldsResult.Any());
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task GetFieldByDcvAndFieldsetId_ValidArguments_OkObjectResult()
        {
            //Arrange
            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.GetField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(new SingleTextField());

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.GetFieldByDcvAndFieldsetId(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID);

            //Assert
            fieldServiceMock.Verify(mock => mock.GetField(DCVID, FIELDSETID, FIELDID), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as SingleTextField;
            Assert.NotNull(fieldResult);
        }
        #endregion

        #region SingleTextField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateTextSingleField_ValidArguments_OkObjectResult()
        {
            //Arrange
            SingleTextField SINGLETEXTFIELD = new SingleTextField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = "Cheese",
                FieldValueType = FieldType.Text
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(SINGLETEXTFIELD);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateTextSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, SINGLETEXTFIELD);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<SingleTextField>(f => f.FieldValueType == FieldType.Text)), Times.Once
            );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Text);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateTextSingleField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            SingleTextField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateTextSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region MultiTextField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateTextMultiField_ValidArguments_OkObjectResult()
        {
            //Arrange
            MultiTextField MULTITEXTFIELD = new MultiTextField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new[] { "Cheese" },
                FieldValueType = FieldType.MultiText
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(MULTITEXTFIELD);
            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);
            var dbid = Guid.Empty;

            //Act
            var actionResult = await controller.UpdateTextMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, MULTITEXTFIELD);

            //Assert
            fieldServiceMock.Verify(mock =>
                    mock.UpdateFieldValue(It.Is<MultiTextField>(f => f.FieldValueType == FieldType.MultiText)), Times.Once
                );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as MultiTextField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.Data.Any());
            Assert.True(fieldResult.FieldValueType == FieldType.MultiText);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateTextMultiField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            MultiTextField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateTextMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region SingleNumberField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateNumberSingleField_ValidArguments_OkObjectResult()
        {
            //Arrange
            SingleNumberField SINGLENUMBERFIELD = new SingleNumberField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = 123,
                FieldValueType = FieldType.Number
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(SINGLENUMBERFIELD);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateNumberSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, SINGLENUMBERFIELD);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<SingleNumberField>(f => f.FieldValueType == FieldType.Number)), Times.Once
            );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Number);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateNumberSingleField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            SingleNumberField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateNumberSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region MultiNumberField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateNumberMultiField_ValidArguments_OkObjectResult()
        {
            //Arrange
            MultiNumberField MULTINUMBERFIELD = new MultiNumberField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new long?[] { 123123123 },
                FieldValueType = FieldType.MultiNumber
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(MULTINUMBERFIELD);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateNumberMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, MULTINUMBERFIELD);

            //Assert
            fieldServiceMock.Verify(mock =>
                    mock.UpdateFieldValue(It.Is<MultiNumberField>(f => f.FieldValueType == FieldType.MultiNumber)), Times.Once
                );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as MultiNumberField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.Data.Any());
            Assert.True(fieldResult.FieldValueType == FieldType.MultiNumber);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateNumberMultiField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            MultiNumberField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateNumberMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region SingleDecimalField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDecimalSingleField_ValidArguments_OkObjectResult()
        {
            //Arrange
            SingleDecimalField singledecimalfield = new SingleDecimalField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = 123.456m,
                FieldValueType = FieldType.Decimal
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(singledecimalfield);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateDecimalSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, singledecimalfield);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<SingleDecimalField>(f => f.FieldValueType == FieldType.Decimal)), Times.Once
            );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Decimal);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDecimalSingleField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            SingleDecimalField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateDecimalSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region MultiDecimalField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDecimalMultiField_ValidArguments_OkObjectResult()
        {
            //Arrange
            MultiDecimalField multidecimalfield = new MultiDecimalField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new decimal?[] { 123.45m, 67.89m },
                FieldValueType = FieldType.MultiDecimal
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(multidecimalfield);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateDecimalMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, multidecimalfield);

            //Assert
            fieldServiceMock.Verify(mock =>
                    mock.UpdateFieldValue(It.Is<MultiDecimalField>(f => f.FieldValueType == FieldType.MultiDecimal)), Times.Once
                );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as MultiDecimalField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.Data.Any());
            Assert.True(fieldResult.FieldValueType == FieldType.MultiDecimal);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDecimalMultiField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            MultiDecimalField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateDecimalMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region SingleBooleanField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateBooleanSingleField_ValidArguments_OkObjectResult()
        {
            //Arrange
            SingleBooleanField singleBooleanField = new SingleBooleanField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = true,
                FieldValueType = FieldType.Boolean
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(singleBooleanField);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateBooleanSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, singleBooleanField);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<SingleBooleanField>(f => f.FieldValueType == FieldType.Boolean)), Times.Once
            );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Boolean);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateBooleanSingleField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            SingleBooleanField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateBooleanSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region SingleDateField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDateSingleField_ValidArguments_OkObjectResult()
        {
            //Arrange
            SingleDateField SINGLENUMBERFIELD = new SingleDateField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = DateTime.MinValue,
                FieldValueType = FieldType.Date
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(SINGLENUMBERFIELD);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateDateSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, SINGLENUMBERFIELD);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<SingleDateField>(f => f.FieldValueType == FieldType.Date && f.Data == DateTime.MinValue)), Times.Once
            );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Date);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDateSingleField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            SingleDateField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateDateSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region MultiDateField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDateMultiField_ValidArguments_OkObjectResult()
        {
            //Arrange
            MultiDateField MULTINUMBERFIELD = new MultiDateField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new List<DateTime?> { DateTime.MinValue },
                FieldValueType = FieldType.MultiDate
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(MULTINUMBERFIELD);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateDateMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, MULTINUMBERFIELD);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<MultiDateField>(f => f.FieldValueType == FieldType.MultiDate && f.Data.Any(d => d == DateTime.MinValue))), Times.Once
            );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.MultiDate);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateDateMultiField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            MultiDateField field = null;

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(field);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateDateMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region SingleRelationshipField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateRelationshipSingleField_ValidArguments_OkObjectResult()
        {
            //Arrange
            SingleRelationshipField singleRelationshipResponseField = new SingleRelationshipField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new Services.v1.Models.RelationshipElement { Dcv = DCVID, Name = "singleRelationshipFieldName", Icon = "singleRelationshipFieldIcon" },
                FieldValueType = FieldType.Relationship
            };

            RelationshipField singleRelationshipRequestField = new RelationshipField { Data = new Topic.v1.Models.RelationshipElement { Dcv = DCVID, Name = "singleRelationshipFieldName", Icon = "singleRelationshipFieldIcon" } };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(singleRelationshipResponseField);

            var fieldMapper = new Mock<IFieldMapper>();
            fieldMapper.Setup(x => x.MapField(It.IsAny<RelationshipField>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(singleRelationshipResponseField);
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateRelationshipSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, singleRelationshipRequestField);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<SingleRelationshipField>(f => f.FieldValueType == FieldType.Relationship && f.Data.Dcv == singleRelationshipRequestField.Data.Dcv
                && f.Data.Name == singleRelationshipRequestField.Data.Name && f.Data.Icon == singleRelationshipRequestField.Data.Icon)), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Relationship);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateRelationshipSingleField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            RelationshipField field = null;
            var fieldServiceMock = new Mock<IFieldService>();
            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateRelationshipSingleField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region MultiRelationshipField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateRelationshipMultiField_ValidArguments_OkObjectResult()
        {
            //Arrange
            Services.v1.Models.Fields.MultiRelationshipField multiRelationshipResponseField = new Services.v1.Models.Fields.MultiRelationshipField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new List<IRelationshipElement> { new Services.v1.Models.RelationshipElement { Dcv = DCVID, Name = "multiRelationshipFieldName", Icon = "multiRelationshipFieldIcon" } },
                FieldValueType = FieldType.Relationship
            };

            Topic.v1.Models.MultiRelationshipField multiRelationshipRequestField = new Topic.v1.Models.MultiRelationshipField { Data = new List<Topic.v1.Models.RelationshipElement> { new Topic.v1.Models.RelationshipElement { Dcv = DCVID, Name = "multiRelationshipFieldName", Icon = "multiRelationshipFieldIcon" } } };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(multiRelationshipResponseField);

            var fieldMapper = new Mock<IFieldMapper>();
            fieldMapper.Setup(x => x.MapField(It.IsAny<Topic.v1.Models.MultiRelationshipField>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(multiRelationshipResponseField);
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            var actionResult = await controller.UpdateRelationshipMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, multiRelationshipRequestField);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<Services.v1.Models.Fields.MultiRelationshipField>(f => f.FieldValueType == FieldType.Relationship)), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as IField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Relationship);
        }

        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateRelationshipMultiField_OneOrMoreEmptyArguments_BadRequestException()
        {
            //Arrange
            Topic.v1.Models.MultiRelationshipField field = null;
            var fieldServiceMock = new Mock<IFieldService>();
            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                        controller.UpdateRelationshipMultiField(DATABASEID, DataLanguages.en, DCVID, FIELDSETID, FIELDID, field));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }
        #endregion

        #region UpdateSingleHyperlinkField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateSingleHyperlinkField_ValidArguments_OkObjectResult()
        {
            //Arrange
            SingleHyperlinkField singleHyperlinkField = new SingleHyperlinkField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new Uri("https://www.mavim.com"),
                FieldValueType = FieldType.Hyperlink
            };

            PatchSingleHyperlinkField hyperlinkField = new PatchSingleHyperlinkField
            {
                Data = "https://www.mavim.com",
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(singleHyperlinkField);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);
            var dbid = Guid.Empty;

            //Act
            var actionResult = await controller.UpdateSingleHyperlinkField(dbid, DataLanguages.en, DCVID, FIELDSETID, FIELDID, hyperlinkField);

            //Assert
            fieldServiceMock.Verify(mock =>
                mock.UpdateFieldValue(It.Is<SingleHyperlinkField>(f => f.FieldValueType == FieldType.Hyperlink)), Times.Once
            );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as SingleHyperlinkField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.FieldValueType == FieldType.Hyperlink);
        }
        #endregion

        #region UpdateMultiHyperlinkField
        [Fact]
        [Trait("Category", "FieldSetField")]
        public async Task UpdateMultiHyperlinkField_ValidArguments_OkObjectResult()
        {
            //Arrange
            Services.v1.Models.Fields.MultiHyperlinkField serviceMultiHyperlinkField = new Services.v1.Models.Fields.MultiHyperlinkField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                TopicId = DCVID,
                Data = new[] { new Uri("https://www.mavim.com") },
                FieldValueType = FieldType.MultiHyperlink
            };

            MultiHyperlinkField multiHyperlinkField = new MultiHyperlinkField
            {
                Data = new List<string> { "https://www.mavim.com" },
            };

            var fieldServiceMock = new Mock<IFieldService>();
            fieldServiceMock.Setup(x => x.UpdateFieldValue(It.IsAny<IField>()))
                            .ReturnsAsync(serviceMultiHyperlinkField);

            var fieldMapper = new Mock<IFieldMapper>();
            var controller = new FieldSetController(fieldServiceMock.Object, fieldMapper.Object);
            var dbid = Guid.Empty;

            //Act
            var actionResult = await controller.UpdateMultiHyperlinkField(dbid, DataLanguages.en, DCVID, FIELDSETID, FIELDID, multiHyperlinkField);

            //Assert
            fieldServiceMock.Verify(mock =>
                    mock.UpdateFieldValue(It.Is<Services.v1.Models.Fields.MultiHyperlinkField>(f => f.FieldValueType == FieldType.MultiHyperlink)), Times.Once
                );

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldResult = okObjectResult.Value as Services.v1.Models.Fields.MultiHyperlinkField;
            Assert.NotNull(fieldResult);
            Assert.True(fieldResult.Data.Any());
            Assert.True(fieldResult.FieldValueType == FieldType.MultiHyperlink);
        }
        #endregion

        #region Theory data
        public static IEnumerable<object[]> InvalidUri
        {
            get
            {

                yield return new object[] { "mavim.com" };
                yield return new object[] { "test" };
            }
        }
        #endregion
    }
}