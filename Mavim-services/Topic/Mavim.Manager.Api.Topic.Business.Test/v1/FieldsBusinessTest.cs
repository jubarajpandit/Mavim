using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Business.v1;
using Mavim.Manager.Api.Topic.Business.v1.Models;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;
using Repo = Mavim.Manager.Api.Topic.Repository.v1;

namespace Mavim.Manager.Api.Topic.Businesss.Test.v1
{
    public class FieldsBusinessTest
    {
        private const string DCVID = "d12950883c414v0";
        private const string DCVID2 = "d12950883c414v1";
        private const string INVALID_DCVID = "d12950883c414v";
        private const string FIELDSETID = "d5926266c1352v0";
        private const string INVALID_FIELDSETID = "d5926266c1352v";
        private const string FIELDID = "d5926266c7221v0";
        private const string INVALID_FIELDID = "d5926266c7221v";

        [Theory, MemberData(nameof(ResponseFields))]
        [Trait("Category", "FieldsBusiness")]
        public async Task GetFields_ValidArguments_IENumerableIField(IField responseBusinessField, IRepo.Fields.IField responseRepoField)
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            IEnumerable<IRepo.Fields.IField> repoFields = new List<IRepo.Fields.IField>() { responseRepoField };
            IEnumerable<IField> expectedBusinessFields = new List<IField>() { responseBusinessField };
            fieldRepositoryMock.Setup(x => x.GetFields(It.IsAny<string>()))
                .ReturnsAsync(repoFields);
            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();
            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            var result = await business.GetFields(DCVID);

