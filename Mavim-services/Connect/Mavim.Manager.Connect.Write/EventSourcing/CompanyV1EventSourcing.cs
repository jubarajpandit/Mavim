using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using System;

namespace Mavim.Manager.Connect.Write.EventSourcing
{
    public class CompanyV1EventSourcing : EventSourcingGeneric<CompanyV1>, IEventSourcingGeneric<CompanyV1>
    {
        public CompanyV1EventSourcing(ConnectDbContext dbContext) : base(dbContext, EntityType.Company)
        {
        }

        protected override CompanyV1 Update(CompanyV1 entity, CompanyV1 payload, CompanyV1 newPayload)
        {
            var (_, Name, Domain, Tenant, _) = payload;

            if (entity.Name != Name && !string.IsNullOrWhiteSpace(Name))
                newPayload = newPayload with { Name = Name };

            if (entity.Domain != Domain && !string.IsNullOrWhiteSpace(Domain))
                newPayload = newPayload with { Domain = Domain };

            if (Tenant is not null && entity.TenantId != Tenant && Tenant != Guid.Empty)
                newPayload = newPayload with { TenantId = Tenant };

            return newPayload;
        }
    }
}