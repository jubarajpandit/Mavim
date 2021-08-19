using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1.Interfaces;
using System;

namespace Mavim.Manager.Api.ChangelogTitle.Public.Services.v1.Models
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
