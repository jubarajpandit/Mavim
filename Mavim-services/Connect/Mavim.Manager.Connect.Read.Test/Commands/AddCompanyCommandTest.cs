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
    public class AddCompanyCommandTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddCompanyCommand_ValidArguments_ListOfGroups()
        {
            // Arrange
            var dbContext = GetMockContext();
            var companyId = Guid.NewGuid();
            var name = "name";
            var domain = "domain";
            var tenantId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var handler = new AddCompanyCommand.Handler(dbContext);
            var request = new AddCompanyCommand.Command(companyId, name, domain, tenantId, modelVersion, aggregateId);
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            var savedCompany = await dbContext.Companies.FindAsync(companyId);
            Assert.NotNull(savedCompany);
            Assert.NotNull(savedCompany.Value);
            var companyValue = JsonSerializer.Deserialize<CompanyValue>(savedCompany.Value);
            Assert.NotNull(companyValue);
            Assert.Equal(tenantId, companyValue.TenantId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task AddCompanyCommand_RequestNull_ArgumentNullException()
        {
            // Arrange
            var expectedMessage = "Value cannot be null. (Parameter 'request')";
            var dbContext = GetMockContext();
            var handler = new AddCompanyCommand.Handler(dbContext);
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
        public async Task AddCompanyCommand_LowerAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var companyId = Guid.NewGuid();
            var name = "name";
            var domain = "domain";
            var tenantId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = -1;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: 0";
            var expectedErrorCode = (int)ErrorCode.AggregateIdLower;
            var handler = new AddCompanyCommand.Handler(dbContext);
            var request = new AddCompanyCommand.Command(companyId, name, domain, tenantId, modelVersion, aggregateId);
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
        public async Task AddCompanyCommand_HigherAggregateId_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var companyId = Guid.NewGuid();
            var name = "name";
            var domain = "domain";
            var tenantId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var expectedMessage = $"Supplied object contains an invalid aggregateId: {aggregateId}, expected aggregateId: 0";
            var expectedErrorCode = (int)ErrorCode.AggregateIdHigher;
            var handler = new AddCompanyCommand.Handler(dbContext);
            var request = new AddCompanyCommand.Command(companyId, name, domain, tenantId, modelVersion, aggregateId);
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
        public async Task AddCompanyCommand_CompanyAlreadyExists_UnprocessableEntityException()
        {
            // Arrange
            var dbContext = GetMockContext();
            var companyId = Guid.NewGuid();
            var name = "name";
            var domain = "domain";
            var tenantId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 0;
            var lastUpdated = DateTime.Now;
            var company = new CompanyTable(companyId, "testvalue", modelVersion, aggregateId, companyId, false, lastUpdated);
            var expectedMessage = $"Company with guid {companyId} already exists.";
            dbContext.Companies.Add(company);
            dbContext.SaveChanges();
            var handler = new AddCompanyCommand.Handler(dbContext);
            var request = new AddCompanyCommand.Command(companyId, name, domain, tenantId, modelVersion, aggregateId);
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
