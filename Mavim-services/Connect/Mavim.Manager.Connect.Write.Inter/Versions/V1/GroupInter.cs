using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Commands;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.EventSourcing;
using Mavim.Manager.Connect.Write.Identity;
using Mavim.Manager.Connect.Write.Inter.Mock;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Inter.Versions.V1
{
    public class GroupInter
    {
        [Fact]
        [Trait("Category", "Connect AddGroupCommand Write")]
        public async Task AddGroupCommand_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == groupId);
            string payload = "{\"Name\":\"Test\",\"Description\":\"Description\",\"CompanyId\":\"" + identity.CompanyId + "\",\"UserIds\":[],\"Id\":\"" + groupId + "\",\"IsActive\":true}";
            var expectRecord = new EventSourcingModel(EventType.Create, 0, record.EntityId, EntityType.Group, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_ValidTitleAndDescriptionArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var eventSourcing = new GroupV1EventSourcing(context);
            var updateHandler = new UpdateGroup.Handler(eventSourcing, identity, new QueueClientMock());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            await HelperEventSourcing.UpdateGroup(context, identity, new UpdateGroup.Command(groupId, "TestUpdate", "DescriptionUpdate"));

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == groupId);
            string payload = "{\"Name\":\"TestUpdate\",\"Description\":\"DescriptionUpdate\"}";
            var expectRecord = new EventSourcingModel(EventType.Update, 1, record.EntityId, EntityType.Group, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_ValidTitleArguments_ValidModelStateAsync()
        {
            // Arrange
            string title = "TestUpdate2";
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var eventSourcing = new GroupV1EventSourcing(context);
            var updateHandler = new UpdateGroup.Handler(eventSourcing, identity, new QueueClientMock());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            await HelperEventSourcing.UpdateGroup(context, identity, new UpdateGroup.Command(groupId, title, null));

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == groupId);

            string payload = "{\"Name\":\"" + title + "\"}";

            var expectRecord = new EventSourcingModel(EventType.Update, 1, record.EntityId, EntityType.Group, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_ValidDescriptionArguments_ValidModelStateAsync()
        {
            // Arrange
            string description = "DescriptionUpdate2";
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var eventSourcing = new GroupV1EventSourcing(context);
            var updateHandler = new UpdateGroup.Handler(eventSourcing, identity, new QueueClientMock());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            await HelperEventSourcing.UpdateGroup(context, identity, new UpdateGroup.Command(groupId, null, description));

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == groupId);

            string payload = "{\"Description\":\"" + description + "\"}";

            var expectRecord = new EventSourcingModel(EventType.Update, 1, record.EntityId, EntityType.Group, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }


        #region Delete Group
        [Fact]
        [Trait("Category", "Connect AddGroupCommand Write")]
        public async Task DeleteGroupCommand_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            await HelperEventSourcing.DeleteGroup(context, identity, groupId);

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == groupId);
            string payload = "{\"IsActive\":false}";
            var expectRecord = new EventSourcingModel(EventType.Delete, 1, record.EntityId, EntityType.Group, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }


        [Fact]
        [Trait("Category", "Connect AddGroupCommand Write")]
        public async Task DeleteGroupCommand_ValidUserID_RequestNotFoundException()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var userId = await HelperEventSourcing.CreateUser(context, identity, "test@mavim.com");
            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.DeleteGroup(context, identity, userId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"Group {userId} not found", exception.Message);
        }
        #endregion

        #region RemovePartial
        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task RemoveUserGroup_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var user1 = await HelperEventSourcing.CreateUser(context, identity, "user1@mavim.com");
            var user2 = await HelperEventSourcing.CreateUser(context, identity, "user2@mavim.com");
            var user3 = await HelperEventSourcing.CreateUser(context, identity, "user3@mavim.com");

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1, user2, user3 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1, user2, user3 });

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == groupId);
            string payload = "{\"UserIds\":[\"" + user1 + "\",\"" + user2 + "\",\"" + user3 + "\"]}";
            var expectRecord = new EventSourcingModel(EventType.RemovePartial, 2, record.EntityId, EntityType.Group, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task RemoveUserGroup_DuplicateUser_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var user1 = await HelperEventSourcing.CreateUser(context, identity, "user1@mavim.com");

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1 });
            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1 }));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<UnprocessableEntityException>(exception);
            Assert.Equal("One or more user(s) to delete do not exist", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task RemoveUserGroup_ValidGroupIdAsUserId_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var groupId2 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Test Group 2", ""));

            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, groupId, new List<Guid> { groupId2 }));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal("Unable to remove an non-existing userId", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task RemoveUserGroup_ListOfDuplicateUserIds_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var user1 = await HelperEventSourcing.CreateUser(context, identity, "user1@mavim.com");
            var user2 = await HelperEventSourcing.CreateUser(context, identity, "user2@mavim.com");

            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1, user1, user2 }));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<UnprocessableEntityException>(exception);
            Assert.Equal("Request contains duplicate items", exception.Message);
        }
        #endregion


        #region AddParial
        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateUserGroup_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var user1 = await HelperEventSourcing.CreateUser(context, identity, "user1@mavim.com");
            var user2 = await HelperEventSourcing.CreateUser(context, identity, "user2@mavim.com");
            var user3 = await HelperEventSourcing.CreateUser(context, identity, "user3@mavim.com");

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1, user2, user3 });

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == groupId);
            string payload = "{\"UserIds\":[\"" + user1 + "\",\"" + user2 + "\",\"" + user3 + "\"]}";
            var expectRecord = new EventSourcingModel(EventType.AddPartial, 1, record.EntityId, EntityType.Group, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateUserGroup_DuplicateUser_BadRequestException()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var user1 = await HelperEventSourcing.CreateUser(context, identity, "user1@mavim.com");

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1 });
            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1 }));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<UnprocessableEntityException>(exception);
            Assert.Equal("Properties to update contains the same results", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateUserGroup_ValidGroupIdAsUserId_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var groupId2 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Test Group 2", ""));

            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, groupId, new List<Guid> { groupId2 }));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal("Unable to add an non-existing userId", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateUserGroup_ListOfDuplicateUserIds_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);
            var user1 = await HelperEventSourcing.CreateUser(context, identity, "user1@mavim.com");
            var user2 = await HelperEventSourcing.CreateUser(context, identity, "user2@mavim.com");

            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, groupId, new List<Guid> { user1, user1, user2 }));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<UnprocessableEntityException>(exception);
            Assert.Equal("Request contains duplicate items", exception.Message);
        }
        #endregion

    }
}
