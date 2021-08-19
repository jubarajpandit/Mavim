using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface
{
    public interface ISaveRelation
    {
        string TopicId { get; set; }
        string RelationId { get; set; }
        Action Action { get; set; }
        string OldCategory { get; set; }
        string OldTopicId { get; set; }
        string Category { get; set; }
        string ToTopicId { get; set; }
    }
}
