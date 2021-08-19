using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1
{
    public interface IChangelogRelationshipPublicService
    {
        Task<IEnumerable<IChangelogRelationship>> GetRelations(Guid dbId, string topicId);
        Task<IEnumerable<IChangelogRelationship>> GetPendingRelations(Guid dbId, string topicId);
        Task<IEnumerable<IChangelogRelationship>> GetAllPendingRelations(Guid dbId);
        Task<ChangeStatus> GetRelationStatus(Guid dbId, string topicId);
        Task<IChangelogRelationship> ApproveRelation(Guid dbId, Guid changelogId);
        Task<IChangelogRelationship> RejectRelation(Guid dbId, Guid changelogId);
    }
}
