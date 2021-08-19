using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1
{
    public interface IRelationService
    {
        Task<IEnumerable<IRelation>> GetRelationsByTopic(Guid dbId, string topicId);
        Task<IEnumerable<IRelation>> GetPendingRelationsByTopic(Guid dbId, string topicId);
        Task<IEnumerable<IRelation>> GetAllPendingRelations(Guid dbId);
        Task<ChangeStatus> GetRelationStatus(Guid dbId, string topicId);
        Task<IRelation> SaveRelation(Guid dbId, ISaveRelation relation);
        Task UpdateRelationStatus(Guid dbId, Guid changelogId, ChangeStatus status);
    }
}
