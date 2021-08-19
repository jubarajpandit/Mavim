using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Ext.ChLog.Relationship.Models
{
    public class GetByTopicRouteParams : BaseRouteParam
    {
        [Required]
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid topicId format")]
        [FromRoute]
        public string TopicId { get; set; }
    }
}
