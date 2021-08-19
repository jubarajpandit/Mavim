using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Test.EventSourcing
{
    public class UserV1EventSourcingTest
    {
        private readonly JsonSerializerOptions JsonSerializerOptions = new() { IgnoreNullValues = true };


        #region CreateNewEvent
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public void CreateNewEvent_ValidArguments_ValidModelStateAsync(int aggregateId)
        {
            // You have two create payloads.
            // - one is from the start with aggregateId 0, this creates a object with all properties filled
            // - two is when the aggregateId is more then 0, this sets the activate property to true. (undo event from delete that set IsActive on false)

            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext());
            var entity1 = new UserV1(Guid.NewGuid(), "test@mavim.com", MockData.companyId, true);

            var @event = new EventSourcingModel(
                EventType.Create,
                aggregateId,
                Guid.NewGuid(),
                EntityType.User,
                1,
                aggregateId == 0
                    ? JsonSerializer.Serialize(entity1, JsonSerializerOptions)
                    : JsonSerializer.Serialize(new { IsActive = true }, JsonSerializerOptions),
                DateTime.Now,
                Guid.NewGuid()
            );

            // Act
            var payload = eventsourcing.CreateNewEvent(@event.EntityId, @event.CompanyId, entity1, aggregateId) with
            {
                TimeStamp = @event.TimeStamp
            };

            // Assert
            Assert.True(@event.ToString() == payload.ToString());

        }
        #endregion

        #region CreateDeleteEvent
        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public void CreateDeleteEvent_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext());
            var @event = new EventSourcingModel(
                    EventType.Delete,
                    1,
                    Guid.NewGuid(),
                    EntityType.User,
                    1,
                    JsonSerializer.Serialize(new { IsActive = false }, JsonSerializerOptions),
                    DateTime.Now,
                    Guid.NewGuid()
                );

            // Act
            var payload = eventsourcing.CreateDeleteEvent(@event.AggregateId, @event.EntityId, @event.CompanyId);

            // Assert
            var expectPayload = @event with
            {
                AggregateId = 2,
                TimeStamp = payload.TimeStamp
            };
            Assert.True(expectPayload.ToString() == payload.ToString());

        }
        #endregion

        #region DoesEntitiesExists
        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task DoesEntitiesExists_ValidArguments_true()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));
            var entityIds = new List<Guid>() { MockData.userEntityId2, MockData.userEntityId3 };

            // Act
            var result = await eventsourcing.DoesEntitiesExists(entityIds, MockData.companyId, new CancellationToken());

            // Assert

            Assert.True(result);

        }

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task DoesEntitiesExists_InValidArguments_false()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext());
            var entityIds = new List<Guid>() { MockData.userEntityId2, MockData.userEntityId3 };

            // Act
            var result = await eventsourcing.DoesEntitiesExists(entityIds, MockData.companyId, new CancellationToken());

            // Assert
            Assert.False(result);

        }
        #endregion

        #region GetListOfEntityEvents
        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetListOfEntityEvents_validArguments_AllEventsFromEntity()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));
            var entityIds = new List<Guid>() { MockData.userEntityId1, MockData.userEntityId2, MockData.userEntityId3 };

            // Act
            var result = await eventsourcing.GetListOfEntityEvents(entityIds, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.Count == 4);

        }
        #endregion

        #region GetLatestEvent
        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetLatestEvent_validArguments_LatestEvent()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetLatestEvent(MockData.userEntityId1, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.AggregateId == 1);

        }
        #endregion

        #region GetEvents

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntity()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetEvents(MockData.userEntityId1, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.Count == 2);

        }

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntityWithAggrigrateId()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetEvents(MockData.userEntityId1, MockData.companyId, 0, new CancellationToken());

            // Assert
            Assert.True(result.Count == 1);

        }

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntityFromCreateDate()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));
            var startDate = new DateTime(2021, 02, 11, 14, 10, 50);
            var endDate = startDate.AddSeconds(2);

            // Act
            var result = await eventsourcing.GetEvents(startDate, MockData.companyId, new CancellationToken(), endDate);

            // Assert
            Assert.True(result.Count == 2);

        }
        #endregion

        #region PlayEvents
        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task PlayEvents_CreateDeleteEvents_EntityDeletedIsFalse()
        {
            // arrange
            var entityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var eventsourcing = new UserV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Company, 0, string.Empty, DateTime.MinValue, entityId);
            var expectedUserV1 = new UserV1(entityId, "test@mavim.com", companyId, false);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Email\":\"test@mavim.com\",\"CompanyId\":\"" + companyId + "\",\"IsActive\":true}",
                },
                defaultModel with
                {
                    EventType = EventType.Delete,
                    AggregateId = 1,
                    Payload = "{\"IsActive\": false}",
                }
            };
            // Act
            var payload = await eventsourcing.PlayEvents(events);

            // Assert
            Assert.Equal(expectedUserV1.ToString(), payload.ToString());
        }

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task PlayEvents_CreateUpdateEvents_EntityIsUpdated()
        {
            // arrange
            var entityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var eventsourcing = new UserV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Group, 0, string.Empty, DateTime.MinValue, entityId);
            var expectedUserV1 = new UserV1(entityId, "testupdate@mavim.com", companyId, true);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Email\":\"test@mavim.com\",\"CompanyId\":\"" + companyId + "\",\"IsActive\":true}",
                },
                defaultModel with
                {
                    EventType = EventType.Update,
                    AggregateId = 1,
                    Payload = "{\"Email\": \"testupdate@mavim.com\"}",
                }
            };

            // Act
            var payload = await eventsourcing.PlayEvents(events);

            // Assert
            Assert.Equal(expectedUserV1.ToString(), payload.ToString());
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task PlayEvents_CreateDeleteActivate_EntityIsActive()
        {
            // arrange
            var entityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var eventsourcing = new UserV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Company, 0, string.Empty, DateTime.MinValue, entityId);
            var expectedUserV1 = new UserV1(entityId, "test@mavim.com", companyId, true);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Email\":\"test@mavim.com\",\"CompanyId\":\"" + companyId + "\",\"IsActive\":true}",
                },
                defaultModel with
                {
                    EventType = EventType.Delete,
                    AggregateId = 1,
                    Payload = "{\"IsActive\": false}",
                },
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 2,
                    Payload = "{\"IsActive\": true}",
                }
            };
            // Act
            var payload = await eventsourcing.PlayEvents(events);

            // Assert
            Assert.Equal(expectedUserV1.ToString(), payload.ToString());
        }
        #endregion

        #region PreventGettingOtherEntities
        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetEvents_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockGroupEvents.First();

            // Act
            var result = await eventsourcing.GetEvents(@event.EntityId, MockData.companyId, new CancellationToken());

            // Assert
            Assert.NotEqual(MockData.mockGroupEvents.Where(e => e.EntityId == @event.EntityId).Count(), result.Count);
            Assert.Equal(0, result.Count);

        }

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetEventsBasedOnAggregateId_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockGroupEvents.First();
            var events = MockData.mockGroupEvents.Where(e => e.EntityId == @event.EntityId);
            var lastevent = events.OrderByDescending(e => e.AggregateId).First();

            // Act
            var result = await eventsourcing.GetEvents(@event.EntityId, MockData.companyId, lastevent.AggregateId, new CancellationToken());

            // Assert
            Assert.NotEqual(MockData.mockGroupEvents.Where(e => e.EntityId == @event.EntityId).Count(), result.Count);
            Assert.Equal(0, result.Count);

        }

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetLatestEvent_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockGroupEvents.First();
            var events = MockData.mockGroupEvents.Where(e => e.EntityId == @event.EntityId);
            var lastevent = events.OrderByDescending(e => e.AggregateId).First();

            // Act
            var result = await eventsourcing.GetLatestEvent(@event.EntityId, MockData.companyId, new CancellationToken());

            // Assert
            Assert.Null(result);
            Assert.NotEqual(lastevent, result);

        }

        [Fact]
        [Trait("Category", "Connect UserV1EventSourcing Write")]
        public async Task GetListOfEntityEvents_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new UserV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockGroupEvents.Select(e => e.EntityId).Distinct().ToList();

            // Act
            var result = await eventsourcing.GetListOfEntityEvents(@event, MockData.companyId, new CancellationToken());

            // Assert
            Assert.Equal(0, result.Count);

        }
        #endregion

        private static ConnectDbContext GetMockContext(bool withData = false)
        {
            var options = new DbContextOptionsBuilder<ConnectDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new ConnectDbContext(options);

            if (withData)
            {
                context.AddRange(MockData.mockGroupEvents);
                context.AddRange(MockData.mockUserEvents);
                context.AddRange(MockData.mockCompanyEvents);
                context.SaveChanges();
            }

            return context;
        }
    }
}
