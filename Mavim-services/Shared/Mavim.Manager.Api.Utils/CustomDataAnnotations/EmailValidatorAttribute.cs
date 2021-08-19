using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Mavim.Manager.Api.Utils.CustomDataAnnotations
{
    public class EmailValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is string email && IsMatchingEmailRegex(email);
        }

        private bool IsMatchingEmailRegex(string email)
        {
            Regex regex = new Regex(RegexUtils.Email, RegexOptions.IgnoreCase);
            return regex.Match(email).Success;
        }
    }
}
