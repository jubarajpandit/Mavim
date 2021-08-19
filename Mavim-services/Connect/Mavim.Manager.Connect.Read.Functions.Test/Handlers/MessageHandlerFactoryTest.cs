using Azure.Messaging.ServiceBus;
using Mavim.Manager.Connect.Read.Functions.Clients;
using Mavim.Manager.Connect.Read.Functions.Constants;
using Mavim.Manager.Connect.Read.Functions.Constants.Enums;
using Mavim.Manager.Connect.Read.Functions.Handlers;
using Mavim.Manager.Connect.Read.Functions.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Mavim.Manager.Connect.Read.Functions.Test.Handlers
{
    public class MessageHandlerFactoryTest
    {
        #region CompanyEvents
        [Theory, MemberData(nameof(ValidCompanyEvents))]
        public async void ExecuteAsync_ValidCompanyMessage_CompletedMessage(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var result = await messageHandler.ExecuteAsync();

            // Assert
            httpClient.Verify(x => x.SendRequestAsync(Endpoints.AddCompany, HttpMethod.Post, It.IsAny<Company>(), It.IsAny<ILogger>()), Times.Once);
            Assert.IsType<CompanyMessageHandler>(messageHandler);
            Assert.True(result.IsSuccessStatusCode);
        }

        [Theory, MemberData(nameof(InvalidCompanyEvents))]
        public async void ExecuteAsync_InvalidCompanyMessage_NotImplementedException(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var response = await Record.ExceptionAsync(async () => await messageHandler.ExecuteAsync());

            // Assert
            Assert.NotNull(response);
            Assert.IsType<NotImplementedException>(response);
        }

        [Theory, MemberData(nameof(UnknownCompanyEvent))]
        public async void ExecuteAsync_UnknownCompanyEvent_Null(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var response = await messageHandler.ExecuteAsync();

            // Assert
            Assert.Null(response);
        }
        #endregion

        #region UserEvents
        [Theory, MemberData(nameof(ValidUserEvents))]
        public async void ExecuteAsync_ValidUserMessage_CompletedMessage(ServiceBusReceivedMessage serviceBusReceiveMessage, string endpoint, HttpMethod httpMethod)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var result = await messageHandler.ExecuteAsync();

            // Assert
            httpClient.Verify(x => x.SendRequestAsync(endpoint, httpMethod, It.IsAny<User>(), It.IsAny<ILogger>()), Times.Once);
            Assert.IsType<UserMessageHandler>(messageHandler);
            Assert.True(result.IsSuccessStatusCode);
        }

        [Theory, MemberData(nameof(InvalidUserEvents))]
        public async void ExecuteAsync_InvalidUserMessage_NotImplementedException(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var response = await Record.ExceptionAsync(async () => await messageHandler.ExecuteAsync());

            // Assert
            Assert.NotNull(response);
            Assert.IsType<NotImplementedException>(response);
        }

        [Theory, MemberData(nameof(UnknownUserEvent))]
        public async void ExecuteAsync_UnknownUserEvent_Null(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var response = await messageHandler.ExecuteAsync();

            // Assert
            Assert.Null(response);
        }
        #endregion

        #region GroupEvents
        [Theory, MemberData(nameof(ValidGroupEvents))]
        public async void ExecuteAsync_ValidGroupMessage_CompletedMessage(ServiceBusReceivedMessage serviceBusReceiveMessage, string endpoint, HttpMethod httpMethod)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var result = await messageHandler.ExecuteAsync();

            // Assert
            httpClient.Verify(x => x.SendRequestAsync(endpoint, httpMethod, It.IsAny<Group>(), It.IsAny<ILogger>()), Times.Once);
            Assert.IsType<GroupMessageHandler>(messageHandler);
            Assert.True(result.IsSuccessStatusCode);
        }

        [Theory, MemberData(nameof(UnknownGroupEvent))]
        public async void ExecuteAsync_UnknownGroupEvent_Null(ServiceBusReceivedMessage serviceBusReceiveMessage)
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var httpClient = new Mock<IConnectHttpClient>();
            var messageHandler = MessageHandlerFactory.GetMessageHandler(serviceBusReceiveMessage, logger.Object, httpClient.Object);
            httpClient.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<ILogger>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

            // Act
            var response = await messageHandler.ExecuteAsync();

            // Assert
            Assert.Null(response);
        }
        #endregion

        #region TestData
        public static IEnumerable<object[]> ValidCompanyEvents
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

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Create, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> InvalidCompanyEvents
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

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Update, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Delete, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.AddPartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.RemovePartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> UnknownCompanyEvent
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

        public static IEnumerable<object[]> ValidUserEvents
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

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Create, aggregateId, entityId, entityType, entityModelVersion, companyId), Endpoints.AddUser, HttpMethod.Post };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Delete, aggregateId, entityId, entityType, entityModelVersion, companyId), string.Format(Endpoints.DisableUser, entityId), HttpMethod.Delete };
            }
        }

        public static IEnumerable<object[]> InvalidUserEvents
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

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Update, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.AddPartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.RemovePartial, aggregateId, entityId, entityType, entityModelVersion, companyId) };
            }
        }

        public static IEnumerable<object[]> UnknownUserEvent
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

        public static IEnumerable<object[]> ValidGroupEvents
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

                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Create, aggregateId, entityId, entityType, entityModelVersion, companyId), Endpoints.AddGroup, HttpMethod.Post };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Delete, aggregateId, entityId, entityType, entityModelVersion, companyId), string.Format(Endpoints.DisableGroup, entityId), HttpMethod.Delete };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.Update, aggregateId, entityId, entityType, entityModelVersion, companyId), string.Format(Endpoints.UpdateGroup, entityId), HttpMethod.Patch };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.AddPartial, aggregateId, entityId, entityType, entityModelVersion, companyId), string.Format(Endpoints.AddOrDeleteUsersToGroup, entityId), HttpMethod.Patch };
                yield return new object[] { GetServiceBusMessage(payload, (int)EventType.RemovePartial, aggregateId, entityId, entityType, entityModelVersion, companyId), string.Format(Endpoints.AddOrDeleteUsersToGroup, entityId), HttpMethod.Delete };
            }
        }

        public static IEnumerable<object[]> UnknownGroupEvent
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
