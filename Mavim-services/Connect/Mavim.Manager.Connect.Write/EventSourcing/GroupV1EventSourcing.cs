using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mavim.Manager.Connect.Write.EventSourcing
{
    public class GroupV1EventSourcing : EventSourcingGeneric<GroupV1>, IEventSourcingGeneric<GroupV1>
    {
        public GroupV1EventSourcing(ConnectDbContext dbContext) : base(dbContext, EntityType.Group)
        {
        }

        protected override GroupV1 Update(GroupV1 entity, GroupV1 payload, GroupV1 newPayload)
        {
            var (_, Name, Description, _, _, _) = payload;

            if (entity.Name != Name && !string.IsNullOrWhiteSpace(Name))
                newPayload = newPayload with { Name = Name };
            if (entity.Description != Description && Description != null)
                newPayload = newPayload with { Description = Description };

            return newPayload;
        }

        protected override GroupV1 AddPartialEntity(GroupV1 entity, GroupV1 payload, bool getPayload)
        {
            var (_, _, _, _, UserIds, _) = payload;

            var newUserIds = getPayload
                    ? UserIds.Except(entity.UserIds).ToList()
                    : UserIds?.Concat(entity.UserIds ?? new List<Guid>()).Distinct().ToList();

            // new group to return null properties for json serialization where we ignore null properties
            var newEntity = getPayload ? new GroupV1() : entity;

            return newEntity with
            {
                UserIds = newUserIds
            };
        }

        protected override GroupV1 RemovePartialEntity(GroupV1 entity, GroupV1 payload, bool getPayload)
        {
            var (_, _, _, _, UserIds, _) = payload;

            var newUserIds = getPayload
                    ? entity.UserIds?.Intersect(UserIds).ToList()
                    : entity.UserIds?.Except(UserIds).ToList();

            // new group to return null properties for json serialization where we ignore null properties
            var newEntity = getPayload ? new GroupV1() : entity;

            return newEntity with
            {
                UserIds = newUserIds
            };
        }

    }
}