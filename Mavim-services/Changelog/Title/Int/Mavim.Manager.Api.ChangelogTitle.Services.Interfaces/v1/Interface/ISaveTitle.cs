using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Enum;
using System;

namespace Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Interface
{
    public interface ISaveTitle
    {
        Guid TenantId { get; set; }
        Guid DatabaseId { get; set; }
        string InitiatorUserEmail { get; set; }
        DateTime TimestampChanged { get; set; }
        string TopicDcv { get; set; }
        ChangeStatus Status { get; set; }
        string FromTitleValue { get; set; }
        string ToTitleValue { get; set; }
    }
}
