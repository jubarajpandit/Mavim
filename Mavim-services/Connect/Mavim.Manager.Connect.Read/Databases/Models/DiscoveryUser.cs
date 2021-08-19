using Mavim.Manager.Connect.Read.Databases.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Connect.Read.Databases.Models
{
    public record DiscoveryUser(Guid Id, string Email, Guid TenantId, bool Disabled, DateTime LastUpdated) : IDiscoveryUser
    {
        [Required]
        public Guid Id { get; init; } = Id;

        [Required]
        public string Email { get; init; } = Email;

        [Required]
        public Guid TenantId { get; init; } = TenantId;

        [Required]
        public bool Disabled { get; init; } = Disabled;

        [Required]
        public DateTime LastUpdated { get; init; } = LastUpdated;

        public DiscoveryUser() : this(Guid.Empty, null, Guid.Empty, false, DateTime.Now) { }
    }
}
