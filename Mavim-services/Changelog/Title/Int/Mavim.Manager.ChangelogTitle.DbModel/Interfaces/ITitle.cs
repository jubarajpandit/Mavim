using Mavim.Manager.ChangelogTitle.DbModel.Enums;
using System;

namespace Mavim.Manager.ChangelogTitle.DbModel.Interfaces
{
    public interface ITitle
    {
        int ChangelogId { get; set; }
        Guid TenantId { get; set; }
        Guid DatabaseId { get; set; }
        DataLanguageType DataLanguage { get; set; }
        string InitiatorUserEmail { get; set; }
        string ReviewerUserEmail { get; set; }
        DateTime TimestampChanged { get; set; }
        DateTime? TimestampApproved { get; set; }
        string TopicDcv { get; set; }
        ChangeStatus Status { get; set; }
        string FromTitleValue { get; set; }
        string ToTitleValue { get; set; }
    }
}