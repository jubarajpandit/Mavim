using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;

namespace Mavim.Manager.Connect.Write.EventSourcing
{
    public class UserV1EventSourcing : EventSourcingGeneric<UserV1>, IEventSourcingGeneric<UserV1>
    {
        public UserV1EventSourcing(ConnectDbContext dbContext) : base(dbContext, EntityType.User)
        {
        }

        protected override UserV1 Update(UserV1 entity, UserV1 payload, UserV1 newPayload)
        {
            var (_, Email, _, _) = payload;

            if (entity.Email != Email && !string.IsNullOrWhiteSpace(Email))
                newPayload = newPayload with { Email = Email };

            return newPayload;
        }
    }
}