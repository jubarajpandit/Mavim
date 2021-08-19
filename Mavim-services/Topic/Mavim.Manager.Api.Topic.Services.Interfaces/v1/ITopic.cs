using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1
{
    public interface ITopic
    {
        /// <summary>
        /// Combined ids of the Dcv in string format
        /// </summary>
        string Dcv { get; set; }

        /// <summary>Gets or sets the parent DCV.</summary>
        /// <value>The parent DCV.</value>
        string Parent { get; set; }

        /// <summary>Indicates if this topic has children</summary>
        /// <value><c>true</c> if this topic has children.</value>
        bool HasChildren { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// TypeCategory
        /// </summary>
        TopicType TypeCategory { get; set; }

        /// <summary>
        /// Icon Path
        /// </summary>
        string Icon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the order of a topic
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value cannot be changed.
        /// </value>
        int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets a list containing the resources this particular topic contains
        /// </summary>
        IEnumerable<TopicResource> Resources { get; set; }

        /// <summary>Indicates if this topic is in the recyclebin</summary>
        bool IsInRecycleBin { get; set; }

        /// <summary>
        /// Gets or sets the business logic properties of the topic
        /// </summary>
        public ITopicBusiness Business { get; set; }        

        /// <summary>
        /// CustomIconId
        /// </summary>
        string CustomIconId { get; set; }
    }
}
