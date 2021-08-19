using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Authorization.Read.Databases.Models
{
    public record Role(
        Guid Id,
        int AggregateId,
        string Name,
        Guid[] Groups,
        Guid[] TopicPermissions,
        Guid CompanyId,
        bool Disabled,
        DateTime LastUpdated
        ) : ReadModelBase(Id, AggregateId, CompanyId, Disabled, LastUpdated)
    {
        [Required]
        public string Name { get; init; } = Name;
        [Required]
        public Guid[] Groups { get; init; } = Groups;
        [Required]
        public Guid[] TopicPermissions { get; init; } = TopicPermissions;
    }

}