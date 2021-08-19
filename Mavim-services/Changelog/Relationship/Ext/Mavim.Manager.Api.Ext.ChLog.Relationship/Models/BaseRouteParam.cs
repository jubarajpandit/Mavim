using Mavim.Manager.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Ext.ChLog.Relationship.Models
{
    public class BaseRouteParam
    {
        [Required]
        [RegularExpression(RegexUtils.AllButEmptyGuid, ErrorMessage = "Cannot use empty Guid")]
        [FromRoute]
        public Guid DatabaseId { get; set; }
    }
}
