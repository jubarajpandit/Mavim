using Mavim.Manager.Api.Connect.Read.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Connect.Read.Versions.V1.DTO
{
    /// <summary>
    /// Add User DTO
    /// </summary>
    public record AddUserDTO(Guid Id, string Email, Guid CompanyId, int ModelVersion, int AggregateId) : MetaDTO(ModelVersion, AggregateId)
    {
        /// <summary>
        /// User Id
        /// </summary>
        [RequiredGuid]
        public Guid Id { get; init; } = Id;

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; init; } = Email;

        /// <summary>
        /// Company Id
        /// </summary>
        [RequiredGuid]
        public Guid CompanyId { get; init; } = CompanyId;
    };
}