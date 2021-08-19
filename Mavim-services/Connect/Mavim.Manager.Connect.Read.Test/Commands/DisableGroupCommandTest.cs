using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Commands;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Databases.Models;
using Mavim.Manager.Connect.Read.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Read.Test.Commands
{
    public class DisableGroupCommandTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task DisableGroupCommand_ValidArguments_GroupDisabledAndUsersUpdated()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId, userId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            var userObject = new UserTable(userId, GetUserValue(userId, companyId, groupId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Users.Add(userObject);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            dbContext.Entry(userObject).State = EntityState.Detached;
            var handler = new DisableGroupCommand.Handler(dbContext);
            var request = new DisableGroupCommand.Command(groupId, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var disabledGroup = await dbContext.Groups.FindAsync(groupId);
            Assert.NotNull(disabledGroup);
            Assert.True(disabledGroup.Disabled);
            var updatedUser = await dbContext.Users.FindAsync(userId);
            Assert.NotNull(updatedUser?.Value);
            var updatedUserValue = JsonSerializer.Deserialize<UserValue>(updatedUser.Value);
            Assert.DoesNotContain(updatedUserValue.Groups, x => x == groupId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task DisableGroupCommand_RequestNull_ArgumentNullException()
        {
            // Arrange
            var expectedMessage = "Value cannot be null. (Parameter 'request')";
            var dbContext = GetMockContext();
            var handler = new DisableGroupCommand.Handler(dbContext);
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
        public async Task DisableGroupCommand_LowerAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId, userId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: {aggregateId + 1}";
            var expectedErrorCode = (int)ErrorCode.AggregateIdLower;
            var handler = new DisableGroupCommand.Handler(dbContext);
            var request = new DisableGroupCommand.Command(groupId, modelVersion, aggregateId);
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
        public async Task DisableGroupCommand_HigherAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var higherAggregateId = 2;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId, userId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {higherAggregateId}, expected aggregateId: {aggregateId + 1}";
            var expectedErrorCode = (int)ErrorCode.AggregateIdHigher;
            var handler = new DisableGroupCommand.Handler(dbContext);
            var request = new DisableGroupCommand.Command(groupId, modelVersion, higherAggregateId);
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
        public async Task DisableGroupCommand_UserInGroupDoesNotExist_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, companyId, userId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var expectedMessage = $"Could not find user with id: {userId}.";
            var handler = new DisableGroupCommand.Handler(dbContext);
            var request = new DisableGroupCommand.Command(groupId, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RequestNotFoundException>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        private static ConnectDatabaseContext GetMockContext()
        {
            var options = new DbContextOptionsBuilder<ConnectDatabaseContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new ConnectDatabaseContext(options);

            return context;
        }

        private string GetGroupValue(Guid groupId, Guid companyId, Guid userId)
        {
            return "{\"Id\": \"" + groupId + "\", \"Name\": \"groupName\", \"Description\": \"groupDescription\", \"CompanyId\": \"" + companyId + "\", \"Users\": [\"" + userId + "\"] }";
        }

        private string GetUserValue(Guid userId, Guid companyId, Guid groupId)
        {
            return "{\"Id\": \"" + userId + "\", \"Email\": \"testemail@mavim.com\", \"CompanyId\": \"" + companyId + "\", \"Groups\": [\"" + groupId + "\"] }";
        }
    }
}
