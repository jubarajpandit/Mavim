using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Mavim.Manager.Connect.Read.Databases.Models;
using Mavim.Manager.Connect.Read.Models;
using Mavim.Manager.Connect.Read.Models.Interfaces;
using Mavim.Manager.Connect.Read.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Read.Test.Queries
{
    public class GetMyCompanyQueryTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetMyCompanyQuery_ValidArguments_Company()
        {
            // Arrange
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            mockUser.Setup(x => x.Id).Returns(userId);
            var mockRepository = new Mock<IConnectRepository>();
            var userValue = new UserValue(userId, "Email", companyId, new List<Guid> { userId });
            var mockUserTable = new UserTable(userId, JsonSerializer.Serialize(userValue), 0, 0, companyId, false, DateTime.Now);
            var companyValue = new CompanyValue(companyId, "companyName", "companyDomain", tenantId);
            var mockCompanyTable = new CompanyTable(companyId, JsonSerializer.Serialize(companyValue), 0, 0, companyId, false, DateTime.Now);
            mockRepository.Setup(x => x.GetUser(userId)).ReturnsAsync(mockUserTable);
            mockRepository.Setup(x => x.GetCompany(companyId)).ReturnsAsync(mockCompanyTable);
            var handler = new GetMyCompany.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetMyCompany.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(companyId, result.Id);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetMyCompanyQuery_UserEmpty_ForbiddenRequestException()
        {
            // Arrange
            var exceptionMessage = "You are not allowed to make this request.";
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var mockRepository = new Mock<IConnectRepository>();
            mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>())).ReturnsAsync((UserTable)null);
            var handler = new GetMyCompany.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetMyCompany.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetMyCompanyQuery_CompanyEmpty_ListOfGroups()
        {
            // Arrange
            var exceptionMessage = "Could not find the company with your user.";
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            mockUser.Setup(x => x.Id).Returns(userId);
            var mockRepository = new Mock<IConnectRepository>();
            var userValue = new UserValue(userId, "Email", companyId, new List<Guid> { userId });
            var mockUserTable = new UserTable(userId, JsonSerializer.Serialize(userValue), 0, 0, companyId, false, DateTime.Now);
            mockRepository.Setup(x => x.GetUser(userId)).ReturnsAsync(mockUserTable);
            mockRepository.Setup(x => x.GetCompany(companyId)).ReturnsAsync((CompanyTable)null);
            var handler = new GetMyCompany.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetMyCompany.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
