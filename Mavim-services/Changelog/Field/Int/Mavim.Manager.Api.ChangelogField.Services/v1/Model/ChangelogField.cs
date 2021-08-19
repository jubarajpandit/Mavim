using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1.Enum;
using System;

namespace Mavim.Manager.Api.ChangelogField.Services.v1.Model
{
    public class ChangelogField : IChangelogField
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid DatabaseId { get; set; }
        public DataLanguageType DataLanguage { get; set; }
        public string InitiatorEmail { get; set; }
        public string ReviewerEmail { get; set; }
        public DateTime TimestampChanged { get; set; }
        public DateTime? TimestampReviewed { get; set; }
        public string TopicId { get; set; }
        public ChangeStatus Status { get; set; }
        public string FieldSetId { get; set; }
        public string FieldId { get; set; }
        public FieldType Type { get; set; }
        public string OldFieldValue { get; set; }
        public string NewFieldValue { get; set; }
    }
}
