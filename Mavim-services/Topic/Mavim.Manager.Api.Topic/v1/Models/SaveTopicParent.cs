using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Utils;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// SaveTopic
    /// </summary>
    public class SaveTopicParent : ISaveTopicParent
    {
        /// <summary>
        /// Gets or sets the parent id
        /// </summary>
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Topic parent identifier is not valid")]
        public string ParentId { get; set; }
    }
}
