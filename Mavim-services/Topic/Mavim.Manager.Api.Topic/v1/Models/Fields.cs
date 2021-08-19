using System.Collections.Generic;
using Service = Mavim.Manager.Api.Topic.Services.v1.Models.Fields;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// Fields
    /// </summary>
    public class Fields
    {
        /// <summary>
        /// SingleTextFields
        /// </summary>
        public List<Service.SingleTextField> SingleTextFields { get; set; }
        /// <summary>
        /// MultiTextFields
        /// </summary>
        public List<Service.MultiTextField> MultiTextFields { get; set; }
        /// <summary>
        /// SingleNumberFields
        /// </summary>
        public List<Service.SingleNumberField> SingleNumberFields { get; set; }
        /// <summary>
        /// MultiNumberFields
        /// </summary>
        public List<Service.MultiNumberField> MultiNumberFields { get; set; }
        /// <summary>
        /// SingleBooleanFields
        /// </summary>
        public List<Service.SingleBooleanField> SingleBooleanFields { get; set; }
        /// <summary>
        /// SingleDecimalFields
        /// </summary>
        public List<Service.SingleDecimalField> SingleDecimalFields { get; set; }
        /// <summary>
        /// MultiDecimalFields
        /// </summary>
        public List<Service.MultiDecimalField> MultiDecimalFields { get; set; }
        /// <summary>
        /// SingleDateFields
        /// </summary>
        public List<Service.SingleDateField> SingleDateFields { get; set; }
        /// <summary>
        /// MultiDateFields
        /// </summary>
        public List<Service.MultiDateField> MultiDateFields { get; set; }
        /// <summary>
        /// SingleListFields
        /// </summary>
        public List<Service.SingleListField> SingleListFields { get; set; }
        /// <summary>
        /// SingleRelationshipFields
        /// </summary>
        public List<RelationshipField> SingleRelationshipFields { get; set; }
        /// <summary>
        /// MultiRelationshipFields
        /// </summary>
        public List<MultiRelationshipField> MultiRelationshipFields { get; set; }
        /// <summary>
        /// SingleRelationshipListFields
        /// </summary>
        public List<RelationshipListField> SingleRelationshipListFields { get; set; }
        /// <summary>
        /// SingleHyperlinkFields
        /// </summary>
        public List<Service.SingleHyperlinkField> SingleHyperlinkFields { get; set; }
        /// <summary>
        /// MultiHyperlinkFields
        /// </summary>
        public List<Service.MultiHyperlinkField> MultiHyperlinkFields { get; set; }
    }
}