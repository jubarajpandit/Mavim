using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface;
using System;
using Action = Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum.Action;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1.Model
{
    public class Relation : IRelation
    {
        public Guid ChangelogId { get; set; }
        public string InitiatorUserEmail { get; set; }
        public string ReviewerUserEmail { get; set; }
        public DateTime TimestampChanged { get; set; }
        public DateTime? TimestampApproved { get; set; }
        public string TopicId { get; set; }
        public Action Action { get; set; }
        public ChangeStatus Status { get; set; }
        public string RelationId { get; set; }
        public string OldCategory { get; set; }
        public string OldTopicId { get; set; }
        public string Category { get; set; }
        public string ToTopicId { get; set; }
    }
}