            //Assert
            fieldRepositoryMock.Verify(mock => mock.GetFields(DCVID), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IField>>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(expectedBusinessFields);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task GetFields_InvalidDcvId_ThrowsBadRequestException()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();
            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.GetFields(INVALID_DCVID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task GetField_WrongFieldValueType_ThrowsArgumentOutOfRangeException()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            fieldRepositoryMock.Setup(x => x.GetField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Repo.Fields.SingleTextField
                {
                    FieldId = FIELDID,
                    FieldName = "test",
                    FieldSetId = FIELDSETID,
                    SetName = "setName",
                    FieldValue = "Text",
                    FieldValueType = (IRepo.enums.FieldType)(-1),
                    Characteristic = new Repo.Models.RelationshipElement(),
                    OpenLocation = "openLocation",
                    RelationshipCategory = null,
                    Usage = "usage",
                    Required = true,
                    Readonly = false
                });

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();

            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.GetField(DCVID, FIELDSETID, FIELDID));

            // Assert
            // I expect ArgumentOutOfRangeException, because the enum type cannot be found
            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfArguments))]
        [Trait("Category", "FieldsBusiness")]
        public async Task GetField_InvalidCombinationOfArguments_ThrowsBadRequestException(string dcvId, string fieldSetId, string fieldId)
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            fieldRepositoryMock.Setup(x => x.GetField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Repo.Fields.SingleTextField());
            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();

            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.GetField(dcvId, fieldSetId, fieldId));

            // Assert
            // I expect ArgumentOutOfRangeException, because the enum type cannot be found
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Theory, MemberData(nameof(ResponseFields))]
        [Trait("Category", "FieldsBusiness")]
        public async Task GetField_ValidArguments_IField(IField responseBusinessField, IRepo.Fields.IField responseRepoField)
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            fieldRepositoryMock.Setup(x => x.GetField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseRepoField);

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();

            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            var result = await business.GetField(DCVID, FIELDSETID, FIELDID);

            //Assert
            fieldRepositoryMock.Verify(mock => mock.GetField(DCVID, FIELDSETID, FIELDID), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(responseBusinessField);
            Assert.Equal(expected, resultstring);
        }

        [Theory, MemberData(nameof(UpdateFieldsTheory))]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateField_ValidArguments_IField(IField requestBusinessField, IRepo.Fields.IField requestRepoField, IField responseBusinessField, IRepo.Fields.IField responseRepoField)
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            fieldRepositoryMock.Setup(x => x.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseRepoField);

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();
            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            Func<IRepo.Fields.IField, bool> validate = actualRequestRepoField =>
            {
                var actual = JsonConvert.SerializeObject(actualRequestRepoField);
                var expected = JsonConvert.SerializeObject(requestRepoField);
                Assert.Equal(expected, actual);
                return true;
            };

            //Act
            var result = await business.UpdateFieldValue(requestBusinessField);

            //Assert
            fieldRepositoryMock.Verify(mock => mock.UpdateFieldValue(It.Is<IRepo.Fields.IField>(x => validate(x)), requestBusinessField.TopicId, requestBusinessField.FieldSetId, requestBusinessField.FieldId), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(responseBusinessField);
            Assert.Equal(expected, resultstring);
        }


        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateField_NullField_ShouldThrowArgumentNullException()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            SingleTextField requestItem = null;

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();
            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValue(requestItem));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateField_MissingTopicId_ShouldThrowBadRequest()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            SingleTextField requestItem = new SingleTextField
            {
                FieldId = DCVID,
                FieldSetId = FIELDSETID,
                FieldValueType = FieldType.Text,
                Data = "test"
            };

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();
            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValue(requestItem));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Theory, MemberData(nameof(UpdateFieldsTheory))]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateFields_ValidArguments_IBulkResult(IField requestBusinessField, IRepo.Fields.IField requestRepoField, IField responseBusinessField, IRepo.Fields.IField responseRepoField)
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            fieldRepositoryMock.Setup(x => x.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseRepoField);

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();
            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);
            var businessFieldList = new List<IField>() { requestBusinessField };

            Func<IRepo.Fields.IField, bool> validate = actualRequestRepoField =>
            {
                var actual = JsonConvert.SerializeObject(actualRequestRepoField);
                var expected = JsonConvert.SerializeObject(requestRepoField);
                Assert.Equal(expected, actual);
                return true;
            };

            //Act
            var result = await business.UpdateFieldValues(businessFieldList);

            //Assert
            fieldRepositoryMock.Verify((mock) => mock.UpdateFieldValue(It.Is<IRepo.Fields.IField>(x => validate(x)), requestBusinessField.TopicId, requestBusinessField.FieldSetId, requestBusinessField.FieldId), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IBulkResult<IField>>(result);

            Assert.Equal(businessFieldList.Count, result.Succeeded.Count);
            Assert.Empty(result.Failed);

            var resultstring = JsonConvert.SerializeObject(result.Succeeded.First());
            var expected = JsonConvert.SerializeObject(responseBusinessField);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateFields_NullField_ShouldThrowArgumentNullException()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();
            List<IField> requestItems = null;

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();
            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await business.UpdateFieldValues(requestItems));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateFields_MissingTopicId_ShouldAddItemToFailed()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();

            List<IField> requestItems =
                new List<IField>
                {
                    new SingleTextField
                    {
                        FieldId = DCVID,
                        FieldSetId = FIELDSETID,
                        TopicId = DCVID,
                        FieldValueType = FieldType.Text,
                        Data = "Test1"
                    },
                    new SingleTextField
                    {
                        FieldId = DCVID2,
                        FieldSetId = FIELDSETID,
                        FieldValueType = FieldType.Text,
                        Data = "Test2"
                    }
                };

            Repo.Fields.SingleTextField responseItem = new Repo.Fields.SingleTextField
            {
                FieldId = FIELDID,
                FieldName = "test",
                FieldSetId = FIELDSETID,
                SetName = "setName",
                FieldValue = "Test1",
                FieldValueType = IRepo.enums.FieldType.Text,
                Characteristic = new Repo.Models.RelationshipElement(),
                OpenLocation = "openLocation",
                RelationshipCategory = new Repo.Models.RelationshipElement(),
                Usage = "usage",
                Required = true,
                Readonly = false
            };

            int expectedSucceededResult = 1;

            fieldRepositoryMock.Setup(x => x.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseItem);

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();

            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            IBulkResult<IField> result = await business.UpdateFieldValues(requestItems);

            //Assert
            fieldRepositoryMock.Verify((mock) => mock.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), DCVID, FIELDSETID, DCVID));
            Assert.Equal(expectedSucceededResult, result.Succeeded.Count);
            Assert.Single(result.Failed);
        }

        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateFields_MissingFieldId_ShouldAddItemToFailed()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();

            List<IField> requestItems =
                new List<IField>
                {
                    new SingleTextField
                    {
                        FieldId = DCVID,
                        FieldSetId = FIELDSETID,
                        TopicId = DCVID,
                        FieldValueType = FieldType.Text,
                        Data = "Test1"
                    },
                    new SingleTextField
                    {
                        FieldSetId = FIELDSETID,
                        TopicId = DCVID,
                        FieldValueType = FieldType.Text,
                        Data = "Test2"
                    }
                };

            Repo.Fields.SingleTextField responseItem = new Repo.Fields.SingleTextField
            {
                FieldId = FIELDID,
                FieldName = "test",
                FieldSetId = FIELDSETID,
                SetName = "setName",
                FieldValue = "Test1",
                FieldValueType = IRepo.enums.FieldType.Text,
                Characteristic = new Repo.Models.RelationshipElement(),
                OpenLocation = "openLocation",
                RelationshipCategory = new Repo.Models.RelationshipElement(),
                Usage = "usage",
                Required = true,
                Readonly = false
            };

            int expectedSucceededResult = 1;

            fieldRepositoryMock.Setup(x => x.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseItem);

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();

            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            IBulkResult<IField> result = await business.UpdateFieldValues(requestItems);

            //Assert
            fieldRepositoryMock.Verify((mock) => mock.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), DCVID, FIELDSETID, DCVID));
            Assert.Equal(expectedSucceededResult, result.Succeeded.Count);
            Assert.Single(result.Failed);
        }

        [Fact]
        [Trait("Category", "FieldsBusiness")]
        public async Task UpdateFields_MissingFieldSetId_ShouldAddItemToFailed()
        {
            //Arrange
            Mock<IRepo.Fields.IFieldRepository> fieldRepositoryMock = new Mock<IRepo.Fields.IFieldRepository>();

            List<IField> requestItems =
                new List<IField>
                {
                    new SingleTextField
                    {
                        FieldId = DCVID,
                        FieldSetId = FIELDSETID,
                        TopicId = DCVID,
                        FieldValueType = FieldType.Text,
                        Data = "Test1"
                    },
                    new SingleTextField
                    {
                        FieldId = DCVID2,
                        TopicId = DCVID,
                        FieldValueType = FieldType.Text,
                        Data = "Test2"
                    }
                };

            Repo.Fields.SingleTextField responseItem = new Repo.Fields.SingleTextField
            {
                FieldId = FIELDID,
                FieldName = "test",
                FieldSetId = FIELDSETID,
                SetName = "setName",
                FieldValue = "Test1",
                FieldValueType = IRepo.enums.FieldType.Text,
                Characteristic = new Repo.Models.RelationshipElement(),
                OpenLocation = "openLocation",
                RelationshipCategory = new Repo.Models.RelationshipElement(),
                Usage = "usage",
                Required = true,
                Readonly = false
            };

            int expectedSucceededResult = 1;

            fieldRepositoryMock.Setup(x => x.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseItem);

            Mock<ILogger<FieldsBusiness>> loggerMock = new Mock<ILogger<FieldsBusiness>>();

            FieldsBusiness business = new FieldsBusiness(fieldRepositoryMock.Object, loggerMock.Object);

            //Act
            IBulkResult<IField> result = await business.UpdateFieldValues(requestItems);

            //Assert
            fieldRepositoryMock.Verify((mock) => mock.UpdateFieldValue(It.IsAny<IRepo.Fields.IField>(), DCVID, FIELDSETID, DCVID));
            Assert.Equal(expectedSucceededResult, result.Succeeded.Count);
            Assert.Single(result.Failed);
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
                    new Repo.Fields.SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "123",
                        FieldValueType = IRepo.enums.FieldType.Number,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "123",
                        FieldValueType = IRepo.enums.FieldType.Number,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new string[] { "123", "456" },
                        FieldValueType = IRepo.enums.FieldType.MultiNumber,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new string[] { "123", "456" },
                        FieldValueType = IRepo.enums.FieldType.MultiNumber,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "123,45",
                        FieldValueType = IRepo.enums.FieldType.Decimal,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "123,45",
                        FieldValueType = IRepo.enums.FieldType.Decimal,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new string[] { "123,45", "456,45" },
                        FieldValueType = IRepo.enums.FieldType.MultiDecimal,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new string[] { "123,45", "456,45" },
                        FieldValueType = IRepo.enums.FieldType.MultiDecimal,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleBooleanField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = true,
                        FieldValueType = IRepo.enums.FieldType.Boolean,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.SingleBooleanField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = true,
                        FieldValueType = IRepo.enums.FieldType.Boolean,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "Test2",
                        FieldValueType = IRepo.enums.FieldType.Text,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "Test2",
                        FieldValueType = IRepo.enums.FieldType.Text,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<string> { "Test2" },
                        FieldValueType = IRepo.enums.FieldType.MultiText,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<string> { "Test2" },
                        FieldValueType = IRepo.enums.FieldType.MultiText,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = DateTime.MinValue,
                        FieldValueType = IRepo.enums.FieldType.Date,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = DateTime.MinValue,
                        FieldValueType = IRepo.enums.FieldType.Date,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = IRepo.enums.FieldType.MultiDate,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = IRepo.enums.FieldType.MultiDate,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = new Uri("https://www.mavim.com"),
                        FieldValueType = IRepo.enums.FieldType.Hyperlink,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = new Uri("https://www.mavim.com"),
                        FieldValueType = IRepo.enums.FieldType.Hyperlink,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = IRepo.enums.FieldType.MultiHyperlink,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                        TopicId = null,
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
                    new Repo.Fields.MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = IRepo.enums.FieldType.MultiHyperlink,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "123",
                        FieldValueType = IRepo.enums.FieldType.Number,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiNumberField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new string[] { "123", "456" },
                        FieldValueType = IRepo.enums.FieldType.MultiNumber,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "123,45",
                        FieldValueType = IRepo.enums.FieldType.Decimal,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new string[] { "123,45", "456,45" },
                        FieldValueType = IRepo.enums.FieldType.MultiDecimal,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = "Test2",
                        FieldValueType = IRepo.enums.FieldType.Text,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiTextField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<string> { "Test2" },
                        FieldValueType = IRepo.enums.FieldType.MultiText,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = DateTime.MinValue,
                        FieldValueType = IRepo.enums.FieldType.Date,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiDateField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<DateTime?> { DateTime.MinValue },
                        FieldValueType = IRepo.enums.FieldType.MultiDate,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValue = new Uri("https://www.mavim.com"),
                        FieldValueType = IRepo.enums.FieldType.Hyperlink,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
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
                    new Repo.Fields.MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        FieldName = "field1",
                        FieldSetId = FIELDSETID,
                        SetOrder = 1,
                        Order = 2,
                        SetName = "setName",
                        FieldValues = new List<Uri> { new Uri("https://www.mavim.com") },
                        FieldValueType = IRepo.enums.FieldType.MultiHyperlink,
                        Characteristic = new Repo.Models.RelationshipElement(),
                        OpenLocation = "openLocation",
                        RelationshipCategory = new Repo.Models.RelationshipElement(),
                        Usage = "usage",
                        Required = true,
                        Readonly = false
                    }
                };
            }
        }
    }
}
