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
    public class GetCompanyUsersQueryTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetCompanyUsersQuery_ValidArguments_ListOfGroups()
        {
            // Arrange
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            mockUser.Setup(x => x.Id).Returns(userId);
            var mockRepository = new Mock<IConnectRepository>();
            var userValue = new UserValue(userId, "Email", companyId, new List<Guid> { userId });
            var mockUserTable = new UserTable(userId, JsonSerializer.Serialize(userValue), 0, 0, companyId, false, DateTime.Now);
            mockRepository.Setup(x => x.GetUser(userId)).ReturnsAsync(mockUserTable);
            mockRepository.Setup(x => x.GetCompanyUsers(companyId)).ReturnsAsync(new List<UserTable> { mockUserTable });

            var handler = new GetCompanyUsers.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetCompanyUsers.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result[0]?.Id);
            Assert.Equal(companyId, result[0]?.CompanyId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetCompanyUsersQuery_UserEmpty_ForbiddenRequestException()
        {
            // Arrange
            var exceptionMessage = "You are not allowed to make this request.";
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var mockRepository = new Mock<IConnectRepository>();
            mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>())).ReturnsAsync((UserTable)null);
            var handler = new GetCompanyUsers.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetCompanyUsers.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(request, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
