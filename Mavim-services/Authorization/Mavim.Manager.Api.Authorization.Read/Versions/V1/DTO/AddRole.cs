using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mavim.Manager.Api.Authorization.Read.Validators;

namespace Mavim.Manager.Api.Authorization.Read.Versions.V1.DTO
{
    /// <summary>
    /// Add role
    /// </summary>
    public record AddRole(Guid Id, string Name, IReadOnlyList<Guid> Groups, IReadOnlyList<Guid> TopicPermissions, Guid CompanyId, int AggregateId) : Meta(CompanyId, AggregateId)
    {
        /// <summary>
        /// Id
        /// </summary>
        [RequiredGuid]
        public Guid Id { get; init; } = Id;
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; init; } = Name;
        /// <summary>
        /// Group ids
        /// </summary>
        [RequiredGuid]
        public IReadOnlyList<Guid> Groups { get; init; } = Groups;
        /// <summary>
        /// TopicPermission ids
        /// </summary>
        [RequiredGuid]
        public IReadOnlyList<Guid> TopicPermissions { get; init; } = TopicPermissions;
    }
}
