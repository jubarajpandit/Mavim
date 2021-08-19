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
    public class CompanyV1EventSourcingTest
    {
        private readonly JsonSerializerOptions JsonSerializerOptions = new() { IgnoreNullValues = true };

        #region Update
        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task UpdateEnity_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var entity1 = new CompanyV1(Guid.NewGuid(), "StartCompany", "StartDomain", Guid.NewGuid(), true);
            var entity2 = new CompanyV1() with
            {
                Name = "ChangedCompany",
                Domain = "ChangedDomain",
                TenantId = Guid.NewGuid(),
            };
            var expectedPayload = entity1 with
            {
                Name = entity2.Name,
                Domain = entity2.Domain,
                TenantId = entity2.TenantId,
            };

            // Act
            var payload = await eventsourcing.UpdateEnity(entity2, entity1);

            // Assert
            Assert.True(expectedPayload.ToString() == payload.ToString());

        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task UpdatePayload_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var entity1 = new CompanyV1(Guid.NewGuid(), "StartCompany", "StartDomain", Guid.NewGuid(), true);
            var entity2 = new CompanyV1() with
            {
                Name = "ChangedCompany",
                Domain = "ChangedDomain",
                TenantId = Guid.NewGuid(),
            };

            // Act
            var payload = await eventsourcing.UpdatePayload(entity1, entity2);

            // Assert
            Assert.True(entity2.ToString() == payload.ToString());

        }
        #endregion

        #region PlayEvents
        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task PlayEvents_CreateDeleteEvents_EntityDeletedIsFalse()
        {
            // arrange
            var entityId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Company, 0, string.Empty, DateTime.MinValue, entityId);
            var expectedCompanyV1 = new CompanyV1(entityId, "Mavim", "mavim.com", tenantId, false);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Name\":\"Mavim\",\"Domain\":\"mavim.com\",\"TenantId\":\"" + tenantId + "\",\"IsActive\":true}",
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
            Assert.Equal(expectedCompanyV1.ToString(), payload.ToString());
        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task PlayEvents_CreateUpdateEvents_EntityIsUpdated()
        {
            // arrange
            var entityId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Group, 0, string.Empty, DateTime.MinValue, entityId);
            var expectedCompanyV1 = new CompanyV1(entityId, "Mavim Updated", "mavim.com", tenantId, true);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Name\":\"Mavim\",\"Domain\":\"mavim.com\",\"TenantId\":\"" + tenantId + "\",\"IsActive\":true}",
                },
                defaultModel with
                {
                    EventType = EventType.Update,
                    AggregateId = 1,
                    Payload = "{\"Name\": \"Mavim Updated\"}",
                }
            };

            // Act
            var payload = await eventsourcing.PlayEvents(events);

            // Assert
            Assert.Equal(expectedCompanyV1.ToString(), payload.ToString());
        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task PlayEvents_CreateDeleteActivate_EntityIsActive()
        {
            // arrange
            var entityId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Company, 0, string.Empty, DateTime.MinValue, entityId);
            var expectedCompanyV1 = new CompanyV1(entityId, "Mavim", "mavim.com", tenantId, true);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Name\":\"Mavim\",\"Domain\":\"mavim.com\",\"TenantId\":\"" + tenantId + "\",\"IsActive\":true}",
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
            Assert.Equal(expectedCompanyV1.ToString(), payload.ToString());
        }
        #endregion

        #region CreateNewEvent
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public void CreateNewEvent_ValidArguments_ValidModelStateAsync(int aggregateId)
        {
            // You have two create payloads.
            // - one is from the start with aggregateId 0, this creates a object with all properties filled
            // - two is when the aggregateId is more then 0, this sets the activate property to true. (undo event from delete that set IsActive on false)

            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var entity1 = new CompanyV1(Guid.NewGuid(), "StartCompany", "StartDomain", Guid.NewGuid(), true);

            var @event = new EventSourcingModel(
                EventType.Create,
                aggregateId,
                Guid.NewGuid(),
                EntityType.Company,
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
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public void CreateDeleteEvent_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var @event = new EventSourcingModel(
                    EventType.Delete,
                    1,
                    Guid.NewGuid(),
                    EntityType.Company,
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

        #region CreateUpdateEvent
        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public void CreateUpdateEvent_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var entity1 = new CompanyV1(null, "UpdateCompany", "UpdateDomain", null, null);
            var expectedEvent = new EventSourcingModel(
                    EventType.Update,
                    1,
                    Guid.NewGuid(),
                    EntityType.Company,
                    1,
                    JsonSerializer.Serialize(entity1, JsonSerializerOptions),
                    DateTime.Now,
                    Guid.NewGuid()
                );

            // Act
            var @event = eventsourcing.CreateUpdateEvent(expectedEvent.AggregateId, expectedEvent.EntityId, expectedEvent.CompanyId, entity1);

            // Assert
            expectedEvent = expectedEvent with
            {
                AggregateId = 2,
                TimeStamp = @event.TimeStamp
            };
            Assert.True(expectedEvent.ToString() == @event.ToString());

        }
        #endregion

        #region DoesEntitiesExists
        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task DoesEntitiesExists_ValidArguments_true()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));
            var entityIds = new List<Guid>() { MockData.companyEntityId2, MockData.companyEntityId3 };

            // Act
            var result = await eventsourcing.DoesEntitiesExists(entityIds, MockData.companyId, new CancellationToken());

            // Assert

            Assert.True(result);

        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task DoesEntitiesExists_InValidArguments_false()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext());
            var entityIds = new List<Guid>() { MockData.companyEntityId2, MockData.companyEntityId3 };

            // Act
            var result = await eventsourcing.DoesEntitiesExists(entityIds, MockData.companyId, new CancellationToken());

            // Assert
            Assert.False(result);

        }
        #endregion

        #region GetListOfEntityEvents
        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetListOfEntityEvents_validArguments_AllEventsFromEntity()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));
            var entityIds = new List<Guid>() { MockData.companyEntityId1, MockData.companyEntityId2, MockData.companyEntityId3 };

            // Act
            var result = await eventsourcing.GetListOfEntityEvents(entityIds, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.Count == 4);

        }
        #endregion

        #region GetLatestEvent
        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetLatestEvent_validArguments_LatestEvent()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetLatestEvent(MockData.companyEntityId1, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.AggregateId == 1);

        }
        #endregion

        #region GetEvents

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntity()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetEvents(MockData.companyEntityId1, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.Count == 2);

        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntityWithAggrigrateId()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetEvents(MockData.companyEntityId1, MockData.companyId, 0, new CancellationToken());

            // Assert
            Assert.True(result.Count == 1);

        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntityFromCreateDate()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));
            var startDate = new DateTime(2021, 02, 11, 14, 10, 50);
            var endDate = startDate.AddSeconds(2);

            // Act
            var result = await eventsourcing.GetEvents(startDate, MockData.companyId, new CancellationToken(), endDate);

            // Assert
            Assert.Equal(2, result.Count);

        }
        #endregion

        #region PreventGettingOtherEntities
        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetEvents_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockUserEvents.First();

            // Act
            var result = await eventsourcing.GetEvents(@event.EntityId, MockData.companyId, new CancellationToken());

            // Assert
            Assert.NotEqual(MockData.mockUserEvents.Where(e => e.EntityId == @event.EntityId).Count(), result.Count);
            Assert.Equal(0, result.Count);

        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetEventsBasedOnAggregateId_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockUserEvents.First();
            var events = MockData.mockUserEvents.Where(e => e.EntityId == @event.EntityId);
            var lastevent = events.OrderByDescending(e => e.AggregateId).First();

            // Act
            var result = await eventsourcing.GetEvents(@event.EntityId, MockData.companyId, lastevent.AggregateId, new CancellationToken());

            // Assert
            Assert.NotEqual(MockData.mockUserEvents.Where(e => e.EntityId == @event.EntityId).Count(), result.Count);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetLatestEvent_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockUserEvents.First();
            var events = MockData.mockUserEvents.Where(e => e.EntityId == @event.EntityId);
            var lastevent = events.OrderByDescending(e => e.AggregateId).First();

            // Act
            var result = await eventsourcing.GetLatestEvent(@event.EntityId, MockData.companyId, new CancellationToken());

            // Assert
            Assert.Null(result);
            Assert.NotEqual(lastevent, result);

        }

        [Fact]
        [Trait("Category", "Connect CompanyV1EventSourcing Write")]
        public async Task GetListOfEntityEvents_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new CompanyV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockUserEvents.Select(e => e.EntityId).Distinct().ToList();

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
                context.AddRange(MockData.mockUserEvents); // addUsers
                context.AddRange(MockData.mockCompanyEvents); //addCompany
                context.AddRange(MockData.mockGroupEvents); // addGroup
                context.SaveChanges();
            }

            return context;
        }
    }
}
