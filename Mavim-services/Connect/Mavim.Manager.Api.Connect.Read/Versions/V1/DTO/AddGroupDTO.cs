using Mavim.Manager.Api.Connect.Read.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Connect.Read.Versions.V1.DTO
{
    /// <summary>
    /// Add User DTO
    /// </summary>
    public record AddGroupDTO(Guid Id, string Name, string Description, Guid CompanyId, int ModelVersion, int AggregateId) : MetaDTO(ModelVersion, AggregateId)
    {
        /// <summary>
        /// Group Id
        /// </summary>
        [RequiredGuid]
        public Guid Id { get; init; } = Id;

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; init; } = Name;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; init; } = Description;

        /// <summary>
        /// Company Id
        /// </summary>
        [RequiredGuid]
        public Guid CompanyId { get; init; } = CompanyId;
    };
}