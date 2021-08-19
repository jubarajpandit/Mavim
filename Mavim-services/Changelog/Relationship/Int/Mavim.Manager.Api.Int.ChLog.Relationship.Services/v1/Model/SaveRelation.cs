using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1.Model
{
    public class SaveRelation : ISaveRelation
    {
        public string TopicId { get; set; }
        public string RelationId { get; set; }
        public Action Action { get; set; }
        public string OldCategory { get; set; }
        public string OldTopicId { get; set; }
        public string Category { get; set; }
        public string ToTopicId { get; set; }
    }
}
