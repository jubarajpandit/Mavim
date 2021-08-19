using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Interface;
using System;

namespace Mavim.Manager.Api.ChangelogTitle.Repository.v1.Model
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
