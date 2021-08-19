using Mavim.Manager.Api.Topic.Services.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class TopicBusiness : ITopicBusiness
    {
        /// <summary>
        /// Gets or sets a value indicating whether a topic can be changed
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value cannot be changed.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// CanDelete
        /// </summary>
        ///  /// <value>
        ///   <c>true</c> if the topic cannot be deleted.
        /// </value>
        public bool CanDelete { get; set; }

        /// <summary>
        /// CanCreateChildTopic
        /// </summary>
        /// ///  /// <value>
        ///   <c>true</c> if a child topic can be created.
        /// </value>
        public bool CanCreateChildTopic { get; set; }

        /// <summary>
        /// CanCreateTopicAfter
        /// </summary>
        /// ///  /// <value>
        ///   <c>true</c> if a topic after can be created.
        /// </value>
        public bool CanCreateTopicAfter { get; set; }
    }
}
