
using Mavim.Manager.Api.Connect.Read.Versions.V1.Controllers.Internal;
using Mavim.Manager.Api.Connect.Read.Versions.V1.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Connect.Read.Test.Versions.V1.Controllers.Internal
{
    public class CompanyControllerTest
    {
        #region AddCompanyV1
        [Fact]
        public async Task AddCompanyV1_ValidArguments_NoContent()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = "companyName";
            var companyDomain = "companyDomain";
            var tenantid = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new CompanyController();
            var body = new Mock<AddCompanyDTO>(companyId, companyName, companyDomain, tenantid, modelVersion, aggregateId);

            // Act
            var result = await controller.AddCompanyV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddCompanyV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = "companyName";
            var companyDomain = "companyDomain";
            var tenantid = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new CompanyController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<AddCompanyDTO>(companyId, companyName, companyDomain, tenantid, modelVersion, aggregateId);

            // Act
            var result = await controller.AddCompanyV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddCompanyV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var companyId = Guid.NewGuid();
            var companyName = "companyName";
            var companyDomain = "companyDomain";
            var tenantid = Guid.NewGuid();
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new CompanyController();
            var body = new Mock<AddCompanyDTO>(companyId, companyName, companyDomain, tenantid, modelVersion, aggregateId);

            // Act
            var result = await controller.AddCompanyV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(unprocessableObject.Value.ToString(), expectedError);
        }
        #endregion

        #region ModelState
        [Fact]
        public void ModelState_ValidAddCompanyDTO_ValidModalState()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = "companyName";
            var companyDomain = "companyDomain";
            var tenantid = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var body = new AddCompanyDTO(companyId, companyName, companyDomain, tenantid, modelVersion, aggregateId);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(AddCompanyDTO), typeof(AddCompanyDTO)), typeof(AddCompanyDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidAddCompanyDTOs))]
        public void ModelState_InvalidAddCompanyDTO_InvalidModalState(Guid companyId, string companyName, string companyDomain, Guid tenantId, string modalStateError)
        {
            // Arrange
            var body = new AddCompanyDTO(companyId, companyName, companyDomain, tenantId, 0, 0);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(AddCompanyDTO), typeof(AddCompanyDTO)), typeof(AddCompanyDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }

        public static IEnumerable<object[]> InvalidAddCompanyDTOs
        {
            get
            {
                var validGuid = Guid.NewGuid();
                var invalidGuid = Guid.Empty;
                var validString = "validString";
                var invalidString = "";

                yield return new object[] { invalidGuid, validString, validString, validGuid, "The field Id is invalid." };
                yield return new object[] { validGuid, invalidString, validString, validGuid, "The Name field is required." };
                yield return new object[] { validGuid, validString, invalidString, validGuid, "The Domain field is required." };
                yield return new object[] { validGuid, validString, validString, invalidGuid, "The field TenantId is invalid." };
            }
        }
        #endregion
    }
}
