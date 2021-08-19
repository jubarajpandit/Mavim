using Mavim.Manager.ChLog.Relationship.DbModel.Enums;
using System;
using Action = Mavim.Manager.ChLog.Relationship.DbModel.Enums.Action;

namespace Mavim.Manager.ChLog.Relationship.DbModel
{
    public class Relation
    {
        public Guid ChangelogId { get; set; }
        public Guid TenantId { get; set; }
        public Guid DatabaseId { get; set; }
        public DataLanguageType DataLanguage { get; set; }
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
