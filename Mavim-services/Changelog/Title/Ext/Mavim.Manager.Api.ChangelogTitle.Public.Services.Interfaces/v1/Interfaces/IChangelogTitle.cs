using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Enums;
using System;

namespace Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Interfaces
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
