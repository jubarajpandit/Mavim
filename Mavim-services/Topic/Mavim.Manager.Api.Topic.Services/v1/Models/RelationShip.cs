using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class Relationship : IRelationship
    {
        /// <summary>
        /// FieldId
        /// </summary>
        public string Dcv { get; set; }
        /// <summary>Gets or sets the if it's a type of topic.</summary>
        /// <value>Boolean if it's a type of topic.</value>
        public bool IsTypeOfTopic { get; set; }
        /// <summary>
        /// RelationCategory
        /// </summary>
        public string Category { get; set; }
        /// <summary>Gets or sets the type of the category.</summary>
        /// <value>The type of the category.</value>
        public CategoryType CategoryType { get; set; }
        /// <summary>Gets or sets the type of the relation.</summary>
        /// <value>The type of the category.</value>
        public RelationshipType RelationshipType { get; set; }
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
    }
}
