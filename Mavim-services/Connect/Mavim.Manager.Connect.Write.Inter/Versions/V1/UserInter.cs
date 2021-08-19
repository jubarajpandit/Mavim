using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Inter.Versions.V1
{
    public class UserInter
    {
        [Fact]
        [Trait("Category", "Connect AddGroupCommand Write")]
        public async Task AddUserCommand_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var userId = await HelperEventSourcing.CreateUser(context, identity, "test@mavim.com");

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == userId);
            string payload = "{\"Email\":\"test@mavim.com\",\"CompanyId\":\"" + identity.CompanyId + "\",\"Id\":\"" + record.EntityId + "\",\"IsActive\":true}";
            var expectRecord = new EventSourcingModel(EventType.Create, 0, record.EntityId, EntityType.User, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }

        #region Delete User
        [Fact]
        [Trait("Category", "Connect AddGroupCommand Write")]
        public async Task DeleteGroupCommand_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var userId = await HelperEventSourcing.CreateUser(context, identity, "test@mavim.com");
            await HelperEventSourcing.DeleteUser(context, identity, userId);

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == userId);
            string payload = "{\"IsActive\":false}";
            var expectRecord = new EventSourcingModel(EventType.Delete, 1, record.EntityId, EntityType.User, 1, payload, record.TimeStamp, record.CompanyId);

            Assert.Equal(expectRecord, record);
        }

        [Fact]
        [Trait("Category", "Connect AddGroupCommand Write")]
        public async Task DeleteGroupCommand_ValidGroupID_RequestNotFoundException()
        {
            // Arrange
            var context = HelperEventSourcing.GetMockContext();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var groupId = await HelperEventSourcing.CreateGroup(context, identity);

            Exception exception = await Record.ExceptionAsync(() => HelperEventSourcing.DeleteUser(context, identity, groupId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"User {groupId} not found", exception.Message);
        }

        #endregion
    }
}
