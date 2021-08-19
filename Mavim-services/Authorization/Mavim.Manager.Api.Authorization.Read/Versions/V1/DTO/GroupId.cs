using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mavim.Manager.Api.Authorization.Read.Validators;

namespace Mavim.Manager.Api.Authorization.Read.Versions.V1.DTO
{
    /// <summary>
    /// RoleGroups
    /// </summary>
    public record RoleGroups(IReadOnlyList<Guid> Ids, Guid CompanyId, int AggregateId) : Meta(CompanyId, AggregateId)
    {
        /// <summary>
        /// Ids
        /// </summary>
        [RequiredGuid]
        public IReadOnlyList<Guid> Ids { get; init; } = Ids;
    }
}
