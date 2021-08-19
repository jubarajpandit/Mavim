using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;
using System;
using Action = Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum.Action;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface
{
    public interface IRelation
    {
        Guid ChangelogId { get; set; }
        string InitiatorUserEmail { get; set; }
        string ReviewerUserEmail { get; set; }
        DateTime TimestampChanged { get; set; }
        DateTime? TimestampApproved { get; set; }
        string TopicId { get; set; }
        Action Action { get; set; }
        ChangeStatus Status { get; set; }
        string RelationId { get; set; }
        string OldCategory { get; set; }
        string OldTopicId { get; set; }
        string Category { get; set; }
        string ToTopicId { get; set; }
    }
}