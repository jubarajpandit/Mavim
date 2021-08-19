using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using System.Collections.Generic;
using System.Drawing;

namespace Mavim.Manager.Api.Topic.Business.v1.Models
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
        public ElementType ElementType { get; set; }

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
        /// Gets or sets a value indicating whether a topic can be changed
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value cannot be changed.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the order of a topic
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value cannot be changed.
        /// </value>
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

        public IEnumerable<TopicResource> Resource { get; set; }        

        /// <summary>
        /// CustomIconId
        /// </summary>
        public string CustomIconId { get; set; }
    }
}
