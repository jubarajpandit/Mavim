using Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1.Enum;
using System;

namespace Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1
{
    public interface IChangelogField
    {
        Guid Id { get; set; }
        Guid TenantId { get; set; }
        Guid DatabaseId { get; set; }
        DataLanguageType DataLanguage { get; set; }
        string InitiatorEmail { get; set; }
        string ReviewerEmail { get; set; }
        DateTime TimestampChanged { get; set; }
        DateTime? TimestampReviewed { get; set; }
        string TopicId { get; set; }
        ChangeStatus Status { get; set; }
        string FieldSetId { get; set; }
        string FieldId { get; set; }
        FieldType Type { get; set; }
        string OldFieldValue { get; set; }
        string NewFieldValue { get; set; }
    }
}