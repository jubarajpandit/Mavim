using Mavim.Manager.Api.Utils;
using Mavim.Manager.Api.Utils.CustomDataAnnotations;
using System.Collections.Generic;
using Xunit;

namespace Mavim.Libraries.Api.Utils.Test.CustomDataAnnotations
{
    public class StringArrayRegexValidatorAttributeTest
    {
        [Theory, MemberData(nameof(ValidHyperlinks))]
        public void IsValid_ValidHyperlinks_True(List<string> hyperlinks)
        {
            // Arrange
            var validator = new StringArrayRegexValidatorAttribute(RegexUtils.Hyperlink);
            validator.AllowEmptyStrings = true;

            // Act
            bool result = validator.IsValid(hyperlinks);

            // Assert
            Assert.True(result);
        }

        [Theory, MemberData(nameof(InvalidHyperlinks))]
        public void IsValid_InvalidHyperlinks_False(List<string> hyperlinks, string errorMessage)
        {
            // Arrange
            var validator = new StringArrayRegexValidatorAttribute(RegexUtils.Hyperlink);

            // Act
            bool result = validator.IsValid(hyperlinks);

            // Assert
            Assert.False(result);
            Assert.Equal(errorMessage, validator.ErrorMessage);
        }

        [Fact]
        public void IsValid_InvalidType_False()
        {
            // Arrange
            var validator = new StringArrayRegexValidatorAttribute(RegexUtils.Hyperlink);

            // Act
            bool result = validator.IsValid(true);

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> ValidHyperlinks
        {
            get
            {
                yield return new object[] { new List<string> { "", "https://www.mavim.com" } };
            }
        }

        public static IEnumerable<object[]> InvalidHyperlinks
        {
            get
            {
                yield return new object[] { new List<string> { "" }, " at index: 0" };
                yield return new object[] { new List<string> { "mavim.com" }, " at index: 0" };
            }
        }
    }
}
