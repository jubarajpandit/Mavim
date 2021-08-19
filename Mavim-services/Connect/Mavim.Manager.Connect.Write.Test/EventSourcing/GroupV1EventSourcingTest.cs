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
    public class GroupV1EventSourcingTest
    {
        private readonly JsonSerializerOptions JsonSerializerOptions = new() { IgnoreNullValues = true };

        #region Update
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task UpdateEntity_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var entity1 = new GroupV1(Guid.NewGuid(), "StartGroup", "StartDescription", Guid.NewGuid(), new List<Guid> { Guid.NewGuid() }, true);
            var payload = new GroupV1() with
            {
                Id = Guid.Empty,
                Name = "ChangedGroup",
                Description = "ChangedDescription",
                CompanyId = Guid.Empty,
                UserIds = new List<Guid> { },
                IsActive = true
            };
            var expectedPayload = entity1 with
            {
                Name = payload.Name,
                Description = payload.Description
            };

            // Act
            var returnPayload = await eventsourcing.UpdateEnity(payload, entity1);

            // Assert
            Assert.True(expectedPayload.ToString() == returnPayload.ToString(), "Compaire primary types properties");
            Assert.True(Enumerable.SequenceEqual(expectedPayload.UserIds.OrderBy(t => t), returnPayload.UserIds.OrderBy(t => t)), "Compare List of UserIds");
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task UpdatePayload_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var entity1 = new GroupV1(Guid.NewGuid(), "StartGroup", "StartDescription", Guid.NewGuid(), new List<Guid> { Guid.NewGuid() }, true);
            var entity2 = new GroupV1() with
            {
                Name = "ChangedGroup",
                Description = "ChangedDescription",
            };

            // Act
            var payload = await eventsourcing.UpdatePayload(entity1, entity2);

            // Assert
            Assert.True(entity2 == payload);
        }
        #endregion

        #region PlayEvents
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task PlayEvents_CreateDeleteEvents_EntityDeletedIsFalse()
        {
            // arrange
            var companyId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Group, 0, string.Empty, DateTime.MinValue, companyId);
            var expectedGroupV1 = new GroupV1(entityId, "Mavim Group", "Mavim Group Description", companyId, new List<Guid> { }, false);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Name\":\"Mavim Group\",\"Description\":\"Mavim Group Description\",\"CompanyId\":\"" + defaultModel.CompanyId + "\",\"UserIds\": [],\"IsActive\":true}",
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
            Assert.Equal(expectedGroupV1.ToString(), payload.ToString());
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task PlayEvents_CreateUpdateEvents_EntityIsUpdated()
        {
            // arrange
            var companyId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Group, 0, string.Empty, DateTime.MinValue, companyId);
            var expectedGroupV1 = new GroupV1(entityId, "Mavim Group Updated", "Mavim Group Description Updated", companyId, new List<Guid> { }, true);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Name\":\"Mavim Group\",\"Description\":\"Mavim Group Description\",\"CompanyId\":\"" + defaultModel.CompanyId + "\",\"UserIds\": [],\"IsActive\":true}",
                },
                defaultModel with
                {
                    EventType = EventType.Update,
                    AggregateId = 1,
                    Payload = "{\"Name\": \"Mavim Group Updated\", \"Description\": \"Mavim Group Description Updated\"}",
                }
            };

            // Act
            var payload = await eventsourcing.PlayEvents(events);

            // Assert
            Assert.Equal(expectedGroupV1.ToString(), payload.ToString());
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task PlayEvents_CreatePartialUpdateEvents_EntityUserListIsUpdated()
        {
            // arrange
            var companyId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var user1 = Guid.NewGuid();
            var user2 = Guid.NewGuid();
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Group, 0, string.Empty, DateTime.MinValue, companyId);
            var expectedGroupV1 = new GroupV1(entityId, "Mavim Group", "Mavim Group Description", companyId, new List<Guid> { user1 }, true);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Name\":\"Mavim Group\",\"Description\":\"Mavim Group Description\",\"CompanyId\":\"" + defaultModel.CompanyId + "\",\"UserIds\": [],\"IsActive\":true}",
                },
                defaultModel with
                {
                    EventType = EventType.AddPartial,
                    AggregateId = 1,
                    Payload = "{\"UserIds\": [\"" + user1 +"\", \"" + user2 + "\"]}",
                },
                defaultModel with
                {
                    EventType = EventType.RemovePartial,
                    AggregateId = 2,
                    Payload = "{\"UserIds\": [\"" + user2 + "\"]}",
                }
            };

            // Act
            var payload = await eventsourcing.PlayEvents(events);

            // Assert
            Assert.True(expectedGroupV1.ToString() == payload.ToString());
            Assert.True(Enumerable.SequenceEqual(expectedGroupV1.UserIds.OrderBy(t => t), payload.UserIds.OrderBy(t => t)));
        }


        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task PlayEvents_CreateDeleteActivate_EntityIsActive()
        {
            // arrange
            var companyId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var defaultModel = new EventSourcingModel(EventType.Create, 0, entityId, EntityType.Group, 0, string.Empty, DateTime.MinValue, companyId);
            var expectedGroupV1 = new GroupV1(entityId, "Mavim Group", "Mavim Group Description", companyId, new List<Guid> { }, true);
            var events = new List<EventSourcingModel> {
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 0,
                    Payload = "{\"Id\":\"" + defaultModel.EntityId + "\",\"Name\":\"Mavim Group\",\"Description\":\"Mavim Group Description\",\"CompanyId\":\"" + defaultModel.CompanyId + "\",\"UserIds\": [],\"IsActive\":true}"
                },
                defaultModel with
                {
                    EventType = EventType.Delete,
                    AggregateId = 1,
                    Payload = "{\"IsActive\": false}"
                },
                defaultModel with
                {
                    EventType = EventType.Create,
                    AggregateId = 2,
                    Payload = "{\"IsActive\": true}"
                }
            };

            // Act
            var payload = await eventsourcing.PlayEvents(events);

            // Assert
            Assert.Equal(expectedGroupV1.ToString(), payload.ToString());
        }
        #endregion

        #region CreateAddPartialEvent
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public void CreateAddPartialEvent_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var userId = Guid.NewGuid();
            var @event = new EventSourcingModel(
                    EventType.AddPartial,
                    1,
                    Guid.NewGuid(),
                    EntityType.Group,
                    1,
                    JsonSerializer.Serialize(new { UserIds = new List<Guid> { userId } }, JsonSerializerOptions),
                    DateTime.Now,
                    Guid.NewGuid()
                );
            var group = new GroupV1();
            var changedGroup = group with
            {
                UserIds = new List<Guid> { userId }
            };

            // Act
            var payload = eventsourcing.CreateAddPartialEvent(@event.AggregateId, @event.EntityId, @event.CompanyId, changedGroup);

            // Assert
            var expectPayload = @event with
            {
                AggregateId = 2,
                TimeStamp = payload.TimeStamp
            };
            Assert.True(expectPayload == payload);
        }
        #endregion

        #region CreateRemovePartialEvent
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public void CreateRemovePartialEvent_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var userId = Guid.NewGuid();
            var @event = new EventSourcingModel(
                    EventType.RemovePartial,
                    1,
                    Guid.NewGuid(),
                    EntityType.Group,
                    1,
                    JsonSerializer.Serialize(new { UserIds = new List<Guid> { userId } }, JsonSerializerOptions),
                    DateTime.Now,
                    Guid.NewGuid()
                );
            var group = new GroupV1();
            var changedGroup = group with
            {
                UserIds = new List<Guid> { userId }
            };

            // Act
            var payload = eventsourcing.CreateRemovePartialEvent(@event.AggregateId, @event.EntityId, @event.CompanyId, changedGroup);

            // Assert
            var expectPayload = @event with
            {
                AggregateId = 2,
                TimeStamp = payload.TimeStamp
            };
            Assert.Equal(expectPayload, payload);
        }
        #endregion

        #region AddPartialPayload
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task AddPartialPayload_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var existingEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1 }
            };

            var addUserEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1, userId2 }
            };

            var expectedPayload = new GroupV1() with
            {
                UserIds = new List<Guid> { userId2 }
            };

            // Act
            var payload = await eventsourcing.AddPartialPayload(existingEntity, addUserEntity);

            // Assert
            Assert.True(Enumerable.SequenceEqual(expectedPayload.UserIds.OrderBy(t => t), payload.UserIds.OrderBy(t => t)));
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task AddPartialPayload_DuplicateUser_EmptyPayload()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var userId1 = Guid.NewGuid();
            var existingEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1 }
            };

            var addUserEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1 }
            };

            // Act
            var payload = await eventsourcing.AddPartialPayload(existingEntity, addUserEntity);

            // Assert
            Assert.False(payload.UserIds.Any(), "Payload should not contains items because it adds a duplicate userid");
        }
        #endregion

        #region RemovePartialPayload
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task RemovePartialPayload_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var existingEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1, userId2 }
            };

            var removeUserEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1 }
            };

            var expectedPayload = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1 }
            };

            // Act
            var payload = await eventsourcing.RemovePartialPayload(existingEntity, removeUserEntity);

            // Assert
            Assert.True(Enumerable.SequenceEqual(expectedPayload.UserIds.OrderBy(t => t), payload.UserIds.OrderBy(t => t)));
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task RemovePartialPayload_NotExistingUser_EmptyPayload()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var existingEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId2 }
            };

            var removeUserEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1 }
            };

            var expectedPayload = new GroupV1() with
            {
                UserIds = new List<Guid> { }
            };

            // Act
            var payload = await eventsourcing.RemovePartialPayload(existingEntity, removeUserEntity);

            // Assert
            Assert.True(Enumerable.SequenceEqual(expectedPayload.UserIds.OrderBy(t => t), payload.UserIds.OrderBy(t => t)));
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task RemovePartialPayload_NonUsersExistsToDeleted_EmptyPayload()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var existingEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { }
            };

            var removeUserEntity = new GroupV1() with
            {
                UserIds = new List<Guid> { userId1 }
            };

            var expectedPayload = new GroupV1() with
            {
                UserIds = new List<Guid> { }
            };

            // Act
            var payload = await eventsourcing.RemovePartialPayload(existingEntity, removeUserEntity);

            // Assert
            Assert.True(Enumerable.SequenceEqual(expectedPayload.UserIds.OrderBy(t => t), payload.UserIds.OrderBy(t => t)));
        }
        #endregion

        #region CreateNewEvent
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public void CreateNewEvent_ValidArguments_ValidModelStateAsync(int aggregateId)
        {
            // You have two create payloads.
            // - one is from the start with aggregateId 0, this creates a object with all properties filled
            // - two is when the aggregateId is more then 0, this sets the activate property to true. (undo event from delete that set IsActive on false)

            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var entity1 = new GroupV1(Guid.NewGuid(), "StartGroup", "StartDescription", Guid.NewGuid(), new List<Guid> { Guid.NewGuid() }, true);

            var @event = new EventSourcingModel(
                EventType.Create,
                aggregateId,
                Guid.NewGuid(),
                EntityType.Group,
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
            Assert.True(@event == payload);
        }
        #endregion

        #region CreateDeleteEvent
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public void CreateDeleteEvent_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var @event = new EventSourcingModel(
                    EventType.Delete,
                    1,
                    Guid.NewGuid(),
                    EntityType.Group,
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
            Assert.True(expectPayload == payload);
        }
        #endregion

        #region CreateUpdateEvent
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public void CreateUpdateEvent_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var entity1 = new GroupV1(null, "UpdateCompany", "UpdateDomain", null, null, null);
            var expectedEvent = new EventSourcingModel(
                    EventType.Update,
                    1,
                    Guid.NewGuid(),
                    EntityType.Group,
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
            Assert.True(expectedEvent == @event);
        }
        #endregion

        #region DoesEntitiesExists
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task DoesEntitiesExists_ValidArguments_true()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));
            var entityIds = new List<Guid>() { MockData.groupEntityId2, MockData.groupEntityId3 };

            // Act
            var result = await eventsourcing.DoesEntitiesExists(entityIds, MockData.companyId, new CancellationToken());

            // Assert

            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task DoesEntitiesExists_InValidArguments_false()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var entityIds = new List<Guid>() { MockData.groupEntityId2, MockData.groupEntityId3 };

            // Act
            var result = await eventsourcing.DoesEntitiesExists(entityIds, MockData.companyId, new CancellationToken());

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task DoesEntitiesExists_InValidEntityType_false()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext());
            var entityIds = new List<Guid>() { MockData.userEntityId1, MockData.userEntityId2 };

            // Act
            var result = await eventsourcing.DoesEntitiesExists(entityIds, MockData.companyId, new CancellationToken());

            // Assert
            Assert.False(result);
        }
        #endregion

        #region GetListOfEntityEvents
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task GetListOfEntityEvents_validArguments_AllEventsFromEntity()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));
            var entityIds = new List<Guid>() { MockData.groupEntityId1, MockData.groupEntityId2, MockData.groupEntityId3 };

            // Act
            var result = await eventsourcing.GetListOfEntityEvents(entityIds, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.Count == 4);
        }
        #endregion

        #region GetLatestEvent
        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task GetLatestEvent_validArguments_LatestEvent()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetLatestEvent(MockData.groupEntityId1, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.AggregateId == 1);
        }
        #endregion

        #region GetEvents

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntity()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetEvents(MockData.groupEntityId1, MockData.companyId, new CancellationToken());

            // Assert
            Assert.True(result.Count == 2);
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntityWithAggrigrateId()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));

            // Act
            var result = await eventsourcing.GetEvents(MockData.groupEntityId1, MockData.companyId, 0, new CancellationToken());

            // Assert
            Assert.True(result.Count == 1);
        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task GetEvents_validArguments_AllEventFromEntityFromCreateDate()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));
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
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task GetEvents_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));
            var @event = MockData.mockUserEvents.First();

            // Act
            var result = await eventsourcing.GetEvents(@event.EntityId, MockData.companyId, new CancellationToken());

            // Assert
            Assert.NotEqual(MockData.mockUserEvents.Where(e => e.EntityId == @event.EntityId).Count(), result.Count);
            Assert.Equal(0, result.Count);

        }

        [Fact]
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
        public async Task GetEventsBasedOnAggregateId_DiffrentEntityId_NonEventsFound()
        {
            // Arrange
            var eventsourcing = new GroupV1EventSourcing(GetMockContext(withData: true));
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
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
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
        [Trait("Category", "Connect GroupV1EventSourcing Write")]
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
                context.AddRange(MockData.mockGroupEvents);
                context.AddRange(MockData.mockUserEvents);
                context.AddRange(MockData.mockCompanyEvents);
                context.SaveChanges();
            }

            return context;
        }
    }
}
