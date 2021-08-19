using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Commands;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Databases.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Read.Test.Commands
{
    public class EnableUserCommandTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task EnableUserCommand_ValidArguments_GroupDisabledAndUsersUpdated()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = true;
            var lastUpdated = DateTime.Now;
            var userObject = new UserTable(userId, "testvalue", modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Users.Add(userObject);
            dbContext.SaveChanges();
            dbContext.Entry(userObject).State = EntityState.Detached;
            var handler = new EnableUserCommand.Handler(dbContext);
            var request = new EnableUserCommand.Command(userId, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var disabledUser = await dbContext.Users.FindAsync(userId);
            Assert.NotNull(disabledUser);
            Assert.False(disabledUser.Disabled);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task EnableUserCommand_RequestNull_ArgumentNullException()
        {
            // Arrange
            var expectedMessage = "Value cannot be null. (Parameter 'request')";
            var dbContext = GetMockContext();
            var handler = new EnableUserCommand.Handler(dbContext);
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
        public async Task EnableUserCommand_LowerAggregateId_UnprocessableEntityException()
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
            var userObject = new UserTable(userId, "testvalue", modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Users.Add(userObject);
            dbContext.SaveChanges();
            dbContext.Entry(userObject).State = EntityState.Detached;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: {aggregateId + 1}";
            var expectedErrorCode = (int)ErrorCode.AggregateIdLower;
            var handler = new EnableUserCommand.Handler(dbContext);
            var request = new EnableUserCommand.Command(userId, modelVersion, aggregateId);
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
        public async Task EnableUserCommand_HigherAggregateId_UnprocessableEntityException()
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
            var userObject = new UserTable(userId, "testvalue", modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Users.Add(userObject);
            dbContext.SaveChanges();
            dbContext.Entry(userObject).State = EntityState.Detached;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {higherAggregateId}, expected aggregateId: {aggregateId + 1}";
            var expectedErrorCode = (int)ErrorCode.AggregateIdHigher;
            var handler = new EnableUserCommand.Handler(dbContext);
            var request = new EnableUserCommand.Command(userId, modelVersion, higherAggregateId);
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
        public async Task EnableUserCommand_IncorrectAggregateId_ConflictException()
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
            var userObject = new UserTable(userId, "testvalue", modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Users.Add(userObject);
            dbContext.SaveChanges();
            dbContext.Entry(userObject).State = EntityState.Detached;
            var expectedMessage = $"User with id: {userId} is already enabled";
            var handler = new EnableUserCommand.Handler(dbContext);
            var request = new EnableUserCommand.Command(userId, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ConflictException>(result);
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

        private string GetUserValue(Guid userId, Guid companyId, Guid groupId)
        {
            return "{\"Id\": \"" + userId + "\", \"Email\": \"testemail@mavim.com\", \"CompanyId\": \"" + companyId + "\", \"Groups\": [\"" + groupId + "\"] }";
        }
    }
}
