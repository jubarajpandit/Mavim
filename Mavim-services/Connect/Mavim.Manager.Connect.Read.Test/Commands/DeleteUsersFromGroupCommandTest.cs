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
    public class DeleteUsersFromGroupCommandTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task DeleteUsersFromGroupCommand_ValidArguments_ListOfGroups()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid> { userId1, userId2 };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var groupValue = new GroupValue(groupId, groupName, groupDescription, companyId, userIds);
            var group = new GroupTable(groupId, JsonSerializer.Serialize(groupValue), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            var user1Object = new UserTable(userId1, GetUserValue(userId1, companyId, groupId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            var user2Object = new UserTable(userId2, GetUserValue(userId2, companyId, groupId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Users.AddRange(user1Object, user2Object);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            dbContext.Entry(user1Object).State = EntityState.Detached;
            dbContext.Entry(user2Object).State = EntityState.Detached;
            var handler = new DeleteUsersFromGroupCommand.Handler(dbContext);
            var request = new DeleteUsersFromGroupCommand.Command(groupId, userIds, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var updatedGroup = await dbContext.Groups.FindAsync(groupId);
            Assert.NotNull(updatedGroup?.Value);
            var updatedGroupValue = JsonSerializer.Deserialize<GroupValue>(updatedGroup.Value);
            Assert.NotNull(updatedGroupValue);
            Assert.Empty(updatedGroupValue.Users);
            var updatedUser1 = await dbContext.Users.FindAsync(userId1);
            Assert.NotNull(updatedUser1?.Value);
            var userValue1 = JsonSerializer.Deserialize<UserValue>(updatedUser1.Value);
            Assert.DoesNotContain(userValue1.Groups, x => x == groupId);
            var userValue2 = JsonSerializer.Deserialize<UserValue>(updatedUser1.Value);
            Assert.DoesNotContain(userValue2.Groups, x => x == groupId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task DeleteUsersFromGroupCommand_RequestNull_ArgumentNullException()
        {
            // Arrange
            var expectedMessage = "Value cannot be null. (Parameter 'request')";
            var dbContext = GetMockContext();
            var handler = new DeleteUsersFromGroupCommand.Handler(dbContext);
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
        public async Task DeleteUsersFromGroupCommand_GroupNotFound_NotFoundException()
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
            var handler = new DeleteUsersFromGroupCommand.Handler(dbContext);
            var request = new DeleteUsersFromGroupCommand.Command(groupId, userIds, modelVersion, aggregateId);
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
        public async Task DeleteUsersFromGroupCommand_LowerAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid> { userId1, userId2 };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var groupValue = new GroupValue(groupId, groupName, groupDescription, companyId, userIds);
            var group = new GroupTable(groupId, JsonSerializer.Serialize(groupValue), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: 1";
            var expectedErrorCode = (int)ErrorCode.AggregateIdLower;
            var handler = new DeleteUsersFromGroupCommand.Handler(dbContext);
            var request = new DeleteUsersFromGroupCommand.Command(groupId, userIds, modelVersion, aggregateId);
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
        public async Task DeleteUsersFromGroupCommand_HigherAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid> { userId1, userId2 };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var higherAggregateId = 2;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var groupValue = new GroupValue(groupId, groupName, groupDescription, companyId, userIds);
            var group = new GroupTable(groupId, JsonSerializer.Serialize(groupValue), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {higherAggregateId}, expected aggregateId: 1";
            var expectedErrorCode = (int)ErrorCode.AggregateIdHigher;
            var handler = new DeleteUsersFromGroupCommand.Handler(dbContext);
            var request = new DeleteUsersFromGroupCommand.Command(groupId, userIds, modelVersion, higherAggregateId);
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
        public async Task DeleteUsersFromGroupCommand_UserNotFound_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var userId = Guid.NewGuid();
            var userIds = new List<Guid> { userId };
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var groupValue = new GroupValue(groupId, groupName, groupDescription, companyId, userIds);
            var group = new GroupTable(groupId, JsonSerializer.Serialize(groupValue), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Could not find user with id: {userId}.";
            var handler = new DeleteUsersFromGroupCommand.Handler(dbContext);
            var request = new DeleteUsersFromGroupCommand.Command(groupId, userIds, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnprocessableEntityException>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        private string GetUserValue(Guid userId, Guid companyId, Guid groupId)
        {
            return "{\"Id\": \"" + userId + "\", \"Email\": \"testemail@mavim.com\", \"CompanyId\": \"" + companyId + "\", \"Groups\": [\"" + groupId + "\"] }";
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
