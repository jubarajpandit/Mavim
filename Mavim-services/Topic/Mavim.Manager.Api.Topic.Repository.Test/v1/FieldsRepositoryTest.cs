using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1;
using Mavim.Manager.Api.Topic.Repository.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Factory;
using Mavim.Manager.Api.Topic.Repository.v1.Models;
using Mavim.Manager.Model;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Repository.Test.v1
{


    public class FieldsRepositoryTest
    {
        private const string TOPICID = "d12950883c414v0";
        private const string INVALID_DCVID = "d12950883c414v";
        private const string FIELDSETID = "d5926266c1352v0";
        private const string INVALID_FIELDSETID = "d5926266c1352v";
        private const string FIELDID = "d5926266c7221v0";
        private const string INVALID_FIELDID = "d5926266c7221v";
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddThh:mm" };

        [Theory, MemberData(nameof(SingleValueFields))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetField_ValidArguments_SingleValueField(IField field)
        {
            //Arrange
            var dataAccessMock = GetDataAccessMockWithSingleValueField(field);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            IField result = await repository.GetField(TOPICID, FIELDSETID, FIELDID);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result, _jsonSerializerSettings);
            var expected = JsonConvert.SerializeObject(field, _jsonSerializerSettings);
            Assert.Equal(expected, resultstring);
        }

        [Theory, MemberData(nameof(MultiValueFields))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetField_ValidArguments_MultiValueField(IField field)
        {
            //Arrange
            var dataAccessMock = GetDataAccessMockWithMultiValueField(field);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            IField result = await repository.GetField(TOPICID, FIELDSETID, FIELDID);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result, _jsonSerializerSettings);
            var expected = JsonConvert.SerializeObject(field, _jsonSerializerSettings);
            Assert.Equal(expected, resultstring);
        }

        [Theory, MemberData(nameof(ThreeInvalidCombinationOfArguments))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetField_InvalidCombinationOfArguments_ShouldThrowArgumentException(string dcvId, string fieldSetId, string fieldId)
        {
            // Arrange
            Mock<IMavimDbDataAccess> dataAccessMock = GetDataAccessMockWithSingleValueField();
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.GetField(dcvId, fieldSetId, fieldId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetField_InvalidDbHandle_ShouldThrowArgumentException()
        {
            // Arrange
            Mock<ISingleValueSimpleField> simpleField = GetSingleTextField();

            // FieldSet
            IElementFields fieldset = GetFieldSet(simpleField.Object);

            // Topic
            IDcvId topicDcv = DcvId.FromDcvKey(TOPICID);
            Mock<IElement> topicMock = GetMockTopic(fieldset);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(topicDcv, topicMock);

            // Data Model
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            topicMock.SetupGet(x => x.Model.MavimHandle).Returns(-1);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.GetField(TOPICID, FIELDSETID, FIELDID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetField_NullBaseTopic_ShouldThrowRequestNotFoundException()
        {
            // Arrange
            Mock<ISingleValueSimpleField> simpleField = GetSingleTextField();

            // FieldSet
            IElementFields fieldset = GetFieldSet(simpleField.Object);

            // Topic
            IDcvId topicDcv = DcvId.FromDcvKey(TOPICID);
            Mock<IElement> topicMock = GetMockTopic(fieldset);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(topicDcv, topicMock);

            // Data Model
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);


            mavimDataModelMock.Setup(model => model.ElementRepository
                                            .GetElement(It.IsAny<IDcvId>()))
                                            .Returns((IElement)null);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.GetField(TOPICID, FIELDSETID, FIELDID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetField_SimpleFieldNullName_ShouldMapToSetNameEmptyString()
        {
            // Arrange
            Mock<ISingleValueSimpleField> simpleField = GetSingleTextField();

            // FieldSet
            IElementFields fieldset = GetFieldSet(simpleField.Object);

            // Topic
            IDcvId topicDcv = DcvId.FromDcvKey(TOPICID);
            Mock<IElement> topicMock = GetMockTopic(fieldset);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(topicDcv, topicMock);

            // Data Model
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns((string)null);

            //Act
            IField result = await repository.GetField(TOPICID, FIELDSETID, FIELDID);

            //Assert
            Assert.Empty(result.SetName);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetField_SimpleFieldNonExistingEnum_Null()
        {
            // Arrange
            Mock<ISingleValueSimpleField> simpleField = GetSingleTextField();

            // FieldSet
            IElementFields fieldset = GetFieldSet(simpleField.Object);

            // Topic
            IDcvId topicDcv = DcvId.FromDcvKey(TOPICID);
            Mock<IElement> topicMock = GetMockTopic(fieldset);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(topicDcv, topicMock);

            // Data Model
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            FieldVarType invalidEnumType = (FieldVarType)(-1);

            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(invalidEnumType);

            //Act
            var result = await repository.GetField(TOPICID, FIELDSETID, FIELDID);

            // Assert
            Assert.Null(result);
        }

        [Theory, MemberData(nameof(SingleValueFields))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetFields_ValidArguments_SingleValueField(IField field)
        {
            // Arrange
            var dataAccessMock = GetDataAccessMockWithSingleValueField(field);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            IEnumerable<IField> result = await repository.GetFields(TOPICID);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IField>>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            IEnumerable<IField> listField = new List<IField>() { field };
            var expected = JsonConvert.SerializeObject(listField);
            Assert.Equal(expected, resultstring);
        }

        [Theory, MemberData(nameof(MultiValueFields))]
        [Trait("Category", "FieldsRepository")]
        public async Task GetFields_ValidArguments_MultiValueField(IField field)
        {
            // Arrange
            var dataAccessMock = GetDataAccessMockWithMultiValueField(field);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            IEnumerable<IField> result = await repository.GetFields(TOPICID);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IField>>(result);
            var resultstring = JsonConvert.SerializeObject(result, _jsonSerializerSettings);
            IEnumerable<IField> listField = new List<IField>() { field };
            var expected = JsonConvert.SerializeObject(listField, _jsonSerializerSettings);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetFields_InvalidFieldId_ArgumentException()
        {
            // Arrange
            var dataAccessMock = GetDataAccessMock(null);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            Exception result = await Record.ExceptionAsync(async () => await repository.GetFields(INVALID_FIELDSETID));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentException>(result);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetFields_NullFieldId_ArgumentNullException()
        {
            // Arrange
            var dataAccessMock = GetDataAccessMock(null);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            Exception result = await Record.ExceptionAsync(async () => await repository.GetFields(null));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentException>(result);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task GetFields_FieldsetNull_ShouldThrowException()
        {
            //Arrange
            Mock<ISingleValueSimpleField> simpleField = GetSingleTextField();

            // FieldSet
            IElementFields fieldset = GetFieldSet(simpleField.Object);

            // Topic
            IDcvId topicDcv = DcvId.FromDcvKey(TOPICID);
            Mock<IElement> topicMock = GetMockTopic(fieldset);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(topicDcv, topicMock);

            IElement nullElement = null;

            // for this test ensure to return null as fieldset result
            mavimDataModelMock
                .Setup(model => model.ElementRepository
                    .GetElement(It.Is<IDcvId>(d => d.Ver == topicDcv.Ver
                                                   && d.Cde == topicDcv.Cde
                                                   && d.Dbs == topicDcv.Dbs)))
                .Returns(nullElement);

            // Data Model
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.GetFields(FIELDSETID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal($"Could not find fields with FieldsetId '{FIELDSETID}'", exception.Message);
        }

        [Theory, MemberData(nameof(SingleValueFields))]
        [Trait("Category", "FieldsRepository")]
        public async Task UpdateFieldValue_ValidArguments_SingleValueField(IField field)
        {
            // Arrange
            var dataAccessMock = GetDataAccessMockWithSingleValueField(field);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            IField result = await repository.UpdateFieldValue(field, TOPICID, FIELDSETID, FIELDID);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(field);
            Assert.Equal(expected, resultstring);
        }

        [Theory, MemberData(nameof(MultiValueFields))]
        [Trait("Category", "FieldsRepository")]
        public async Task UpdateFieldValue_ValidArguments_MultiValueField(IField field)
        {
            // Arrange
            var dataAccessMock = GetDataAccessMockWithMultiValueField(field);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            IField result = await repository.UpdateFieldValue(field, TOPICID, FIELDSETID, FIELDID);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IField>(result);
            var resultstring = JsonConvert.SerializeObject(result, _jsonSerializerSettings);
            var expected = JsonConvert.SerializeObject(field, _jsonSerializerSettings);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task UpdateFieldValue_NullField_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dataAccessMock = GetDataAccessMock(null);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            //Act
            Exception result = await Record.ExceptionAsync(async () => await repository.UpdateFieldValue(null, TOPICID, FIELDSETID, FIELDID));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task UpdateFieldValue_InvalidDcvId_ShouldThrowArgumentException()
        {
            // Arrange
            var dataAccessMock = GetDataAccessMock(null);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            var field = SingleValueFields.First().First() as IField;

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.UpdateFieldValue(field, INVALID_DCVID, FIELDSETID, FIELDID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfArguments))]
        [Trait("Category", "FieldsRepository")]
        public async Task UpdateFieldValue_InvalidDcvId_ShouldThrowArgumentNullException(string fieldSetId, string fieldId)
        {
            // Arrange
            var dataAccessMock = GetDataAccessMock(null);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            var field = SingleValueFields.First().First() as IField;

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.UpdateFieldValue(field, TOPICID, fieldSetId, fieldId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task UpdateFieldValue_NullBaseTopic_ShouldThrowRequestNotFoundException()
        {
            // Arrange
            Mock<ISingleValueSimpleField> simpleField = GetSingleTextField();

            // FieldSet
            IElementFields fieldset = GetFieldSet(simpleField.Object);

            // Topic
            IDcvId topicDcv = DcvId.FromDcvKey(TOPICID);
            Mock<IElement> topicMock = GetMockTopic(fieldset);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(topicDcv, topicMock);

            // Data Model
            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            SingleTextField requestItem = new SingleTextField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                FieldValueType = FieldType.Text,
                FieldValue = "Data"
            };

            IElement nullElement = null;

            mavimDataModelMock.Setup(model => model.ElementRepository
                    .GetElement(It.IsAny<IDcvId>()))
                .Returns(nullElement);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.UpdateFieldValue(requestItem, TOPICID, FIELDSETID, INVALID_FIELDID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
        }

        [Fact]
        [Trait("Category", "FieldsRepository")]
        public async Task UpdateFieldValue_FieldTypesNotMatching_ShouldThrowRequestNotFoundException()
        {
            // Arrange
            SingleNumberField requestedField = new SingleNumberField()
            {
                FieldId = FIELDID,
                SetOrder = 1,
                Order = 2,
                FieldValue = "123",
                FieldValueType = FieldType.Number,
                FieldName = "field1",
                Required = true,
                SetName = "SingleNumberField",
                FieldSetId = FIELDSETID
            };

            SingleTextField suppliedField = new SingleTextField
            {
                FieldId = FIELDID,
                FieldSetId = FIELDSETID,
                FieldValueType = FieldType.Text,
            };

            var expected = $"Field with Id {suppliedField.FieldId} is not of type {suppliedField.FieldValueType}";
            var dataAccessMock = GetDataAccessMockWithSingleValueField(requestedField);
            var dataLanguageMock = GetDataLanguageMock();
            var featureManager = new Mock<IFeatureManager>();
            var loggerFactory = new Mock<ILoggerFactory>();
            var fieldMapperFactory = new FieldMapperFactory(featureManager.Object, loggerFactory.Object, dataAccessMock.Object);
            FieldsRepository repository = new FieldsRepository(dataAccessMock.Object, dataLanguageMock.Object, fieldMapperFactory);

            // Act
            Exception exception = await Record.ExceptionAsync(async () => await repository.UpdateFieldValue(suppliedField, TOPICID, FIELDSETID, FIELDID));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal(exception.Message, expected);
        }

        public static IEnumerable<object[]> ThreeInvalidCombinationOfArguments
        {
            get
            {
                yield return new object[] { INVALID_DCVID, FIELDSETID, FIELDID };
                yield return new object[] { TOPICID, INVALID_FIELDSETID, FIELDID };
                yield return new object[] { TOPICID, FIELDSETID, INVALID_FIELDID };
            }
        }

        public static IEnumerable<object[]> InvalidCombinationOfArguments
        {
            get
            {
                yield return new object[] { INVALID_FIELDSETID, FIELDID };
                yield return new object[] { FIELDSETID, INVALID_FIELDID };
            }
        }

        public static IEnumerable<object[]> SingleValueFields
        {
            get
            {
                yield return new object[]
                {
                    new SingleTextField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValue = "abc",
                        FieldValueType = FieldType.Text,
                        FieldName = "field1",
                        Required = true,
                        SetName = "SingleTextField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[] {
                    new SingleNumberField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValue = "123",
                        FieldValueType = FieldType.Number,
                        FieldName = "field1",
                        Required = true,
                        SetName = "SingleNumberField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[] {
                    new SingleDecimalField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValue = "123,45",
                        FieldValueType = FieldType.Decimal,
                        FieldName = "field1",
                        Required = true,
                        SetName = "SingleDecimalField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[]
                {
                    new SingleBooleanField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValue = true,
                        FieldValueType = FieldType.Boolean,
                        FieldName = "field1",
                        Required = true,
                        SetName = "SingleBooleanField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[]
                {
                    new SingleDateField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValue = DateTime.MinValue,
                        FieldValueType = FieldType.Date,
                        FieldName = "field1",
                        Required = true,
                        SetName = "SingleDateField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[]
                {
                    new SingleRelationshipField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValue = new RelationshipElement { Dcv = TOPICID, Icon = Resources.TreeIconID.TreeIconID_ArchiMate_ApplicationInterface.ToString("G"), Name = "SingleRelationshipFieldName" },
                        FieldValueType = FieldType.Relationship,
                        FieldName = "field1",
                        Required = true,
                        SetName = "SingleRelationshipField",
                        FieldSetId = FIELDSETID,
                        OpenLocation = "OpenLocation"
                    }
                };
                yield return new object[]
                {
                    new SingleHyperlinkField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValue = new Uri("https://www.mavim.com"),
                        FieldValueType = FieldType.Hyperlink,
                        FieldName = "field1",
                        Required = true,
                        SetName = "SingleHyperlinkField",
                        FieldSetId = FIELDSETID
                    }
                };
            }
        }

        public static IEnumerable<object[]> MultiValueFields
        {
            get
            {
                yield return new object[] {
                    new MultiTextField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValues = new string[] {"abc", "def" },
                        FieldValueType = FieldType.MultiText,
                        FieldName = "field1",
                        Required = true,
                        SetName = "MultiTextField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[] {
                    new MultiNumberField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValues = new string[] {"123", "456" },
                        FieldValueType = FieldType.MultiNumber,
                        FieldName = "field1",
                        Required = true,
                        SetName = "MultiNumberField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[] {
                    new MultiDecimalField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValues = new string[] {"123,45", "456,45" },
                        FieldValueType = FieldType.MultiDecimal,
                        FieldName = "field1",
                        Required = true,
                        SetName = "MultiDecimalField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[] {
                    new MultiDateField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValues = new DateTime?[] {DateTime.MinValue, DateTime.MaxValue },
                        FieldValueType = FieldType.MultiDate,
                        FieldName = "field1",
                        Required = true,
                        SetName = "MultiDateField",
                        FieldSetId = FIELDSETID
                    }
                };
                yield return new object[]
{
                    new MultiRelationshipField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValues = new List<RelationshipElement> { new RelationshipElement { Dcv = TOPICID, Icon = Resources.TreeIconID.TreeIconID_ArchiMate_ApplicationInterface.ToString("G"), Name = "MultiRelationshipFieldName" } },
                        FieldValueType = FieldType.MultiRelationship,
                        FieldName = "field1",
                        Required = true,
                        SetName = "MultiRelationshipField",
                        FieldSetId = FIELDSETID,
                        OpenLocation = "OpenLocation"
                    }
                };
                yield return new object[]
{
                    new MultiHyperlinkField()
                    {
                        FieldId = FIELDID,
                        SetOrder = 1,
                        Order = 2,
                        FieldValues = new List<Uri> { new Uri("https://www.mavim.com"), new Uri("https://www.mavim.com") },
                        FieldValueType = FieldType.MultiHyperlink,
                        FieldName = "field1",
                        Required = true,
                        SetName = "MultiHyperlinkField",
                        FieldSetId = FIELDSETID
                    }
                };
            }
        }

        private Mock<IMavimDbDataAccess> GetDataAccessMockWithSingleValueField(IField field = null)
        {
            var simpleField = GetSingleValueField(field);
            var fieldset = GetFieldSet(simpleField.Object);

            return GetDataAccessMock(fieldset);
        }

        private Mock<IDataLanguage> GetDataLanguageMock()
        {
            var mock = new Mock<IDataLanguage>();
            mock.Setup(x => x.Type).Returns(DataLanguageType.English);

            return mock;
        }

        private Mock<IMavimDbDataAccess> GetDataAccessMock(IElementFields fieldset)
        {
            // Topic
            var topicDcv = DcvId.FromDcvKey(TOPICID);
            var topicMock = GetMockTopic(fieldset);

            // Data Model
            var mavimDataModelMock = GetMockDataModel(topicDcv, topicMock);
            var dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel)
                .Returns(mavimDataModelMock.Object);

            return dataAccessMock;
        }

        private Mock<IMavimDbDataAccess> GetDataAccessMockWithMultiValueField(IField field = null)
        {
            var simpleField = GetMultiValueField(field);
            var fieldset = GetFieldSet(simpleField.Object);

            return GetDataAccessMock(fieldset);
        }

        private Mock<ISingleValueSimpleField> GetSingleValueField(IField field)
        {
            if (field == null)
            {
                return GetSingleTextField();
            }

            return field.FieldValueType switch
            {
                FieldType.Text => GetSingleTextField(),
                FieldType.Number => GetSingleNumberField(),
                FieldType.Decimal => GetSingleDecimalField(),
                FieldType.Boolean => GetSingleBooleanField(),
                FieldType.Date => GetSingleDateField(),
                FieldType.Relationship => GetSingleRelationshipField(),
                FieldType.Hyperlink => GetSingleHyperlinkField(),
                _ => null,
            };
        }

        private Mock<IMultiValueSimpleField> GetMultiValueField(IField field)
        {
            return field.FieldValueType switch
            {
                FieldType.MultiText => GetMultiTextField(),
                FieldType.MultiNumber => GetMultiNumberField(),
                FieldType.MultiDecimal => GetMultiDecimalField(),
                FieldType.MultiDate => GetMultiDateField(),
                FieldType.MultiRelationship => GetMultiRelationshipField(),
                FieldType.MultiHyperlink => GetMultiHyperlinkField(),
                _ => null,
            };
        }

        private Mock<ISingleValueSimpleField> GetSingleTextField()
        {
            Mock<ISingleValueSimpleField> simpleField = new Mock<ISingleValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("SingleTextField");
            simpleField.SetupGet(x => x.FieldValue).Returns("abc");
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Text);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<ISingleValueSimpleField> GetSingleNumberField()
        {
            Mock<ISingleValueSimpleField> simpleField = new Mock<ISingleValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("SingleNumberField");
            simpleField.SetupGet(x => x.FieldValue).Returns("123");
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Number);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<ISingleValueSimpleField> GetSingleDecimalField()
        {
            Mock<ISingleValueSimpleField> simpleField = new Mock<ISingleValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("SingleDecimalField");
            simpleField.SetupGet(x => x.FieldValue).Returns("123,45");
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Decimal);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<ISingleValueSimpleField> GetSingleBooleanField()
        {
            Mock<ISingleValueSimpleField> simpleField = new Mock<ISingleValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("SingleBooleanField");
            simpleField.SetupGet(x => x.FieldValue).Returns(true);
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Boolean);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<ISingleValueSimpleField> GetSingleDateField()
        {
            Mock<ISingleValueSimpleField> simpleField = new Mock<ISingleValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("SingleDateField");
            simpleField.SetupGet(x => x.FieldValue).Returns(DateTime.MinValue.ToString());
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Date);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<ISingleValueSimpleField> GetSingleRelationshipField()
        {
            var simpleField = new Mock<ISingleValueSimpleField>();
            var fieldRelationDefinitionMock = new Mock<IFieldRelationDefinition>();
            fieldRelationDefinitionMock.Setup(x => x.BrowseLocation.DcvID.ToString()).Returns("OpenLocation");
            simpleField.SetupGet(x => x.FieldDefinition).Returns(fieldRelationDefinitionMock.Object);
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("SingleRelationshipField");
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Relation);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);
            simpleField.SetupGet(x => x.FieldValue).Returns(GetMockTopicForRelation(TOPICID, "SingleRelationshipFieldName", Resources.TreeIconID.TreeIconID_ArchiMate_ApplicationInterface).Object);

            return simpleField;
        }

        private Mock<ISingleValueSimpleField> GetSingleHyperlinkField()
        {
            Mock<ISingleValueSimpleField> simpleField = new Mock<ISingleValueSimpleField>();
            Mock<IFieldTextDefinition> textFieldDefinition = new Mock<IFieldTextDefinition>();
            textFieldDefinition.SetupGet(x => x.UriFormat).Returns(true);
            simpleField.SetupGet(x => x.FieldDefinition).Returns(textFieldDefinition.Object);
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("SingleHyperlinkField");
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Text);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);
            simpleField.SetupGet(x => x.FieldValue).Returns("https://www.mavim.com");

            return simpleField;
        }

        private Mock<IMultiValueSimpleField> GetMultiTextField()
        {
            Mock<IMultiValueSimpleField> simpleField = new Mock<IMultiValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("MultiTextField");
            simpleField.SetupGet(x => x.FieldValues).Returns(new string[] { "abc", "def" });
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Text);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<IMultiValueSimpleField> GetMultiNumberField()
        {
            Mock<IMultiValueSimpleField> simpleField = new Mock<IMultiValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("MultiNumberField");
            simpleField.SetupGet(x => x.FieldValues).Returns(new string[] { "123", "456" });
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Number);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<IMultiValueSimpleField> GetMultiDecimalField()
        {
            Mock<IMultiValueSimpleField> simpleField = new Mock<IMultiValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("MultiDecimalField");
            simpleField.SetupGet(x => x.FieldValues).Returns(new string[] { "123,45", "456,45" });
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Decimal);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<IMultiValueSimpleField> GetMultiDateField()
        {
            Mock<IMultiValueSimpleField> simpleField = new Mock<IMultiValueSimpleField>();
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("MultiDateField");
            simpleField.SetupGet(x => x.FieldValues).Returns(new string[] {
                DateTime.MinValue.ToString(),
                DateTime.MaxValue.ToString()
            });
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Date);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private Mock<IMultiValueSimpleField> GetMultiRelationshipField()
        {
            Mock<IMultiValueSimpleField> simpleField = new Mock<IMultiValueSimpleField>();
            var fieldRelationDefinitionMock = new Mock<IFieldRelationDefinition>();
            fieldRelationDefinitionMock.Setup(x => x.BrowseLocation.DcvID.ToString()).Returns("OpenLocation");
            simpleField.SetupGet(x => x.FieldDefinition).Returns(fieldRelationDefinitionMock.Object);
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("MultiRelationshipField");
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Relation);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);
            simpleField.SetupGet(x => x.FieldValues).Returns(new List<IElement> { GetMockTopicForRelation(TOPICID, "MultiRelationshipFieldName", Resources.TreeIconID.TreeIconID_ArchiMate_ApplicationInterface).Object });

            return simpleField;
        }

        private Mock<IMultiValueSimpleField> GetMultiHyperlinkField()
        {
            Mock<IMultiValueSimpleField> simpleField = new Mock<IMultiValueSimpleField>();
            Mock<IFieldTextDefinition> textFieldDefinition = new Mock<IFieldTextDefinition>();
            textFieldDefinition.SetupGet(x => x.UriFormat).Returns(true);
            simpleField.SetupGet(x => x.FieldDefinition).Returns(textFieldDefinition.Object);
            simpleField.SetupGet(x => x.FieldDefinition.ID).Returns(FIELDID);
            simpleField.SetupGet(x => x.FieldSetDefinition.ID).Returns(FIELDSETID);
            simpleField.SetupGet(x => x.FieldSetDefinition.OrderNumber).Returns(1);
            simpleField.SetupGet(x => x.OrderNumber).Returns(2);
            simpleField.SetupGet(x => x.FieldDefinition.Name).Returns("field1");
            simpleField.SetupGet(x => x.FieldSetDefinition.Name).Returns("MultiHyperlinkField");
            simpleField.SetupGet(x => x.FieldValues).Returns(new string[] { "https://www.mavim.com", "https://www.mavim.com" });
            simpleField.SetupGet(x => x.FieldDefinition.VarType).Returns(FieldVarType.Text);
            simpleField.SetupGet(x => x.FieldDefinition.Required).Returns(true);
            simpleField.SetupGet(x => x.ReadOnly).Returns(false);

            return simpleField;
        }

        private static IElementFields GetFieldSet(ISimpleField field)
        {
            List<ISimpleField> simpleFields = new List<ISimpleField> { field };
            Mock<IFieldSet> fieldSet = new Mock<IFieldSet>();
            fieldSet.Setup(x => x.GetEnumerator()).Returns(() => simpleFields.GetEnumerator());
            fieldSet.SetupGet(x => x.Definition.ID).Returns(FIELDSETID);
            fieldSet.SetupGet(x => x.Definition.OrderNumber).Returns(1);

            List<IFieldSet> fieldsets = new List<IFieldSet> { fieldSet.Object };

            Mock<IElementFields> elementFields = new Mock<IElementFields>();
            elementFields.Setup(x => x.GetEnumerator()).Returns(() => fieldsets.GetEnumerator());
            return elementFields.Object;
        }

        private static Mock<IMavimDatabaseModel> GetMockDataModel(IDcvId topicDcv, Mock<IElement> topicMock)
        {
            Mock<IMavimDatabaseModel> mavimDataModelMock = new Mock<IMavimDatabaseModel>();

            mavimDataModelMock
                .Setup(model => model.ElementRepository
                .GetElement(It.Is<IDcvId>(d => d.Ver == topicDcv.Ver
                                            && d.Cde == topicDcv.Cde
                                            && d.Dbs == topicDcv.Dbs)))
                .Returns(topicMock.Object);

            return mavimDataModelMock;
        }

        private static Mock<IElement> GetMockTopic(IElementFields fieldSetMock, string name = "", string icon = "")
        {
            Mock<IElement> topicMock = new Mock<IElement>();
            Mock<IMavimDatabaseModel> dbMock = new Mock<IMavimDatabaseModel>();
            Mock<IDcvId> dcvIdMock = new Mock<IDcvId>();
            dcvIdMock.SetupGet(dcvId => dcvId.Dbs).Returns(12950883);
            dcvIdMock.SetupGet(dcvId => dcvId.Cde).Returns(414);
            dcvIdMock.SetupGet(dcvId => dcvId.Ver).Returns(0);

            Mock<IElementType> typeMock = new Mock<IElementType>();
            typeMock.SetupGet(type => type.HasSystemName).Returns(false);
            typeMock.SetupGet(type => type.IsImportedVersionsRoot).Returns(false);
            typeMock.SetupGet(type => type.IsImportedVersion).Returns(false);
            typeMock.SetupGet(type => type.IsCreatedVersionsRoot).Returns(false);
            typeMock.SetupGet(type => type.IsCreatedVersion).Returns(false);
            typeMock.SetupGet(type => type.IsRecycleBin).Returns(false);
            typeMock.SetupGet(type => type.IsRelationshipsCategoriesRoot).Returns(false);
            typeMock.SetupGet(type => type.IsExternalReferencesRoot).Returns(false);
            typeMock.SetupGet(type => type.IsObjectsRoot).Returns(false);

            topicMock
                .SetupGet(x => x.Model).Returns(dbMock.Object);
            topicMock
                .SetupGet(x => x.Model.MavimHandle).Returns(1);
            topicMock
                .SetupProperty(topic => topic.Model.DataLanguage).SetReturnsDefault(new Mock<ILanguage>().Object);
            topicMock
                .Setup(topic => topic.GetFields(It.IsAny<ILanguage>())).Returns(fieldSetMock);
            topicMock.SetupGet(topic => topic.DcvID).Returns(dcvIdMock.Object);
            topicMock.SetupGet(topic => topic.Type).Returns(typeMock.Object);
            topicMock.SetupGet(topic => topic.IsDeleted).Returns(false);
            topicMock.SetupGet(topic => topic.Name).Returns(name);

            return topicMock;
        }

        private static Mock<IElement> GetMockTopicForRelation(string dcv, string name, Resources.TreeIconID icon)
        {
            Mock<IElement> topicMock = new Mock<IElement>();
            Mock<IMavimDatabaseModel> dbMock = new Mock<IMavimDatabaseModel>();
            Mock<IDcvId> dcvIdMock = new Mock<IDcvId>();
            Mock<IElementVisual> elementVisualMock = new Mock<IElementVisual>();
            elementVisualMock.SetupGet(visual => visual.IconResourceID).Returns(icon);


            Mock<IElementType> typeMock = new Mock<IElementType>();
            typeMock.SetupGet(type => type.HasSystemName).Returns(false);
            typeMock.SetupGet(type => type.IsImportedVersionsRoot).Returns(false);
            typeMock.SetupGet(type => type.IsImportedVersion).Returns(false);
            typeMock.SetupGet(type => type.IsCreatedVersionsRoot).Returns(false);
            typeMock.SetupGet(type => type.IsCreatedVersion).Returns(false);
            typeMock.SetupGet(type => type.IsRecycleBin).Returns(false);
            typeMock.SetupGet(type => type.IsRelationshipsCategoriesRoot).Returns(false);
            typeMock.SetupGet(type => type.IsExternalReferencesRoot).Returns(false);
            typeMock.SetupGet(type => type.IsObjectsRoot).Returns(false);

            topicMock
                .SetupGet(x => x.Model).Returns(dbMock.Object);
            topicMock
                .SetupGet(x => x.Model.MavimHandle).Returns(1);
            topicMock
                .SetupProperty(topic => topic.Model.DataLanguage).SetReturnsDefault(new Mock<ILanguage>().Object);
            topicMock.SetupGet(topic => topic.DcvID).Returns(dcvIdMock.Object);
            topicMock.Setup(topic => topic.DcvID.ToString()).Returns(dcv);
            topicMock.SetupGet(topic => topic.Type).Returns(typeMock.Object);
            topicMock.SetupGet(topic => topic.IsDeleted).Returns(false);
            topicMock.SetupGet(topic => topic.Name).Returns(name);
            topicMock.SetupGet(topic => topic.Visual).Returns(elementVisualMock.Object);

            return topicMock;
        }
    }
}
