using System;

namespace Mavim.Manager.Connect.Write.Identity
{
    public record IdentityService(Guid UserId, Guid GroupId, Guid CompanyId) : IIdentityService;

    public interface IIdentityService
    {
        Guid UserId { get; init; }
        Guid GroupId { get; init; }
        Guid CompanyId { get; init; }
    }
}