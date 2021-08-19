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
    public class GetCompanyGroupsQueryTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetCompanyGroupsQuery_ValidArguments_ListOfGroups()
        {
            // Arrange
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var groupValue = new GroupValue(groupId, "groupName", "groupDescription", companyId, new List<Guid> { userId });
            var mockGroup = new GroupTable(groupId, JsonSerializer.Serialize(groupValue), 0, 0, companyId, false, DateTime.Now);
            mockUser.Setup(x => x.Id).Returns(userId);
            var mockRepository = new Mock<IConnectRepository>();
            var mockUserTable = new UserTable(userId, "", 0, 0, companyId, false, DateTime.Now);
            mockRepository.Setup(x => x.GetUser(userId)).ReturnsAsync(mockUserTable);
            mockRepository.Setup(x => x.GetCompanyGroups(companyId)).ReturnsAsync(new List<GroupTable> { mockGroup });
            mockRepository.Setup(x => x.GetUsers(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<UserTable> { mockUserTable });
            var handler = new GetCompanyGroups.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetCompanyGroups.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result[0]?.CompanyId, companyId);
            Assert.Contains(result[0].Users, x => x == userId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetCompanyGroupsQuery_ValidArgumentsWithDisabledUsers_ListOfGroups()
        {
            // Arrange
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var disabledUserId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var groupValue = new GroupValue(groupId, "groupName", "groupDescription", companyId, new List<Guid> { userId });
            var mockGroup = new GroupTable(groupId, JsonSerializer.Serialize(groupValue), 0, 0, companyId, false, DateTime.Now);
            mockUser.Setup(x => x.Id).Returns(userId);
            var mockRepository = new Mock<IConnectRepository>();
            var mockUserTable = new UserTable(userId, "", 0, 0, companyId, false, DateTime.Now);
            var mockDisabledUserTable = new UserTable(disabledUserId, "", 0, 0, companyId, true, DateTime.Now);
            mockRepository.Setup(x => x.GetUser(userId)).ReturnsAsync(mockUserTable);
            mockRepository.Setup(x => x.GetCompanyGroups(companyId)).ReturnsAsync(new List<GroupTable> { mockGroup });
            mockRepository.Setup(x => x.GetUsers(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<UserTable> { mockUserTable, mockDisabledUserTable });
            var handler = new GetCompanyGroups.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetCompanyGroups.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result[0]?.CompanyId, companyId);
            Assert.Contains(result[0].Users, x => x == userId);
            Assert.DoesNotContain(result[0].Users, x => x == disabledUserId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetCompanyGroupsQuery_UserEmpty_ForbiddenRequestException()
        {
            // Arrange
            var exceptionMessage = "You are not allowed to make this request.";
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var mockRepository = new Mock<IConnectRepository>();
            mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>())).ReturnsAsync((UserTable)null);
            var handler = new GetCompanyGroups.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetCompanyGroups.Query();
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
