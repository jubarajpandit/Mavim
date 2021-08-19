using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1
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

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// ElementType
        /// </summary>
        ElementType ElementType { get; set; }

        /// <summary>
        /// Icon Path
        /// </summary>
        string Icon { get; set; }

        /// <summary>
        /// IsChart
        /// </summary>
        public bool IsChart { get; set; }

        /// <summary>
        /// IsReadOnly
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the order of a topic
        /// </summary>
        /// <value>
        ///   <c>true</c> if the value cannot be changed.
        /// </value>
        int OrderNumber { get; set; }

        /// <summary>
        /// HasChildren
        /// </summary>
        bool HasChildren { get; set; }

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

        /// <summary>
        /// IsInRecycleBin
        /// </summary>
        bool IsInRecycleBin { get; set; }        

        IEnumerable<TopicResource> Resource { get; set; }

        /// <summary>
        /// CustomIconId
        /// </summary>
        string CustomIconId { get; set; }
    }
}
