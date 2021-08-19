using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// RelationshipListField
    /// </summary>
    public class RelationshipListField : Field
    {
        /// <summary>
        /// Data
        /// </summary>
        public Dictionary<string, RelationshipElement> Data { get; set; }
    }
}