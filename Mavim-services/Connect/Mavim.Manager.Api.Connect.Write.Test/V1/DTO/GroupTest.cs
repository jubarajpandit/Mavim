using Mavim.Manager.Api.Connect.Write.V1.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mavim.Manager.Api.Connect.Write.Test.V1.DTO
{

    public class AddCompanyAddGroupTest : BaseDTO
    {
        [Theory]
        [InlineData("Test", "With description")]
        [InlineData("Test", "")]
        [InlineData("Test", null)]
        [Trait("Category", "Connect AddGroup Write")]
        public void AddGroup_ValidArguments_ValidModelState(string name, string description)
        {
            // Arrange
            var command = new AddGroupDto(name, description);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(!lstErrors.Any());
        }

        [Theory]
        [InlineData("", "with description", "The Name field is required.")]
        [InlineData(null, "with description", "The Name field is required.")]
        [Trait("Category", "Connect AddGroup Write")]
        public void AddGroup_InValidArguments_InValidModelState(string name, string description, string errorMessage)
        {
            // Arrange
            var command = new AddGroupDto(name, description);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(lstErrors.First().ErrorMessage == errorMessage);
        }

    }

    public class UpdateGroupTest : BaseDTO
    {
        [Theory]
        [InlineData("Mavim", "With description")]
        [InlineData("Mavim", "")]
        [InlineData("Mavim", null)]
        [InlineData("", "")]
        [InlineData(null, null)]
        [Trait("Category", "Connect UpdateGroup Write")]
        public void UpdateGroup_ValidArguments_ValidModelState(string name, string description)
        {
            // Arrange
            var command = new UpdateGroupDto(name, description);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(!lstErrors.Any());
        }

    }

    public class AddUsersGroupTest : BaseDTO
    {
        [Fact]
        [Trait("Category", "Connect AddUsersGroup Write")]
        public void AddUsersGroup_ValidArguments_ValidModelState()
        {
            // Arrange
            var payload = new List<Guid>() {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var command = new UpdateUserGroupDto(payload);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(!lstErrors.Any());
        }

        [Fact]
        [Trait("Category", "Connect AddUsersGroup Write")]
        public void AddUsersGroup_InValidArguments_InValidModelState()
        {
            // Arrange
            var payload = new List<Guid>() { new Guid("00000000-0000-0000-0000-000000000000") };
            var command = new UpdateUserGroupDto(payload);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(lstErrors.First().ErrorMessage == "The field UserIds is invalid.");
        }
    }

    public class DeleteUserGroupTest : BaseDTO
    {
        [Fact]
        [Trait("Category", "Connect DeleteUserGroup Write")]
        public void DeleteUserGroup_ValidArguments_ValidModelState()
        {
            // Arrange
            var payload = new List<Guid>() {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var command = new DeleteUserGroupDto(payload);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(!lstErrors.Any());
        }

        [Fact]
        [Trait("Category", "Connect DeleteUserGroup Write")]
        public void DeleteUserGroup_InValidArguments_InValidModelState()
        {
            // Arrange
            var payload = new List<Guid>() { new Guid("00000000-0000-0000-0000-000000000000") };
            var command = new DeleteUserGroupDto(payload);

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(lstErrors.First().ErrorMessage == "The field UserIds is invalid.");
        }
    }
}
