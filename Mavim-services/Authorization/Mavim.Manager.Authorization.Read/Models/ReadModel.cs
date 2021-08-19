using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Authorization.Read.Models
{
    public abstract record ReadModel(Guid Id, int AggregateId, Guid CompanyId, bool Disabled, DateTime LastUpdated)
    {
        [Required]
        public Guid Id { get; init; } = Id;

        [Required]
        public int AggregateId { get; init; } = AggregateId;

        [Required]
        public Guid CompanyId { get; init; } = CompanyId;

        [Required]
        public bool Disabled { get; init; } = Disabled;

        [Required]
        public DateTime LastUpdated { get; init; } = LastUpdated;
    }
}
