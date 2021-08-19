using Mavim.Manager.Connect.Write.Commands;
using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.EventSourcing;
using Mavim.Manager.Connect.Write.Identity;
using Mavim.Manager.Connect.Write.Inter.Mock;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Inter.Versions.V1
{
    public static class HelperEventSourcing
    {
        #region Company
        public static async Task<Guid> CreateCompany(ConnectDbContext context, string name, string domain, Guid tenantId)
        {
            var eventSourcing = new CompanyV1EventSourcing(context);
            var handler = new AddCompany.Handler(eventSourcing, new QueueClientMock());
            var addCommand = new AddCompany.Command(name, domain, tenantId);

            return await handler.Handle(addCommand, new CancellationToken());
        }

        public static async Task UndoEventSourcing(ConnectDbContext context, IdentityService identity, DateTime startDate)
        {
            var eventSourcingCompany = new CompanyV1EventSourcing(context);
            var eventSourcingGroup = new GroupV1EventSourcing(context);
            var eventSourcingUser = new UserV1EventSourcing(context);
            var eventSourcingUndo = new CommonEventSourcing(context);
            var handler = new UndoEventSourcing.Handler(eventSourcingUser, eventSourcingGroup, eventSourcingCompany, eventSourcingUndo, identity, new BatchQueueClientMock());

            await handler.Handle(new UndoEventSourcing.Command(startDate), new CancellationToken());
        }
        #endregion

        #region Users
        public static async Task<Guid> CreateUser(ConnectDbContext context, IdentityService identity, string email)
        {
            var eventSourcing = new UserV1EventSourcing(context);
            var handler = new AddUser.Handler(eventSourcing, identity, new QueueClientMock());
            var addCommand = new AddUser.Command(email);

            return await handler.Handle(addCommand, new CancellationToken());
        }
        public static async Task DeleteUser(ConnectDbContext context, IdentityService identity, Guid userId)
        {
            var eventSourcing = new UserV1EventSourcing(context);
            var handler = new DeleteUser.Handler(eventSourcing, identity, new QueueClientMock());
            var addCommand = new DeleteUser.Command(userId);

            await handler.Handle(addCommand, new CancellationToken());
        }
        #endregion

        #region Groups
        public static async Task AddPartialUserIdsToGroup(ConnectDbContext context, IdentityService identity, Guid groupId, List<Guid> userIds)
        {
            var eventSourcing = new GroupV1EventSourcing(context);
            var eventSourcingUser = new UserV1EventSourcing(context);
            var handler = new UpdateUserGroup.Handler(eventSourcing, eventSourcingUser, identity, new QueueClientMock());

            await handler.Handle(new UpdateUserGroup.Command(groupId, userIds), new CancellationToken());
        }

        public static async Task RemovePartialUserIdsToGroup(ConnectDbContext context, IdentityService identity, Guid groupId, List<Guid> userIds)
        {
            var eventSourcing = new GroupV1EventSourcing(context);
            var eventSourcingUser = new UserV1EventSourcing(context);
            var handler = new DeleteUserGroup.Handler(eventSourcing, eventSourcingUser, identity, new QueueClientMock());

            await handler.Handle(new DeleteUserGroup.Command(groupId, userIds), new CancellationToken());
        }



        public static async Task<Guid> CreateGroup(ConnectDbContext context, IdentityService identity, AddGroup.Command command = null)
        {

            var eventSourcing = new GroupV1EventSourcing(context);
            var handler = new AddGroup.Handler(eventSourcing, identity, new QueueClientMock());
            var addCommand = command ?? new AddGroup.Command("Test", "Description");

            return await handler.Handle(addCommand, new CancellationToken());
        }

        public static async Task DeleteGroup(ConnectDbContext context, IdentityService identity, Guid groupId)
        {

            var eventSourcing = new GroupV1EventSourcing(context);
            var handler = new DeleteGroup.Handler(eventSourcing, identity, new QueueClientMock());
            var command = new DeleteGroup.Command(groupId);

            await handler.Handle(command, new CancellationToken());
        }

        public static async Task UpdateGroup(ConnectDbContext context, IdentityService identity, UpdateGroup.Command command)
        {

            var eventSourcing = new GroupV1EventSourcing(context);
            var handler = new UpdateGroup.Handler(eventSourcing, identity, new QueueClientMock());

            await handler.Handle(command, new CancellationToken());
        }
        #endregion
        public static ConnectDbContext GetMockContext()
        {
            var options = new DbContextOptionsBuilder<ConnectDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new ConnectDbContext(options);

            return context;
        }
    }
}
