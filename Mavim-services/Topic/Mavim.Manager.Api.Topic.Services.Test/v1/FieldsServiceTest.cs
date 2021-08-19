using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1;
using Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.Test.v1
{
    public class FieldsServiceTest
    {
        private const string DCVID = "d12950883c414v0";
        private const string INVALID_DCVID = "d12950883c414v";
        private const string FIELDSETID = "d5926266c1352v0";
        private const string INVALID_FIELDSETID = "d5926266c1352v";
        private const string FIELDID = "d5926266c7221v0";
        private const string INVALID_FIELDID = "d5926266c7221v";

        [Theory, MemberData(nameof(ResponseFields))]
        [Trait("Category", "FieldsService")]
        public async Task GetFields_ValidArguments_IENumerableIField(IField responseServiceField, IBusiness.Fields.IField responseBusinessField)
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            IEnumerable<IBusiness.Fields.IField> repoFields = new List<IBusiness.Fields.IField>() { responseBusinessField };
            IEnumerable<IField> expectedServiceFields = new List<IField>() { responseServiceField };
            fieldBusinessMock.Setup(x => x.GetFields(It.IsAny<string>()))
                .ReturnsAsync(repoFields);
            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            var result = await business.GetFields(DCVID);

            //Assert
            fieldBusinessMock.Verify(mock => mock.GetFields(DCVID), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IField>>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(expectedServiceFields);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task GetFields_InvalidDcvId_ThrowsBadRequestException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.GetFields(INVALID_DCVID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task GetField_WrongFieldValueType_ThrowsArgumentOutOfRangeException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            fieldBusinessMock.Setup(x => x.GetField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Business.v1.Models.Fields.SingleTextField
                {
                    FieldId = FIELDID,
                    FieldName = "test",
                    FieldSetId = FIELDSETID,
                    SetName = "setName",
                    Data = "Text",
                    FieldValueType = (IBusiness.enums.FieldType)(-1),
                    Characteristic = new Business.v1.Models.RelationshipElement(),
                    OpenLocation = "openLocation",
                    RelationshipCategory = null,
                    Usage = "usage",
                    Required = true,
                    Readonly = false
                });

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();

            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.GetField(DCVID, FIELDSETID, FIELDID));

            // Assert
            // I expect ArgumentOutOfRangeException, because the enum type cannot be found
            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfArguments))]
        [Trait("Category", "FieldsService")]
        public async Task GetField_InvalidCombinationOfArguments_ThrowsBadRequestException(string dcvId, string fieldSetId, string fieldId)
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            fieldBusinessMock.Setup(x => x.GetField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Business.v1.Models.Fields.SingleTextField());
            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();

            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.GetField(dcvId, fieldSetId, fieldId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Theory, MemberData(nameof(ResponseFields))]
        [Trait("Category", "FieldsService")]
        public async Task GetField_ValidArguments_IField(IField responseServiceField, IBusiness.Fields.IField responseBusinessField)
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            fieldBusinessMock.Setup(x => x.GetField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseBusinessField);

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();

            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            var result = await business.GetField(DCVID, FIELDSETID, FIELDID);

            //Assert
            fieldBusinessMock.Verify(mock => mock.GetField(DCVID, FIELDSETID, FIELDID), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(responseServiceField);
            Assert.Equal(expected, resultstring);
        }

        [Theory, MemberData(nameof(UpdateFieldsTheory))]
        [Trait("Category", "FieldsService")]
        public async Task UpdateField_ValidArguments_IField(IField requestServiceField, IBusiness.Fields.IField requestBusinessField, IField responseServiceField, IBusiness.Fields.IField responseBusinessField)
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            Mock<IBusiness.ITopic> topicMock = new Mock<IBusiness.ITopic>();
            topicMock.Setup(x => x.IsReadOnly).Returns(false);
            topicBusinessMock.Setup(x => x.GetTopic(It.IsAny<string>())).ReturnsAsync(topicMock.Object);
            fieldBusinessMock.Setup(x => x.UpdateFieldValue(It.IsAny<IBusiness.Fields.IField>()))
                .ReturnsAsync(responseBusinessField);

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            Func<IBusiness.Fields.IField, bool> validate = actualRequestBusinessField =>
            {
                var actual = JsonConvert.SerializeObject(actualRequestBusinessField);
                var expected = JsonConvert.SerializeObject(requestBusinessField);
                Assert.Equal(expected, actual);
                return true;
            };

            //Act
            var result = await business.UpdateFieldValue(requestServiceField);

            //Assert
            fieldBusinessMock.Verify(mock => mock.UpdateFieldValue(It.Is<IBusiness.Fields.IField>(x => validate(x))), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(responseServiceField);
            Assert.Equal(expected, resultstring);
        }


        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task UpdateField_NullField_ShouldThrowArgumentNullException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            SingleTextField requestItem = null;

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValue(requestItem));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task UpdateField_MissingTopicId_ShouldThrowBadRequest()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            SingleTextField requestItem = new SingleTextField
            {
                FieldId = DCVID,
                FieldSetId = FIELDSETID,
                FieldValueType = FieldType.Text,
                Data = "test"
            };

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValue(requestItem));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task UpdateField_NullTopic_ShouldThrowForbiddenRequestException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            Mock<IField> mockField = new Mock<IField>();
            mockField.Setup(x => x.TopicId).Returns(DCVID);
            IField requestField = mockField.Object;

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValue(requestField));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task UpdateField_ReadOnlyTopic_ShouldThrowForbiddenRequestException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            Mock<IBusiness.ITopic> topicMock = new Mock<IBusiness.ITopic>();
            topicMock.Setup(x => x.IsReadOnly).Returns(true);
            topicBusinessMock.Setup(x => x.GetTopic(It.IsAny<string>())).ReturnsAsync(topicMock.Object);
            Mock<IField> mockField = new Mock<IField>();
            mockField.Setup(x => x.TopicId).Returns(DCVID);
            IField requestField = mockField.Object;

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValue(requestField));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }

        [Theory, MemberData(nameof(UpdateFieldsTheory))]
        [Trait("Category", "FieldsService")]
        public async Task UpdateFields_ValidArguments_IBulkResult(IField requestServiceField, IBusiness.Fields.IField requestBusinessField, IField responseServiceField, IBusiness.Fields.IField responseBusinessField)
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            Mock<IBusiness.ITopic> topicMock = new Mock<IBusiness.ITopic>();
            topicMock.Setup(x => x.IsReadOnly).Returns(false);
            Mock<IBusiness.Fields.IBulkResult<IBusiness.Fields.IField>> businessBulkResult = new Mock<IBusiness.Fields.IBulkResult<IBusiness.Fields.IField>>();
            topicBusinessMock.Setup(x => x.GetTopic(It.IsAny<string>())).ReturnsAsync(topicMock.Object);
            businessBulkResult.Setup(x => x.Succeeded).Returns(new List<IBusiness.Fields.IField>() { responseBusinessField });
            businessBulkResult.Setup(x => x.Failed).Returns(new List<IBusiness.Fields.IFailed<IBusiness.Fields.IField>>());
            fieldBusinessMock.Setup(x => x.UpdateFieldValues(It.IsAny<List<IBusiness.Fields.IField>>())).ReturnsAsync(businessBulkResult.Object);

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);
            var businessFieldList = new List<IField>() { requestServiceField };

            Func<List<IBusiness.Fields.IField>, bool> validate = actualRequestBusinessField =>
            {
                var actual = JsonConvert.SerializeObject(actualRequestBusinessField.First());
                var expected = JsonConvert.SerializeObject(requestBusinessField);
                Assert.Equal(expected, actual);
                return true;
            };

            //Act
            var result = await business.UpdateFieldValues(businessFieldList);

            //Assert
            fieldBusinessMock.Verify((mock) => mock.UpdateFieldValues(It.Is<List<IBusiness.Fields.IField>>(x => validate(x))), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IBulkResult<IField>>(result);

            Assert.Equal(businessFieldList.Count, result.Succeeded.Count);
            Assert.Empty(result.Failed);

            var resultstring = JsonConvert.SerializeObject(result.Succeeded.First());
            var expected = JsonConvert.SerializeObject(responseServiceField);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task UpdateFields_NullField_ShouldThrowArgumentNullException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            List<IField> requestItems = null;

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValues(requestItems));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task UpdateFields_NullTopic_ShouldThrowForbiddenRequestException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            List<IField> requestItems = new List<IField>();

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValues(requestItems));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsService")]
        public async Task UpdateFields_ReadOnlyTopic_ShouldThrowForbiddenRequestException()
        {
            //Arrange
            Mock<IBusiness.Fields.IFieldBusiness> fieldBusinessMock = new Mock<IBusiness.Fields.IFieldBusiness>();
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            Mock<IBusiness.ITopic> topicMock = new Mock<IBusiness.ITopic>();
            topicMock.Setup(x => x.IsReadOnly).Returns(true);
            topicBusinessMock.Setup(x => x.GetTopic(It.IsAny<string>())).ReturnsAsync(topicMock.Object);
            List<IField> requestItems = new List<IField>();

            Mock<ILogger<FieldsService>> loggerMock = new Mock<ILogger<FieldsService>>();
            FieldsService business = new FieldsService(fieldBusinessMock.Object, topicBusinessMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValues(requestItems));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }

        public static IEnumerable<object[]> InvalidCombinationOfArguments
        {
            get
            {
                yield return new object[] { INVALID_DCVID, FIELDSETID, FIELDID };
                yield return new object[] { DCVID, INVALID_FIELDSETID, FIELDID };
                yield return new object[] { DCVID, FIELDSETID, INVALID_FIELDID };
            }
        }

        public static IEnumerable<object[]> UpdateFieldsTheory
        {
            get
            {
                yield return new object[] {
                    new SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123,
                        FieldValueType = FieldType.Number,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123,
                        FieldValueType = IBusiness.enums.FieldType.Number,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123,
                        FieldValueType = FieldType.Number,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123,
                        FieldValueType = IBusiness.enums.FieldType.Number,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new long?[] { 123, 456 },
                        FieldValueType = FieldType.MultiNumber,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new long?[] { 123, 456 },
                        FieldValueType = IBusiness.enums.FieldType.MultiNumber,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new long?[] { 123, 456 },
                        FieldValueType = FieldType.MultiNumber,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new long?[] { 123, 456 },
                        FieldValueType = IBusiness.enums.FieldType.MultiNumber,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123.45m,
                        FieldValueType = FieldType.Decimal,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123.45m,
                        FieldValueType = IBusiness.enums.FieldType.Decimal,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123.45m,
                        FieldValueType = FieldType.Decimal,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123.45m,
                        FieldValueType = IBusiness.enums.FieldType.Decimal,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new decimal?[] { 123.45m, 456.45m },
                        FieldValueType = FieldType.MultiDecimal,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new decimal?[] { 123.45m, 456.45m },
                        FieldValueType = IBusiness.enums.FieldType.MultiDecimal,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new decimal?[] { 123.45m, 456.45m },
                        FieldValueType = FieldType.MultiDecimal,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new decimal?[] { 123.45m, 456.45m },
                        FieldValueType = IBusiness.enums.FieldType.MultiDecimal,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new SingleBooleanField
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = true,
                        FieldValueType = FieldType.Boolean,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleBooleanField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = true,
                        FieldValueType = IBusiness.enums.FieldType.Boolean,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new SingleBooleanField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = true,
                        FieldValueType = FieldType.Boolean,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleBooleanField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = true,
                        FieldValueType = IBusiness.enums.FieldType.Boolean,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }



                };
                yield return new object[]
                {
                    new SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = "Test2",
                        FieldValueType = FieldType.Text,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = "Test2",
                        FieldValueType = IBusiness.enums.FieldType.Text,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = "Test2",
                        FieldValueType = FieldType.Text,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = "Test2",
                        FieldValueType = IBusiness.enums.FieldType.Text,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<string> { "Test2" },
                        FieldValueType = FieldType.MultiText,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<string> { "Test2" },
                        FieldValueType = IBusiness.enums.FieldType.MultiText,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<string> { "Test2" },
                        FieldValueType = FieldType.MultiText,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<string> { "Test2" },
                        FieldValueType = IBusiness.enums.FieldType.MultiText,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = DateTime.MinValue,
                        FieldValueType = FieldType.Date,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = DateTime.MinValue,
                        FieldValueType = IBusiness.enums.FieldType.Date,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = DateTime.MinValue,
                        FieldValueType = FieldType.Date,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = DateTime.MinValue,
                        FieldValueType = IBusiness.enums.FieldType.Date,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
{
                    new MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = FieldType.MultiDate,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = IBusiness.enums.FieldType.MultiDate,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = FieldType.MultiDate,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = IBusiness.enums.FieldType.MultiDate,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new SingleRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new RelationshipElement { Dcv = DCVID, Icon = "singleRelationshipFieldIcon", Name = "singleRelationshipFieldName" },
                        FieldValueType = FieldType.Relationship,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Business.v1.Models.RelationshipElement { Dcv = DCVID, Icon = "singleRelationshipFieldIcon", Name = "singleRelationshipFieldName" },
                        FieldValueType = IBusiness.enums.FieldType.Relationship,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new SingleRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new RelationshipElement { Dcv = DCVID, Icon = "singleRelationshipFieldIcon", Name = "singleRelationshipFieldName" },
                        FieldValueType = FieldType.Relationship,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Business.v1.Models.RelationshipElement { Dcv = DCVID, Icon = "singleRelationshipFieldIcon", Name = "singleRelationshipFieldName" },
                        FieldValueType = IBusiness.enums.FieldType.Relationship,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
{
                    new MultiRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<RelationshipElement> { new RelationshipElement { Dcv = DCVID, Icon = "multiRelationshipFieldIcon", Name = "multiRelationshipFieldName" } },
                        FieldValueType = FieldType.MultiRelationship,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Business.v1.Models.RelationshipElement> { new Business.v1.Models.RelationshipElement { Dcv = DCVID, Icon = "multiRelationshipFieldIcon", Name = "multiRelationshipFieldName" } },
                        FieldValueType = IBusiness.enums.FieldType.MultiRelationship,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new MultiRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<RelationshipElement> { new RelationshipElement { Dcv = DCVID, Icon = "multiRelationshipFieldIcon", Name = "multiRelationshipFieldName" } },
                        FieldValueType = FieldType.MultiRelationship,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Business.v1.Models.RelationshipElement> { new Business.v1.Models.RelationshipElement { Dcv = DCVID, Icon = "multiRelationshipFieldIcon", Name = "multiRelationshipFieldName" } },
                        FieldValueType = IBusiness.enums.FieldType.MultiRelationship,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Uri("https://www.mavim.com"),
                        FieldValueType = FieldType.Hyperlink,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Uri("https://www.mavim.com"),
                        FieldValueType = IBusiness.enums.FieldType.Hyperlink,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Uri("https://www.mavim.com"),
                        FieldValueType = FieldType.Hyperlink,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Uri("https://www.mavim.com"),
                        FieldValueType = IBusiness.enums.FieldType.Hyperlink,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = FieldType.MultiHyperlink,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = IBusiness.enums.FieldType.MultiHyperlink,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = FieldType.MultiHyperlink,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = IBusiness.enums.FieldType.MultiHyperlink,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
            }
        }

        public static IEnumerable<object[]> ResponseFields
        {
            get
            {
                yield return new object[] {
                    new SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123,
                        FieldValueType = FieldType.Number,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123,
                        FieldValueType = IBusiness.enums.FieldType.Number,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new long?[] { 123, 456 },
                        FieldValueType = FieldType.MultiNumber,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new long?[] { 123, 456 },
                        FieldValueType = IBusiness.enums.FieldType.MultiNumber,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123.45m,
                        FieldValueType = FieldType.Decimal,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = 123.45m,
                        FieldValueType = IBusiness.enums.FieldType.Decimal,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new decimal?[] { 123.45m, 456.45m },
                        FieldValueType = FieldType.MultiDecimal,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new decimal?[] { 123.45m, 456.45m },
                        FieldValueType = IBusiness.enums.FieldType.MultiDecimal,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = "Test2",
                        FieldValueType = FieldType.Text,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = "Test2",
                        FieldValueType = IBusiness.enums.FieldType.Text,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<string> { "Test2" },
                        FieldValueType = FieldType.MultiText,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<string> { "Test2" },
                        FieldValueType = IBusiness.enums.FieldType.MultiText,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = DateTime.MinValue,
                        FieldValueType = FieldType.Date,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = DateTime.MinValue,
                        FieldValueType = IBusiness.enums.FieldType.Date,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = FieldType.MultiDate,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = IBusiness.enums.FieldType.MultiDate,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new SingleRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new RelationshipElement { Dcv = DCVID, Icon = "singleRelationshipFieldIcon", Name = "singleRelationshipFieldName" },
                        FieldValueType = FieldType.Relationship,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Business.v1.Models.RelationshipElement { Dcv = DCVID, Icon = "singleRelationshipFieldIcon", Name = "singleRelationshipFieldName" },
                        FieldValueType = IBusiness.enums.FieldType.Relationship,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[]
                {
                    new MultiRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<RelationshipElement> { new RelationshipElement { Dcv = DCVID, Icon = "multiRelationshipFieldIcon", Name = "multiRelationshipFieldName" } },
                        FieldValueType = FieldType.MultiRelationship,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiRelationshipField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Business.v1.Models.RelationshipElement> {  new Business.v1.Models.RelationshipElement { Dcv = DCVID, Icon = "multiRelationshipFieldIcon", Name = "multiRelationshipFieldName" } },
                        FieldValueType = IBusiness.enums.FieldType.MultiRelationship,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Uri("https://www.mavim.com"),
                        FieldValueType = FieldType.Hyperlink,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new Uri("https://www.mavim.com"),
                        FieldValueType = IBusiness.enums.FieldType.Hyperlink,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
                yield return new object[] {
                    new MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = FieldType.MultiHyperlink,
                        Characteristic = new RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    },
                    new Business.v1.Models.Fields.MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        TopicId = DCVID,
                        SetName = "setName",
                        Data = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = IBusiness.enums.FieldType.MultiHyperlink,
                        Characteristic = new Business.v1.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Business.v1.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
            }
        }
    }
}
