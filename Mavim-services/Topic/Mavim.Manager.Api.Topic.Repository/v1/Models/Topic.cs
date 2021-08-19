using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;
using Mavim.Manager.Model;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.v1.Models
{
    public class Topic : ITopic
    {
        /// <summary>
        /// DcvId object
        /// </summary>
        public IDcvId Dcv { get; set; }

        /// <summary>Gets or sets the parent DCV.</summary>
        /// <value>The parent DCV.</value>
        public string Parent { get; set; }

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
        public Interfaces.v1.enums.ElementType ElementType { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public IType Type { get; set; }

        /// <summary>
        /// Icon Path
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is chart.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is chart; otherwise, <c>false</c>.
        /// </value>
        public bool IsChart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a topic is internal
        /// </summary>
        /// <value>
        ///   <c>true</c> if the topic is internal; otherwise, <c>false</c>.
        /// </value>
        public bool IsInternal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the order of a topic
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// HasChildren
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// CanDelete
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// CanCreateChildTopic
        /// </summary>
        public bool CanCreateChildTopic { get; set; }

        /// <summary>
        /// CanCreateTopicAfter
        /// </summary>
        public bool CanCreateTopicAfter { get; set; }

        /// <summary>
        /// IsInRecycleBin
        /// </summary>
        public bool IsInRecycleBin { get; set; }

        public List<TopicResource> Resource { get; set; }        

        /// <summary>
        /// CustomIconId
        /// </summary>
        public string CustomIconId { get; set; }
    }
}
