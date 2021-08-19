using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips
{
    public interface IRelationship
    {
        /// <summary>
        /// DcvId
        /// </summary>
        string Dcv { get; set; }
        /// <summary>
        /// RelationCategory
        /// </summary>
        string Category { get; set; }
        /// <summary>
        /// RelationType
        /// </summary>
        RelationshipType RelationshipType { get; set; }
        /// <summary>
        /// CategoryType
        /// </summary>
        CategoryType CategoryType { get; set; }
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
        /// <summary>
        /// IsAttributeRelation
        /// </summary>
        bool IsAttributeRelation { get; set; }
        /// <summary>
        /// IsDestroyed
        /// </summary>
        bool IsDestroyed { get; set; }
    }
}
