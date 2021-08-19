using Mavim.Manager.Api.Connect.Write.V1.DTO;
using System;
using System.Linq;
using Xunit;

namespace Mavim.Manager.Api.Connect.Write.Test.V1.DTO
{
    public class AddCompany : BaseDTO
    {
        [Theory]
        [InlineData("Mavim", "mavim.com", "f0fd2957-ddca-40c2-bc87-7dc9029ad2d3")]
        [Trait("Category", "Connect Company Write")]
        public void AddCompany_ValidArguments_ValidModelState(string name, string domain, string tenantId)
        {
            // Arrange
            var command = new AddCompanyDto(name, domain, new Guid(tenantId));

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(!lstErrors.Any());
        }

        [Theory]
        [InlineData("Mavim", "mavim.com", "00000000-0000-0000-0000-000000000000", "The field TenantId is invalid.")]
        [InlineData("", "mavim.com", "f0fd2957-ddca-40c2-bc87-7dc9029ad2d3", "The Name field is required.")]
        [InlineData("Mavim", "", "f0fd2957-ddca-40c2-bc87-7dc9029ad2d3", "The Domain field is required.")]
        [Trait("Category", "Connect AddCompany Write")]
        public void AddCompany_InValidArguments_InValidModelState(string name, string domain, string tenantId, string errorMessage)
        {
            // Arrange
            var command = new AddCompanyDto(name, domain, new Guid(tenantId));
            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(lstErrors.First().ErrorMessage == errorMessage);
        }

    }

    public class UndoEventSourcing : BaseDTO
    {
        [Fact]
        [Trait("Category", "Connect UndoEventSourcing Write")]
        public void UndoEventSourcing_ValidArguments_ValidModelState()
        {
            // Arrange
            var command = new UndoEventSourcingDto(new DateTime(2020, 06, 10));

            // Act
            var lstErrors = ValidateModel(command);

            // Assert
            Assert.True(!lstErrors.Any());
        }

    }
}
