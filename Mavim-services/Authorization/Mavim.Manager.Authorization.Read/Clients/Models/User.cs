using System;

namespace Mavim.Manager.Authorization.Read.Clients.Models
{
    public record User(Guid Id, Guid CompanyId, Guid[] Groups);

}
