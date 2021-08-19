using Mavim.Libraries.Changelog.Enums;
using Mavim.Libraries.Changelog.Interfaces;
using System;

namespace Mavim.Libraries.Changelog.Models
{
    public class ChangelogRelationshipResponse : IChangelogRelationship
    {
        public Guid ChangelogId { get; set; }
        public string InitiatorUserEmail { get; set; }
        public string ReviewerUserEmail { get; set; }
        public DateTime TimestampChanged { get; set; }
        public DateTime? TimestampApproved { get; set; }
        public string TopicDcv { get; set; }
        public ChangeStatus Status { get; set; }
        public string RelationDcv { get; set; }
        public string FromCategory { get; set; }
        public string FromTopicDcv { get; set; }
        public string ToCategory { get; set; }
        public string ToTopicDcv { get; set; }
    }
}
