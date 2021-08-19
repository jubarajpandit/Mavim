using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1
{
    public interface IRelationRepository
    {
        Task<IRelation> GetRelationById(Guid tenantId, Guid changelogId);
        Task<IEnumerable<IRelation>> GetRelationsByTopic(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, string topicId);
        Task<IEnumerable<IRelation>> GetAllRelationsByStatus(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, ChangeStatus status);
        Task<IEnumerable<IRelation>> GetAllTopicRelationsByStatus(Guid tenantId, Guid dbId, DataLanguageType dataLanguage, string topicId, ChangeStatus status);
        Task<IRelation> SaveRelation(IRelation relation);
        Task UpdateRelationStatus(Guid tenantId, Guid changelogId, string reviewerUserEmail, DateTime approvedTime, ChangeStatus status);
    }
}
