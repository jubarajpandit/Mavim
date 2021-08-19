using Mavim.Libraries.Changelog.Enums;
using Mavim.Libraries.Changelog.Interfaces;
using System;

namespace Mavim.Libraries.Changelog.Models
{
    public class ChangelogTitle : IChangelogTitle
    {
        public int ChangelogId { get; set; }
        public string InitiatorUserEmail { get; set; }
        public string ReviewerUserEmail { get; set; }
        public DateTime TimestampChanged { get; set; }
        public DateTime? TimestampApproved { get; set; }
        public string TopicDcv { get; set; }
        public ChangeStatus Status { get; set; }
        public string FromTitleValue { get; set; }
        public string ToTitleValue { get; set; }
    }
}
