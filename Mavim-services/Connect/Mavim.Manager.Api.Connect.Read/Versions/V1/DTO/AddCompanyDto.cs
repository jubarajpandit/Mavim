using Mavim.Manager.Api.Connect.Read.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Connect.Read.Versions.V1.DTO
{
    /// <summary>
    /// Add Company DTO
    /// </summary>
    public record AddCompanyDTO(Guid Id, string Name, string Domain, Guid TenantId, int ModelVersion, int AggregateId) : MetaDTO(ModelVersion, AggregateId)
    {
        /// <summary>
        /// Company Id
        /// </summary>
        [RequiredGuid]
        public Guid Id { get; init; } = Id;

        /// <summary>
        /// Company Name
        /// </summary>
        [Required]
        public string Name { get; init; } = Name;

        /// <summary>
        /// Company Domain
        /// </summary>
        [Required]
        public string Domain { get; init; } = Domain;

        /// <summary>
        /// Company TenantId
        /// </summary>
        [RequiredGuid]
        public Guid TenantId { get; init; } = TenantId;
    };
}