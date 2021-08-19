using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1
{
    public interface IRelationship
    {
        /// <summary>
        /// DcvId
        /// </summary>
        string Dcv { get; set; }
        /// <summary>Gets or sets the if it's a type of topic.</summary>
        /// <value>Boolean if it's a type of topic.</value>
        public bool IsTypeOfTopic { get; set; }
        /// <summary>
        /// RelationCategory
        /// </summary>
        string Category { get; set; }
        /// <summary>Gets or sets the type of the category type.</summary>
        /// <value>The type of the category.</value>
        CategoryType CategoryType { get; set; }
        /// <summary>Gets or sets the type of the relationship type.</summary>
        /// <value>The type of the category.</value>
        RelationshipType RelationshipType { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        string Icon { get; set; }
        /// <summary>
        /// UserInstruction
        /// </summary>
        IRelationshipElement UserInstruction { get; set; }
        /// <summary>
        /// DispatchInstructions
        /// </summary>
        IEnumerable<ISimpleDispatchInstruction> DispatchInstructions { get; set; }
        /// <summary>
        /// Characteristic
        /// </summary>
        IRelationshipElement Characteristic { get; set; }
        /// <summary>
        /// WithElement
        /// </summary>
        IRelationshipElement WithElement { get; set; }
        /// <summary>
        /// WithElementParent
        /// </summary>
        IRelationshipElement WithElementParent { get; set; }
    }
}
