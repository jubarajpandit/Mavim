using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields.Base
{
    public abstract class Field : IField
    {
        /// <summary>
        /// FieldSetDefinitionID
        /// </summary>
        public string FieldSetId { get; set; }

        /// <summary>
        /// FieldDefinitionID
        /// </summary>
        public string FieldId { get; set; }
        /// <summary>
        /// Order of the fieldset in the fields in the topic
        /// </summary>
        public int SetOrder { get; set; }
        /// <summary>
        /// Order of the field in the fieldset
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// SetName
        /// </summary>
        public string SetName { get; set; }

        /// <summary>
        /// FieldName
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// FieldValueType
        /// </summary>
        public FieldType FieldValueType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Mavim.Manager.Api.Topic.Interfaces.Models.v1.IField" /> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Mavim.Manager.Api.Topic.Interfaces.Models.v1.IField" /> is readonly.
        /// </summary>
        /// <value>
        ///   <c>true</c> if readonly; otherwise, <c>false</c>.
        /// </value>
        public bool Readonly { get; set; }

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public string Usage { get; set; }

        /// <summary>
        /// Gets or sets the relationship category.
        /// </summary>
        /// <value>
        /// The relationship category.
        /// </value>
        public IRelationshipElement RelationshipCategory { get; set; }

        /// <summary>
        /// Gets or sets the characteristics.
        /// </summary>
        /// <value>
        /// The characteristics.
        /// </value>
        public IRelationshipElement Characteristic { get; set; }

        /// <summary>
        /// Gets or sets the open location.
        /// </summary>
        /// <value>
        /// The open location.
        /// </value>
        public string OpenLocation { get; set; }
    }
}
