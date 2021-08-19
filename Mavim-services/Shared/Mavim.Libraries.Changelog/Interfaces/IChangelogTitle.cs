using Mavim.Libraries.Changelog.Enums;
using System;

namespace Mavim.Libraries.Changelog.Interfaces
{
    public interface IChangelogTitle
    {
        int ChangelogId { get; set; }
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
