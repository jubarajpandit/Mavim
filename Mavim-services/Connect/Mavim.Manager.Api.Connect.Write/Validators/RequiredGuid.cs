using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mavim.Manager.Api.Connect.Write.Validators
{
    /// <summary>
    ///     Guid Validator
    /// </summary>
    public class RequiredGuid : ValidationAttribute
    {
        /// <summary>
        /// Guid Validation Attribute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return value is IEnumerable<Guid> list
                ? list.All(l => l != Guid.Empty)
                : value is Guid guid && guid != Guid.Empty;
        }
    }
}