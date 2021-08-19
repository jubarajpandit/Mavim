using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mavim.Manager.Api.Authorization.Read.Validators;

namespace Mavim.Manager.Api.Authorization.Read.Versions.V1.DTO
{
    /// <summary>
    /// Update Role
    /// </summary>
    public record UpdateRole(string Name, Guid CompanyId, int AggregateId) : Meta(CompanyId, AggregateId)
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; init; } = Name;
    }
}
