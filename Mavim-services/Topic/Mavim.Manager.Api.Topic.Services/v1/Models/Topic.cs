using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class Topic : ITopic
    {
        /// <summary>
        /// Combined ids of the Dcv in string format
        /// </summary>
        public string Dcv { get; set; }

        /// <summary>Gets or sets the parent DCV.</summary>
        /// <value>The parent DCV.</value>
        public string Parent { get; set; }

        /// <summary>Indicates if this topic has children</summary>
        /// <value><c>true</c> if this topic has children.</value>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// TypeCategory
        /// </summary>
        public TopicType TypeCategory { get; set; }

        /// <summary>
        /// Icon Path
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the order of a topic
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value cannot be changed.
        /// </value>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets a list containing the resources this particular topic contains
        /// </summary>
        public IEnumerable<TopicResource> Resources { get; set; }

        /// <summary>Indicates if this topic is in the recyclebin</summary>
        /// <value><c>true</c> if this topic is in the recyclebin.</value>
        public bool IsInRecycleBin { get; set; }

        /// <summary>
        /// Gets or sets the business logic properties of the topic
        /// </summary>
        public ITopicBusiness Business { get; set; }        

        /// <summary>
        /// Custom Icon Id
        /// </summary>
        public string CustomIconId { get; set; }
    }
}
