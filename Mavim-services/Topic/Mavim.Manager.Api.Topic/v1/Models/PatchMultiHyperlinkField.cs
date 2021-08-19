using Mavim.Manager.Api.Utils;
using Mavim.Manager.Api.Utils.CustomDataAnnotations;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// PatchMultiHyperlinkField
    /// </summary>
    public class PatchMultiHyperlinkField
    {
        /// <summary>
        /// Data
        /// </summary>
        [StringArrayRegexValidator(RegexUtils.Hyperlink, AllowEmptyStrings = true, ErrorMessage = "URL is not valid")]
        public IEnumerable<string> Data { get; set; }
    }
}
