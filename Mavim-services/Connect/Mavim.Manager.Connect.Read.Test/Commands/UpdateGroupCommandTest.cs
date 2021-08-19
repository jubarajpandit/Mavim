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
    public class UpdateGroupCommandTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task UpdateGroupCommand_UpdatedName_NameUpdated()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var updatedName = "updatedName";
            var modelVersion = 1;
            var aggregateId = 1;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, name, description, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            var handler = new UpdateGroupCommand.Handler(dbContext);
            var request = new UpdateGroupCommand.Command(groupId, updatedName, null, modelVersion, aggregateId + 1);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var result = await dbContext.Groups.FindAsync(groupId);
            Assert.NotNull(result?.Value);
            var groupValue = JsonSerializer.Deserialize<GroupValue>(result.Value);
            Assert.NotNull(groupValue);
            Assert.Equal(updatedName, groupValue.Name);
            Assert.Equal(description, groupValue.Description);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task UpdateGroupCommand_UpdatedDescription_DescriptionUpdated()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var updatedDescription = "updatedDescription";
            var modelVersion = 1;
            var aggregateId = 1;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, name, description, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            var handler = new UpdateGroupCommand.Handler(dbContext);
            var request = new UpdateGroupCommand.Command(groupId, null, updatedDescription, modelVersion, aggregateId + 1); ;
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var result = await dbContext.Groups.FindAsync(groupId);
            Assert.NotNull(result?.Value);
            var groupValue = JsonSerializer.Deserialize<GroupValue>(result.Value);
            Assert.NotNull(groupValue);
            Assert.Equal(name, groupValue.Name);
            Assert.Equal(updatedDescription, groupValue.Description);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task UpdateGroupCommand_UpdatedNameAndDescription_NameAndDescriptionUpdated()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var updatedName = "updatedName";
            var updatedDescription = "updatedDescription";
            var modelVersion = 1;
            var aggregateId = 1;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, name, description, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            var handler = new UpdateGroupCommand.Handler(dbContext);
            var request = new UpdateGroupCommand.Command(groupId, updatedName, updatedDescription, modelVersion, aggregateId + 1); ;
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var result = await dbContext.Groups.FindAsync(groupId);
            Assert.NotNull(result?.Value);
            var groupValue = JsonSerializer.Deserialize<GroupValue>(result.Value);
            Assert.NotNull(groupValue);
            Assert.Equal(updatedName, groupValue.Name);
            Assert.Equal(updatedDescription, groupValue.Description);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task UpdateGroupCommand_RequestNull_ArgumentNullException()
        {
            // Arrange
            var expectedMessage = "Value cannot be null. (Parameter 'request')";
            var dbContext = GetMockContext();
            var handler = new UpdateGroupCommand.Handler(dbContext);
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
        public async Task UpdateGroupCommand_LowerAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var modelVersion = 1;
            var aggregateId = 1;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, name, description, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: {aggregateId + 1}";
            var expectedErrorCode = (int)ErrorCode.AggregateIdLower;
            var handler = new UpdateGroupCommand.Handler(dbContext);
            var request = new UpdateGroupCommand.Command(groupId, name, description, modelVersion, aggregateId);
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
        public async Task UpdateGroupCommand_HigherAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var modelVersion = 1;
            var aggregateId = 1;
            var higherAggregateId = 3;
            var disabled = false;
            var lastUpdated = DateTime.Now;
            var group = new GroupTable(groupId, GetGroupValue(groupId, name, description, companyId), modelVersion, aggregateId, companyId, disabled, lastUpdated);
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            dbContext.Entry(group).State = EntityState.Detached;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {higherAggregateId}, expected aggregateId: {aggregateId + 1}";
            var expectedErrorCode = (int)ErrorCode.AggregateIdHigher;
            var handler = new UpdateGroupCommand.Handler(dbContext);
            var request = new UpdateGroupCommand.Command(groupId, name, description, modelVersion, higherAggregateId);
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
        public async Task UpdateGroupCommand_GroupNotFound_RequestNotFoundException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var groupId = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var modelVersion = 1;
            var aggregateId = 1;
            var expectedMessage = $"Could not find the specified group with id: {groupId}.";
            var handler = new UpdateGroupCommand.Handler(dbContext);
            var request = new UpdateGroupCommand.Command(groupId, name, description, modelVersion, aggregateId);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RequestNotFoundException>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        private string GetGroupValue(Guid groupId, string groupName, string groupDescription, Guid companyId)
        {
            return "{\"Id\": \"" + groupId + "\", \"Name\": \"" + groupName + "\", \"Description\": \"" + groupDescription + "\", \"CompanyId\": \"" + companyId + "\", \"Users\": [] }";
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
