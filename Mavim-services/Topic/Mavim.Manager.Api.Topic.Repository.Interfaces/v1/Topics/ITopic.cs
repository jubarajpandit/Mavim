using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Model;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics
{
    public interface ITopic
    {
        /// <summary>
        /// DcvId object
        /// </summary>
        IDcvId Dcv { get; set; }

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
        /// TypeCategory
        /// </summary>
        enums.ElementType ElementType { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        IType Type { get; set; }

        /// <summary>
        /// Icon Path
        /// </summary>
        string Icon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is chart.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is chart; otherwise, <c>false</c>.
        /// </value>
        bool IsChart { get; set; }

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

        /// <summary>
        /// Contains all resources of topic
        /// </summary>
        List<TopicResource> Resource { get; set; }        

        /// <summary>
        /// CustomIconId
        /// </summary>
        string CustomIconId { get; set; }
    }
}
