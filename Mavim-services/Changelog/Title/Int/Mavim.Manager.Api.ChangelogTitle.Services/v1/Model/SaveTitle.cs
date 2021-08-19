using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Services.Interfaces.v1.Interface;
using System;

namespace Mavim.Manager.Api.ChangelogTitle.Services.v1.Model
{
    public class SaveTitle : ISaveTitle
    {
        public Guid TenantId { get; set; }
        public Guid DatabaseId { get; set; }
        public string InitiatorUserEmail { get; set; }
        public DateTime TimestampChanged { get; set; }
        public string TopicDcv { get; set; }
        public ChangeStatus Status { get; set; }
        public string FromTitleValue { get; set; }
        public string ToTitleValue { get; set; }
    }
}
