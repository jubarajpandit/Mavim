using Mavim.Manager.Api.Utils;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// PatchSingleHyperlinkField
    /// </summary>
    public class PatchSingleHyperlinkField
    {
        /// <summary>
        /// Data
        /// </summary>
        [RegularExpression(RegexUtils.Hyperlink, ErrorMessage = "URL is not valid")]
        public string Data { get; set; }
    }
}
