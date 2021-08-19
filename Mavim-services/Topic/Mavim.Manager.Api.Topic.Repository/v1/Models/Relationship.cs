using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.v1.Models
{
    public class Relationship : IRelationship
    {
        /// <summary>
        /// DcvId
        /// </summary>
        public string Dcv { get; set; }
        /// <summary>
        /// RelationCategory
        /// </summary>
        public string Category { get; set; }
        /// <summary>Gets or sets the type of the relationship type.</summary>
        /// <value>The type of the category.</value>
        public RelationshipType RelationshipType { get; set; }
        /// <summary>Gets or sets the type of the category.</summary>
        /// <value>The type of the category.</value>
        public CategoryType CategoryType { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// UserInstruction
        /// </summary>
        public IRelationshipElement UserInstruction { get; set; }
        /// <summary>
        /// DispatchInstructions
        /// </summary>
        public IEnumerable<ISimpleDispatchInstruction> DispatchInstructions { get; set; }
        /// <summary>
        /// Characteristic
        /// </summary>
        public IRelationshipElement Characteristic { get; set; }
        /// <summary>
        /// WithElement
        /// </summary>
        public IRelationshipElement WithElement { get; set; }
        /// <summary>
        /// WithElementParent
        /// </summary>
        public IRelationshipElement WithElementParent { get; set; }
        /// <summary>
        /// IsAttributeRelation
        /// </summary>
        public bool IsAttributeRelation { get; set; }
        /// <summary>
        /// IsDestroyed
        /// </summary>
        public bool IsDestroyed { get; set; }
    }
}
