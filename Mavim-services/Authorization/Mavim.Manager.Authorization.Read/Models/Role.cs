using System;
using System.Collections.Generic;

namespace Mavim.Manager.Authorization.Read.Models
{
    public record Role(
        Guid Id,
        string Name,
        IReadOnlyList<Guid> Groups,
        IReadOnlyList<Guid> TopicPermissions
        )
    {
        public Guid Id { get; init; } = Id;
        public string Name { get; init; } = Name;
        public IReadOnlyList<Guid> Groups { get; init; } = Groups;
        public IReadOnlyList<Guid> TopicPermissions { get; init; } = TopicPermissions;

    };
}
