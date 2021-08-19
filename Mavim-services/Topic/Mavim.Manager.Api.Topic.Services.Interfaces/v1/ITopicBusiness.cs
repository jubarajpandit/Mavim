namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1
{
    public interface ITopicBusiness
    {
        /// <summary>
        /// Gets or sets a value indicating whether a topic can be changed
        /// </summary>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// CanDelete
        /// </summary>
        bool CanDelete { get; set; }

        /// <summary>
        /// CanCreateChildTopic
        /// </summary>
        bool CanCreateChildTopic { get; set; }

        /// <summary>
        /// CanCreateTopicAfter
        /// </summary>
        bool CanCreateTopicAfter { get; set; }
    }
}
