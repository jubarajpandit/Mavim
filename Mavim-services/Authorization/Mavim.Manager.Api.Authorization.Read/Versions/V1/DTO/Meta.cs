using System;
using System.ComponentModel.DataAnnotations;
using Mavim.Manager.Api.Authorization.Read.Validators;

namespace Mavim.Manager.Api.Authorization.Read.Versions.V1.DTO
{
    /// <summary>
    /// Meta of Roles
    /// </summary>
    public record Meta(Guid CompanyId, int AggregateId)
    {
        /// <summary>
        /// CompanyId
        /// </summary>
        [RequiredGuid]
        public Guid CompanyId { get; init; } = CompanyId;

        /// <summary>
        /// AggregateId
        /// </summary>
        [Required]
        public int AggregateId { get; init; } = AggregateId;
    }
}
