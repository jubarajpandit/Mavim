using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Enums;
using System;

namespace Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Interfaces
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
