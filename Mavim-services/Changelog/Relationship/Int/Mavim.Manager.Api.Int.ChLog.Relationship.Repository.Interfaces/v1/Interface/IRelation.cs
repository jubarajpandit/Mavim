using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Enum;
using System;
using Action = Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Enum.Action;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Interface
{
    public interface IRelation
    {
        Guid ChangelogId { get; set; }
        Guid TenantId { get; set; }
        Guid DatabaseId { get; set; }
        DataLanguageType DataLanguage { get; set; }
        string InitiatorUserEmail { get; set; }
        string ReviewerUserEmail { get; set; }
        DateTime TimestampChanged { get; set; }
        DateTime? TimestampApproved { get; set; }
        string TopicId { get; set; }
        string RelationId { get; set; }
        Action Action { get; set; }
        ChangeStatus Status { get; set; }
        string OldCategory { get; set; }
        string OldTopicId { get; set; }
        string Category { get; set; }
        string ToTopicId { get; set; }
    }
}