using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Connect.Read.Databases.Models
{
    public abstract record ReadModel(Guid Id, string Value, int ModelVersion, int AggregateId, Guid CompanyId, bool Disabled, DateTime LastUpdated)
    {
        [Required]
        public Guid Id { get; init; } = Id;

        [Required]
        public string Value { get; init; } = Value;

        [Required]
        public int ModelVersion { get; init; } = ModelVersion;

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
