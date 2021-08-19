using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// MultiRelationshipField
    /// </summary>
    public class MultiRelationshipField : Field
    {
        /// <summary>
        /// Data
        /// </summary>
        public List<RelationshipElement> Data { get; set; }
    }
}