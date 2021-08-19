using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1
{
    public interface ISaveRelationship
    {
        /// <summary>
        /// FromElementDcv
        /// </summary>
        string FromElementDcv { get; set; }
        /// <summary>
        /// ToElementDcv
        /// </summary>
        string ToElementDcv { get; set; }
        /// <summary>
        /// RelationType
        /// </summary>
        RelationshipType RelationshipType { get; set; }
    }
}
