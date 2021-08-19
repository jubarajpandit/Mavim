using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1.Enum;
using System;

namespace Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1
{
    public interface ISaveFieldChange
    {
        Guid TenantId { get; set; }
        Guid DatabaseId { get; set; }
        DataLanguageType DataLanguage { get; set; }
        string InitiatorEmail { get; set; }
        DateTime TimestampChanged { get; set; }
        string TopicId { get; set; }
        ChangeStatus Status { get; set; }
        string FieldSetId { get; set; }
        string FieldId { get; set; }
        FieldType Type { get; set; }
        string OldFieldValue { get; set; }
        string NewFieldValue { get; set; }
    }
}
