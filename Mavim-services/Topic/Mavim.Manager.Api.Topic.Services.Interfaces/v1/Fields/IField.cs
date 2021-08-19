using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface IField
    {
        /// <summary>
        /// FieldSetDefinitionID
        /// </summary>
        string FieldSetId { get; set; }
        /// <summary>
        /// FieldDefinitionID
        /// </summary>
        string FieldId { get; set; }
        /// <summary>
        /// Order of the fieldset in the fields in the topic
        /// </summary>
        int SetOrder { get; set; }
        /// <summary>
        /// Order of the field in the fieldset
        /// </summary>
        int Order { get; set; }
        /// <summary>
        /// SetName
        /// </summary>
        string SetName { get; set; }
        /// <summary>
        /// FieldName
        /// </summary>
        string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the DCV identifier.
        /// </summary>
        /// <value>
        /// The DCV identifier.
        /// </value>
        string TopicId { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        FieldType FieldValueType { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="IField"/> is required.</summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.</value>
        bool Required { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="IField"/> is readonly.</summary>
        /// <value>
        ///   <c>true</c> if readonly; otherwise, <c>false</c>.</value>
        bool Readonly { get; set; }

        /// <summary>Gets or sets the usage.</summary>
        /// <value>The usage.</value>
        string Usage { get; set; }

        /// <summary>Gets or sets the relationship category.</summary>
        /// <value>The relationship category.</value>
        IRelationshipElement RelationshipCategory { get; set; }

        /// <summary>Gets or sets the characteristics.</summary>
        /// <value>The characteristics.</value>
        IRelationshipElement Characteristic { get; set; }

        /// <summary>
        /// Gets or sets the open location.
        /// </summary>
        /// <value>
        /// The open location.
        /// </value>
        string OpenLocation { get; set; }
    }
}
