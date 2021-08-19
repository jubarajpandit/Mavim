using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.ChangelogField.Models
{
    public class PatchRouteParams : BaseRouteParam
    {
        [Required]
        [RegularExpression(RegexUtils.AllButEmptyGuid, ErrorMessage = "Cannot use empty Guid")]
        [FromRoute]
        public Guid ChangelogId { get; set; }
    }
}
