using System;

namespace Mavim.Manager.Authorization.Read.Clients.Models
{
    public record ConnectMeResponse(Guid id, string email, Guid companyId, Guid[] groups);
}
