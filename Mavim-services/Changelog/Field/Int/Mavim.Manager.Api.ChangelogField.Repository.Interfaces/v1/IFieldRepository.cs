using Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogField.Repository.Interfaces.v1
{
    public interface IFieldRepository
    {
        Task<IChangelogField> GetField(Guid changelogId, Guid tenantId);
        Task<IEnumerable<IChangelogField>> GetFieldsByTopic(Guid tenantId, Guid dbId, DataLanguageType language, string topicId);
        Task<IEnumerable<IChangelogField>> GetFieldsByTopicAndStatus(Guid tenantId, Guid dbId, DataLanguageType language, string topicId, ChangeStatus status);
        Task<IEnumerable<IChangelogField>> GetFields(Guid tenantId, Guid dbId, DataLanguageType language);
        Task<IEnumerable<IChangelogField>> GetFieldsByStatus(Guid tenantId, Guid dbId, DataLanguageType language, ChangeStatus status);
        Task SaveField(ISaveFieldChange field);
        Task<IChangelogField> UpdateFieldStatus(Guid changelogId, Guid tenantId, string reviewerEmail, ChangeStatus status);
    }
}
