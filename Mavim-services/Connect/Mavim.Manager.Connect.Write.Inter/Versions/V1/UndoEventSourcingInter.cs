using Mavim.Manager.Connect.Write.Commands;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.EventSourcing;
using Mavim.Manager.Connect.Write.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Inter.Versions.V1
{
    public class UndoEventSourcingInter
    {
        [Fact]
        [Trait("Category", "Connect UndoEventSourcingCommand Write")]
        public async Task UndoEventSourcingCommand_ValidArguments_ValidModelStateAsync()
        {
            // Initialize Database
            // Arrange
            Database.ConnectDbContext context = HelperEventSourcing.GetMockContext();

            // Create Entities
            // Company
            var companyId = await HelperEventSourcing.CreateCompany(context, "Test", "test.nl", Guid.NewGuid());
            var identity = new IdentityService(Guid.Empty, Guid.Empty, companyId);

            // Group
            var group1 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group1", "Description1"));
            var group2 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group2", "Description2"));
            var group3 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group3", "Description3"));
            var group4 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group4", "Description4"));

            // Users
            var user1 = await HelperEventSourcing.CreateUser(context, identity, "user1@mavim.com");
            var user2 = await HelperEventSourcing.CreateUser(context, identity, "user2@mavim.com");
            var user3 = await HelperEventSourcing.CreateUser(context, identity, "user3@mavim.com");

            // Update Entities
            // Group
            await HelperEventSourcing.UpdateGroup(context, identity, new UpdateGroup.Command(group1, "Group1-1", string.Empty));
            await HelperEventSourcing.UpdateGroup(context, identity, new UpdateGroup.Command(group3, "Group3-1", string.Empty));

            // Add Partial // Remove Partial
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group1, new List<Guid> { user1, user2, user3 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group1, new List<Guid> { user1, user3 });

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group2, new List<Guid> { user1 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group2, new List<Guid> { user2 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group2, new List<Guid> { user3 });

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group4, new List<Guid> { user1 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group4, new List<Guid> { user2 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group4, new List<Guid> { user3 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group4, new List<Guid> { user1 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group4, new List<Guid> { user2 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group4, new List<Guid> { user3 });

            await HelperEventSourcing.DeleteGroup(context, identity, group2);
            await HelperEventSourcing.DeleteUser(context, identity, user1);

            var allRecords = await context.EventSourcings.AsNoTracking().ToListAsync();

            // Make a fixt date to undo events
            DateTime? lastDateTime = null;
            var allRecordsWithNewTimeStamp = allRecords.OrderByDescending(e => e.TimeStamp).Select(e => Map(e, ref lastDateTime)).OrderBy(e => e.TimeStamp).ToList();
            allRecordsWithNewTimeStamp.ForEach(@event => Update(context, @event));

            // Group
            var group5 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group5", "Description5"));
            var group6 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group6", "Description6"));
            var group7 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group7", "Description7"));
            var group8 = await HelperEventSourcing.CreateGroup(context, identity, new AddGroup.Command("Group8", "Description8"));

            // Users
            var user4 = await HelperEventSourcing.CreateUser(context, identity, "user4@mavim.com");
            var user5 = await HelperEventSourcing.CreateUser(context, identity, "user5@mavim.com");
            var user6 = await HelperEventSourcing.CreateUser(context, identity, "user6@mavim.com");

            // Update Entities
            // Group
            await HelperEventSourcing.UpdateGroup(context, identity, new UpdateGroup.Command(group5, null, "Description5-1"));
            await HelperEventSourcing.UpdateGroup(context, identity, new UpdateGroup.Command(group7, null, "Description7-1"));

            // Add Partial // Remove Partial
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group1, new List<Guid> { user4, user5, user6 });

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group3, new List<Guid> { user4 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group3, new List<Guid> { user5 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group3, new List<Guid> { user6 });

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group4, new List<Guid> { user4 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group4, new List<Guid> { user5 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group4, new List<Guid> { user6 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group4, new List<Guid> { user4 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group4, new List<Guid> { user5 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group4, new List<Guid> { user6 });


            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group5, new List<Guid> { user4 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group5, new List<Guid> { user2 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group5, new List<Guid> { user3 });

            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group8, new List<Guid> { user4 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group8, new List<Guid> { user2 });
            await HelperEventSourcing.AddPartialUserIdsToGroup(context, identity, group8, new List<Guid> { user3 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group8, new List<Guid> { user4 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group8, new List<Guid> { user2 });
            await HelperEventSourcing.RemovePartialUserIdsToGroup(context, identity, group8, new List<Guid> { user3 });

            await HelperEventSourcing.UndoEventSourcing(context, identity, new DateTime(2000, 1, 1, 1, 1, 2));

            var allRecordsAfterUndo = await context.EventSourcings.ToListAsync();

            var compES = new CompanyV1EventSourcing(context);
            var userES = new UserV1EventSourcing(context);
            var groupES = new GroupV1EventSourcing(context);

            var entityIds = allRecordsWithNewTimeStamp.Select(e => e.EntityId).Distinct().ToList();

            // Before StartDate
            foreach (var id in entityIds)
            {
                var typeEvent = allRecordsWithNewTimeStamp.First(a => a.EntityId == id).EntityType;
                var listOfEvents = allRecordsWithNewTimeStamp.Where(b => b.EntityId == id).ToList();
                var listOfEventsAfterUndo = allRecordsAfterUndo.Where(b => b.EntityId == id).ToList();
                var eventSourcing = GetEventSourcing(typeEvent, userES, groupES, compES);
                var oldEvent = await eventSourcing.PlayEvents(listOfEvents);
                var newEvent = await eventSourcing.PlayEvents(listOfEventsAfterUndo);
                Assert.Equal(oldEvent.ToString(), newEvent.ToString());
            }

            // After StartDate
            // (all new created entities should be inactive)
            var entityIdsAfter = allRecordsAfterUndo.Select(e => e.EntityId).Distinct().ToList();
            entityIdsAfter = entityIdsAfter.Where(e => !entityIds.Contains(e)).ToList();
            foreach (var id in entityIdsAfter)
            {
                var typeEvent = allRecordsAfterUndo.First(a => a.EntityId == id).EntityType;
                var listOfEventsAfterUndo = allRecordsAfterUndo.Where(b => b.EntityId == id).ToList();
                var eventSourcing = GetEventSourcing(typeEvent, userES, groupES, compES);
                var newEvent = await eventSourcing.PlayEvents(listOfEventsAfterUndo);
                Assert.False(newEvent.IsActive);
            }

        }

        private static EventSourcingModel Map(EventSourcingModel g, ref DateTime? lastDateTime)
        {
            lastDateTime = lastDateTime?.AddSeconds(-1) ?? new DateTime(2000, 1, 1, 1, 1, 1);
            return g with
            {
                TimeStamp = (DateTime)lastDateTime
            };
        }

        private static void Update(Database.ConnectDbContext context, EventSourcingModel entity)
        {
            var entry = context.EventSourcings.First(e => e.EntityId == entity.EntityId && e.AggregateId == entity.AggregateId);
            context.Entry(entry).CurrentValues.SetValues(entity);
            context.SaveChanges();
        }

        private static dynamic GetEventSourcing(EntityType entityType, UserV1EventSourcing userEventSourcing, GroupV1EventSourcing groupEventSourcing, CompanyV1EventSourcing companyEventSourcing)
        {
            return entityType switch
            {
                EntityType.User => userEventSourcing,
                EntityType.Group => groupEventSourcing,
                EntityType.Company => companyEventSourcing,
                _ => throw new NotImplementedException($"Entity type of {entityType} is unknown")
            };
        }

    }
}
