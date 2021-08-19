using Mavim.Manager.Connect.Write.Commands;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using Mavim.Manager.Connect.Write.ServiceBus.Interfaces;
using Microsoft.Azure.ServiceBus;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Test.Commands
{
    public class ResendEventSourcingTest
    {
        private static readonly List<EventSourcingModel> CompanyEvents =
            MockData.mockCompanyEvents.Where(c => c.EntityId == MockData.companyEntityId1).ToList();

        [Fact]
        [Trait("Category", "Connect AddCompanyCommand Write")]
        public async Task AddCompany_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var companyEvents = CompanyEvents;
            var eventSourcing = new Mock<ICommonEventSourcing>();
            eventSourcing.Setup(es => es.GetEvents(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => companyEvents);

            var serviceBus = new Mock<IBatchQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), MockData.companyId);

            var handler = new ResendEventSourcing.Handler(eventSourcing.Object, identity, serviceBus.Object);
            var command = new ResendEventSourcing.Command(0, MockData.companyEntityId1);
            var cancellationToken = new CancellationToken();

            // Act
            await handler.Handle(command, cancellationToken);

            // Assert
            eventSourcing.Verify(sb => sb.GetEvents(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            int count = 0;
            serviceBus.Verify(sb => sb.SendAsync(It.Is<Message>(e => Validate(e, ref count))), Times.Exactly(2));
        }

        private static bool Validate(Message message, ref int count)
        {
            var mAggregateId = int.Parse(message.UserProperties["aggregateId"].ToString());
            var mEventType = (EventType)int.Parse(message.UserProperties["eventType"].ToString());
            var mEntityId = Guid.Parse(message.UserProperties["entityId"].ToString());
            var mEntityType = (EntityType)int.Parse(message.UserProperties["entityType"].ToString());
            var mEntityModelVersion = int.Parse(message.UserProperties["entityModelVersion"].ToString());
            var mCompanyId = Guid.Parse(message.UserProperties["companyId"].ToString());

            (EventType eventType, int aggregateId, Guid entityId,
             EntityType entityType, int entityModelVersion, string payload,
             DateTime _, Guid companyId) = CompanyEvents[count];

            Assert.Equal(aggregateId, mAggregateId);
            Assert.Equal(eventType, mEventType);
            Assert.Equal(entityId, mEntityId);
            Assert.Equal(entityType, mEntityType);
            Assert.Equal(entityModelVersion, mEntityModelVersion);
            Assert.Equal(companyId, mCompanyId);
            Assert.Equal(aggregateId, mAggregateId);
            Assert.Equal(aggregateId, mAggregateId);
            Assert.Equal(Encoding.UTF8.GetBytes(payload), message.Body);
            Assert.Equal($"{entityId}_{aggregateId}", message.MessageId);
            Assert.Equal("application/json", message.ContentType);

            count++;
            return true;
        }

    }

}
