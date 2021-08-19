using Mavim.Manager.Api.Utils;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Controllers.v1.Models
{
    public class SaveRelation
    {
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid TopicId")]
        public string TopicId { get; set; }
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid RelationId")]
        public string RelationId { get; set; }
    }
    public class SaveCreateRelation : SaveRelation
    {
        [Required(ErrorMessage = "Category is a required field")]
        public string Category { get; set; }
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid ToTopicId")]
        public string ToTopicId { get; set; }
    }
    public class SaveEditRelation : SaveCreateRelation
    {
        [Required(ErrorMessage = "OldCategory is a required field")]
        public string OldCategory { get; set; }
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid OldTopicId")]
        public string OldToTopicId { get; set; }
    }
}