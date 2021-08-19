using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Authorization.Read.Versions.V1.DTO
{
    /// <summary>
    /// Patch DTO for disable/enable Role
    /// </summary>
    public record DisabledRole(bool Disabled, Guid CompanyId, int AggregateId) : Meta(CompanyId, AggregateId)
    {
        /// <summary>
        /// Disabled
        /// </summary>
        [Required]
        public bool Disabled { get; init; } = Disabled;
    }
}
