using Mavim.Manager.Api.Connect.Write.V1.DTO;
using System.Linq;
using Xunit;

namespace Mavim.Manager.Api.Connect.Write.Test.V1.DTO
{
    public class AddUserTest : BaseDTO
    {
        [Fact]
        [Trait("Category", "Connect AddGroup Write")]
        public void AddGroup_ValidArguments_ValidModelState()
        {
            // Arrange
            var command = new AddUserDto("Test");

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(!lstErrors.Any());
        }

        [Theory]
        [InlineData("", "The Email field is required.")]
        [InlineData(null, "The Email field is required.")]
        [Trait("Category", "Connect AddGroup Write")]
        public void AddGroup_InValidArguments_InValidModelState(string email, string errorMessage)
        {
            // Arrange
            var command = new AddUserDto(email);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(lstErrors.First().ErrorMessage == errorMessage);
        }

    }
}
