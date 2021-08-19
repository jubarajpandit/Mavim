using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Commands;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Databases.Models;
using Mavim.Manager.Connect.Read.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Read.Test.Commands
{
    public class AddUsersToGroupCommandTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddUsersToGroupCommand_ValidArguments_ListOfGroups()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid> { userId1, userId2 };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            var user1Object = new UserTable(userId1, GetUserValue(userId1, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            var user2Object = new UserTable(userId2, GetUserValue(userId2, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Users.AddRange(user1Object, user2Object);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            dbContext.Entry(user1Object).State = EntityState.Detached;
            dbContext.Entry(user2Object).State = EntityState.Detached;
            var handler = new AddUsersToGroupCommand.Handler(dbContext);
            var request = new AddUsersToGroupCommand.Command(groupId, userIds, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var updatedGroup = await dbContext.Groups.FindAsync(groupId);
            Assert.NotNull(updatedGroup?.Value);
            var groupValue = JsonSerializer.Deserialize<GroupValue>(updatedGroup.Value);
            Assert.NotNull(groupValue);
            Assert.Equal(userIds, groupValue.Users);
            var updatedUser1 = await dbContext.Users.FindAsync(userId1);
            Assert.NotNull(updatedUser1?.Value);
            var userValue1 = JsonSerializer.Deserialize<UserValue>(updatedUser1.Value);
            Assert.Contains(userValue1.Groups, x => x == groupId);
            var userValue2 = JsonSerializer.Deserialize<UserValue>(updatedUser1.Value);
            Assert.Contains(userValue2.Groups, x => x == groupId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddUsersToGroupCommand_RequestNull_ArgumentNullException()
        {
            // Arrange
            var expectedMessage = "Value cannot be null. (Parameter 'request')";
            var dbContext = GetMockContext();
            var handler = new AddUsersToGroupCommand.Handler(dbContext);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentNullException>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddUsersToGroupCommand_GroupNotFound_NotFoundException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid> { userId1, userId2 };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var expectedMessage = $"Could not find the specified group with id: {groupId}.";
            var handler = new AddUsersToGroupCommand.Handler(dbContext);
            var request = new AddUsersToGroupCommand.Command(groupId, userIds, modelVersion, aggregateId);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RequestNotFoundException>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddUsersToGroupCommand_LowerAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid> { userId1, userId2 };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: 1";
            var expectedErrorCode = (int)ErrorCode.AggregateIdLower;
            var handler = new AddUsersToGroupCommand.Handler(dbContext);
            var request = new AddUsersToGroupCommand.Command(groupId, userIds, modelVersion, aggregateId);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnprocessableEntityException>(result);
            var unprocessableEntityException = result as UnprocessableEntityException;
            Assert.Equal(expectedMessage, unprocessableEntityException.Message);
            Assert.Equal(expectedErrorCode, unprocessableEntityException.ErrorCode);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddUsersToGroupCommand_HigherAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid> { userId1, userId2 };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var higherAggregateId = 2;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {higherAggregateId}, expected aggregateId: 1";
            var expectedErrorCode = (int)ErrorCode.AggregateIdHigher;
            var handler = new AddUsersToGroupCommand.Handler(dbContext);
            var request = new AddUsersToGroupCommand.Command(groupId, userIds, modelVersion, higherAggregateId);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnprocessableEntityException>(result);
            var unprocessableEntityException = result as UnprocessableEntityException;
            Assert.Equal(expectedMessage, unprocessableEntityException.Message);
            Assert.Equal(expectedErrorCode, unprocessableEntityException.ErrorCode);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddUsersToGroupCommand_UserNotFound_RequestNotFoundException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var userIds = new List<Guid> { userId };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Could not find user with id: {userId}.";
            var handler = new AddUsersToGroupCommand.Handler(dbContext);
            var request = new AddUsersToGroupCommand.Command(groupId, userIds, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RequestNotFoundException>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        private string GetGroupValue(Guid groupId, Guid companyId)
        {
            return "{\"Id\": \"" + groupId + "\", \"Name\": \"groupName\", \"Description\": \"groupDescription\", \"CompanyId\": \"" + companyId + "\", \"Users\": [] }";
        }

        private string GetUserValue(Guid userId, Guid companyId)
        {
            return "{\"Id\": \"" + userId + "\", \"Email\": \"testemail@mavim.com\", \"CompanyId\": \"" + companyId + "\", \"Groups\": [] }";
        }

        private static ConnectDatabaseContext GetMockContext()
        {
            var options = new DbContextOptionsBuilder<ConnectDatabaseContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new ConnectDatabaseContext(options);

            return context;
        }
    }
}
