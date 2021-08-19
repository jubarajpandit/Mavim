using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1
{
    public interface IFieldService
    {
        Task<IEnumerable<IChangelogField>> GetFields(Guid dbId, DataLanguageType dataLanguage, string topicId);
        Task<IEnumerable<IChangelogField>> GetPendingFieldsByTopic(Guid dbId, DataLanguageType dataLanguage, string topicId);
        Task<IEnumerable<IChangelogField>> GetPendingFields(Guid dbId, DataLanguageType dataLanguage);
        Task<ChangeStatus> GetTopicStatusByFields(Guid dbId, DataLanguageType dataLanguage, string topicId);
        Task<IChangelogField> ApproveField(Guid dbId, DataLanguageType dataLanguage, Guid changelogId);
        Task<IChangelogField> RejectField(Guid dbId, DataLanguageType dataLanguage, Guid changelogId);
        Task SaveField(ISaveFieldChange field);
    }
}
