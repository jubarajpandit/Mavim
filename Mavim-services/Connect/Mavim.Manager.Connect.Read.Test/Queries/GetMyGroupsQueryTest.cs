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
    public class GetMyGroupsQueryTest
    {
        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetMyGroupsQuery_ValidArguments_ListOfGroups()
        {
            // Arrange
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var disabledUserId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            mockUser.Setup(x => x.Id).Returns(userId);
            var mockRepository = new Mock<IConnectRepository>();
            var userValue = new UserValue(userId, "Email", companyId, new List<Guid> { groupId });
            var mockUserTable = new UserTable(userId, JsonSerializer.Serialize(userValue), 0, 0, companyId, false, DateTime.Now);
            var groupValue = new GroupValue(groupId, "groupName", "groupDescription", companyId, new List<Guid> { userId });
            var mockGroupTable = new GroupTable(groupId, JsonSerializer.Serialize(groupValue), 0, 0, companyId, false, DateTime.Now);
            var mockDisabledUserTable = new UserTable(disabledUserId, "", 0, 0, companyId, true, DateTime.Now);
            mockRepository.Setup(x => x.GetUsers(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<UserTable> { mockUserTable, mockDisabledUserTable });
            mockRepository.Setup(x => x.GetUser(userId)).ReturnsAsync(mockUserTable);
            mockRepository.Setup(x => x.GetGroups(new Guid[] { groupId })).ReturnsAsync(new GroupTable[] { mockGroupTable });
            var handler = new GetMyGroups.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetMyGroups.Query();
            var cancellationToken = new System.Threading.CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(groupId, result[0]?.Id);
            Assert.Contains(result[0].Users, x => x == userId);
            Assert.DoesNotContain(result[0].Users, x => x == disabledUserId);
        }

        [Fact]
        [Trait("category", "connectRead")]
        public async Task GetMyGroupByIdQuery_UserEmpty_ForbiddenRequestException()
        {
            // Arrange
            var exceptionMessage = "You are not allowed to make this request.";
            var mockUser = new Mock<IUserIdentity>();
            var userId = Guid.NewGuid();
            var mockRepository = new Mock<IConnectRepository>();
            mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>())).ReturnsAsync((UserTable)null);
            var handler = new GetMyGroups.Handler(mockUser.Object, mockRepository.Object);
            var request = new GetMyGroups.Query();
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
