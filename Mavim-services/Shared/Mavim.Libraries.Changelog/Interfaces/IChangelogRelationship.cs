using Mavim.Libraries.Changelog.Enums;
using System;

namespace Mavim.Libraries.Changelog.Interfaces
{
    public interface IChangelogRelationship
    {
        Guid ChangelogId { get; set; }
        string InitiatorUserEmail { get; set; }
        string ReviewerUserEmail { get; set; }
        DateTime TimestampChanged { get; set; }
        DateTime? TimestampApproved { get; set; }
        string TopicDcv { get; set; }
        ChangeStatus Status { get; set; }
        string RelationDcv { get; set; }
        string FromCategory { get; set; }
        string FromTopicDcv { get; set; }
        string ToCategory { get; set; }
        string ToTopicDcv { get; set; }
    }
}
