using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Mavim.Manager.Api.Utils.CustomDataAnnotations
{
    public class StringArrayRegexValidatorAttribute : ValidationAttribute
    {
        private readonly string _regex;
        private readonly string _errorMessage;

        public bool AllowEmptyStrings { get; set; }

        public StringArrayRegexValidatorAttribute(string regex)
        {
            _regex = regex ?? throw new ArgumentNullException(nameof(regex));
            _errorMessage = ErrorMessage;
        }

        public override bool IsValid(object value)
        {
            return value is List<string> result && IsPassingValidation(result);
        }

        private bool IsPassingValidation(List<string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (AllowEmptyStrings ? !IsMatchingEmptyStringOrRegex(values[i]) : !IsMatchingRegex(values[i]))
                {
                    ErrorMessage += $"{_errorMessage} at index: {i}";
                    return false;
                }
            }

            return true;
        }

        private bool IsMatchingRegex(string value)
        {
            Regex regex = new Regex(_regex);
            return regex.Match(value).Success;
        }

        private bool IsMatchingEmptyStringOrRegex(string value)
        {
            Regex regex = new Regex(_regex);
            return string.IsNullOrWhiteSpace(value) || regex.Match(value).Success;
        }
    }
}
