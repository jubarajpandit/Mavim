using Mavim.Manager.Api.Utils;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.ChangelogField.Models
{
    public class FieldChange<T>
    {
        [Required]
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid topicId format")]
        public string TopicId { get; set; }
        [Required]
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid fieldSetId format")]
        public string FieldSetId { get; set; }
        [Required]
        [RegularExpression(RegexUtils.Dcv, ErrorMessage = "Invalid fieldId format")]
        public string FieldId { get; set; }
        public T OldFieldValue { get; set; }
        public T NewFieldValue { get; set; }
    }
}
