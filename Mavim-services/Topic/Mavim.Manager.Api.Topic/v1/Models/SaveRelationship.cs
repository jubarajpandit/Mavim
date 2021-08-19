using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// SaveRelationship
    /// </summary>
    public class SaveRelationship : ISaveRelationship
    {
        /// <summary>
        /// FromElementDcv
        /// </summary>
        public string FromElementDcv { get; set; }
        /// <summary>
        /// ToElementDcv
        /// </summary>
        public string ToElementDcv { get; set; }
        /// <summary>
        /// RelationType
        /// </summary>
        public RelationshipType RelationshipType { get; set; } // RELBuffer.MvmSrv_RELtpe
    }
}
