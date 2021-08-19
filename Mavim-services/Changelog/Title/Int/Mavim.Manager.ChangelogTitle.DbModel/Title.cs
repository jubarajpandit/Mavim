using Mavim.Manager.ChangelogTitle.DbModel.Enums;
using Mavim.Manager.ChangelogTitle.DbModel.Interfaces;
using System;

namespace Mavim.Manager.ChangelogTitle.DbModel
{
    public class Title : ITitle
    {
        public int ChangelogId { get; set; }
        public Guid TenantId { get; set; }
        public Guid DatabaseId { get; set; }
        public DataLanguageType DataLanguage { get; set; }
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
