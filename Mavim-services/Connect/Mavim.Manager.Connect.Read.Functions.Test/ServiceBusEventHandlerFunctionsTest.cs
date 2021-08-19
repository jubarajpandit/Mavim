using Azure.Messaging.ServiceBus;
using Mavim.Manager.Connect.Read.Functions.Clients;
using Mavim.Manager.Connect.Read.Functions.Constants;
using Mavim.Manager.Connect.Read.Functions.Constants.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Mavim.Manager.Connect.Read.Functions.Test
{
    public class ServiceBusEventHandlerFunctionsTest
    {

        #region CompanyMessages
        [Theory, MemberData(nameof(ValidCompanyMessages))]
        public async void HandleMessage_ValidCompanyMessage_CompletedMessage(ServiceBusReceivedMessage serviceBusReceiveMessage, string route, HttpMethod method)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var action = await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object);

            // Assert
            mockClient.Verify(x => x.SendRequestAsync(route, method, It.IsAny<object>(), It.IsAny<ILogger>()), Times.Once);
            Assert.Equal(MessageActions.Complete, action);
        }

        [Theory, MemberData(nameof(InValidCompanyMessages))]
        public async void HandleMessage_InValidCompanyMessage_NotImplementedException(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotImplementedException>(result);
        }

        [Theory, MemberData(nameof(UnknownCompanyMessage))]
        public async void HandleMessage_UnknownCompanyMessage_DeadletterMessage(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var action = await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object);

            // Assert
            Assert.Equal(MessageActions.DeadLetter, action);
        }
        #endregion

        #region GroupMessages
        [Theory, MemberData(nameof(ValidGroupMessages))]
        public async void HandleMessage_ValidGroupMessages_CompletedMessage(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var action = await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object);

            // Assert
            Assert.Equal(MessageActions.Complete, action);
        }

        [Theory, MemberData(nameof(UnknownGroupMessages))]
        public async void HandleMessage_UnknownGroupMessage_DeadletterMessage(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var action = await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object);

            // Assert
            Assert.Equal(MessageActions.DeadLetter, action);
        }
        #endregion

        #region UserMessages
        [Theory, MemberData(nameof(ValidUserMessages))]
        public async void HandleMessage_ValidUserMessages_CompletedMessage(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var action = await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object);

            // Assert
            Assert.Equal(MessageActions.Complete, action);
        }

        [Theory, MemberData(nameof(InValidUserMessages))]
        public async void HandleMessage_InValidUserMessages_NotImplementedException(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotImplementedException>(result);
        }

        [Theory, MemberData(nameof(UnknownUserMessages))]
        public async void HandleMessage_UnknownUserMessages_DeadletterMessage(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockResponse = new Mock<HttpResponseMessage>(HttpStatusCode.NoContent);
            var mockClient = new Mock<IConnectHttpClient>();
            mockClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(mockResponse.Object);

            // Act
            var action = await ServiceBusEventHandlerFunctions.HandleMessage(serviceBusReceiveMessage, mockLogger.Object, mockClient.Object);

            // Assert
            Assert.Equal(MessageActions.DeadLetter, action);
        }
        #endregion

        #region TestData
        public static IEnumerable<object[]> ValidCompanyMessages
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.Company;
                var entityModelVersion = 1;
                var payload = GetCompanyPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Create, aggregateId, entityId, entityType, entityModelVersion, companyId), Endpoints.AddCompany, HttpMethod.Post };
            }
        }

        public static IEnumerable<object[]> InValidCompanyMessages
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.Company;
                var entityModelVersion = 1;
                var payload = GetCompanyPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.AddPartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Delete, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.RemovePartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Update, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> UnknownCompanyMessage
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.Company;
                var entityModelVersion = 1;
                var payload = GetCompanyPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Unknown, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> ValidGroupMessages
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.Group;
                var entityModelVersion = 1;
                var payload = GetGroupPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Create, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.AddPartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Delete, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.RemovePartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Update, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> UnknownGroupMessages
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.Group;
                var entityModelVersion = 1;
                var payload = GetGroupPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Unknown, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> ValidUserMessages
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.User;
                var entityModelVersion = 1;
                var payload = GetUserPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Create, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Delete, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> InValidUserMessages
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.User;
                var entityModelVersion = 1;
                var payload = GetUserPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.AddPartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.RemovePartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Update, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> UnknownUserMessages
        {
            get
            {
                var entityId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var tenantId = Guid.NewGuid();
                var aggregateId = 0;
                var entityType = (int)EntityType.User;
                var entityModelVersion = 1;
                var payload = GetUserPayload(entityModelVersion, aggregateId, entityId, tenantId);

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Unknown, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        private static ServiceBusReceivedMessage GetServiceBusMessage(string payload, int eventType, int aggregateId, Guid entityId, int entityType, int entityModelVersion, Guid companyId)
        {
            var payloadInBytes = new BinaryData(Encoding.UTF8.GetBytes(payload));
            var messageId = $"{entityId}_{aggregateId}";
            var contentType = "application/json";
            var partitionKey = $"{entityId}";

            var appProperties = new Dictionary<string, object>
            {
                { "eventType", eventType },
                { "aggregateId", aggregateId },
                { "entityId", entityId },
                { "entityType", entityType },
                { "entityModelVersion", entityModelVersion },
                { "companyId", companyId }
            };

            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(payloadInBytes, messageId, partitionKey, null, null, null, default, null, null, null, contentType, null, default, appProperties, default, 0, default, -1, null, 0, default);

            return message;
        }

        private static string GetCompanyPayload(int entityModelVersion, int aggregateId, Guid entityId, Guid tenantId)
        {
            return @"{
                        ""domain"": ""mavim.com"",
                        ""name"": ""Mavim"",
                        ""modelVersion"": " + entityModelVersion + @",
                        ""aggregateId"": " + aggregateId + @",
                        ""id"": """ + entityId + @""",
                        ""tenantId"": """ + tenantId + @"""
                    }";
        }

        private static string GetUserPayload(int entityModelVersion, int aggregateId, Guid entityId, Guid tenantId)
        {
            return @"{
                        ""email"": ""mavim.com"",
                        ""modelVersion"": " + entityModelVersion + @",
                        ""aggregateId"": " + aggregateId + @",
                        ""id"": """ + entityId + @""",
                        ""tenantId"": """ + tenantId + @"""
                    }";
        }

        private static string GetGroupPayload(int entityModelVersion, int aggregateId, Guid entityId, Guid tenantId)
        {
            return @"{
                        ""name"": ""mavim group"",
                        ""description"": ""group description"",
                        ""modelVersion"": " + entityModelVersion + @",
                        ""aggregateId"": " + aggregateId + @",
                        ""id"": """ + entityId + @""",
                        ""tenantId"": """ + tenantId + @"""
                    }";
        }
        #endregion
    }
}
