using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.Business.v1.Models
{
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
