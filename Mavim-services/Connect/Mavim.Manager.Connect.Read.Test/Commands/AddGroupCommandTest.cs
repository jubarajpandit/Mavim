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
    public class AddGroupCommandCommandTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddGroupCommand_ValidArguments_ListOfGroups()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var handler = new AddGroupCommand.Handler(dbContext);
            var request = new AddGroupCommand.Command(groupId, name, description, companyId, modelVersion, aggregateId);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var savedGroup = await dbContext.Groups.FindAsync(groupId);
            Assert.NotNull(savedGroup);
            Assert.NotNull(savedGroup.Value);
            var groupValue = JsonSerializer.Deserialize<GroupValue>(savedGroup.Value);
            Assert.NotNull(groupValue);
            Assert.Equal(companyId, groupValue.CompanyId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddGroupCommand_RequestNull_ArgumentNullException()
        {
            // Arrange
            var expectedMessage = "Value cannot be null. (Parameter 'request')";
            var dbContext = GetMockContext();
            var handler = new AddGroupCommand.Handler(dbContext);
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
        public async Task AddGroupCommand_LowerAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = -1;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: 0";
            var expectedErrorCode = (int)ErrorCode.AggregateIdLower;
            var handler = new AddGroupCommand.Handler(dbContext);
            var request = new AddGroupCommand.Command(groupId, name, description, companyId, modelVersion, aggregateId);
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
        public async Task AddGroupCommand_HigherAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: 0";
            var expectedErrorCode = (int)ErrorCode.AggregateIdHigher;
            var handler = new AddGroupCommand.Handler(dbContext);
            var request = new AddGroupCommand.Command(groupId, name, description, companyId, modelVersion, aggregateId);
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
        public async Task AddGroupCommand_CompanyAlreadyExists_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, "testvalue", modelVersion, aggregateId, companyId, disabled, lastUpdated);
            var expectedMessage = $"Group with guid {groupId} already exists.";
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            var handler = new AddGroupCommand.Handler(dbContext);
            var request = new AddGroupCommand.Command(groupId, name, description, companyId, modelVersion, aggregateId);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnprocessableEntityException>(result);
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
    }
}
