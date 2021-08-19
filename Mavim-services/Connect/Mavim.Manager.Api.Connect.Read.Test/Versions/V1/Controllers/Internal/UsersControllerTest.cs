
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
    public class UsersControllerTest
    {
        #region AddUserV1
        [Fact]
        public async Task AddUserV1_ValidArguments_NoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEmail = "userEmail";
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            var body = new Mock<AddUserDTO>(userId, userEmail, companyId, modelVersion, aggregateId);

            // Act
            var result = await controller.AddUserV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddUserV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEmail = "userEmail";
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<AddUserDTO>(userId, userEmail, companyId, modelVersion, aggregateId);

            // Act
            var result = await controller.AddUserV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddUserV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var userId = Guid.NewGuid();
            var userEmail = "userEmail";
            var companyId = Guid.NewGuid();
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            var body = new Mock<AddUserDTO>(userId, userEmail, companyId, modelVersion, aggregateId);

            // Act
            var result = await controller.AddUserV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(unprocessableObject.Value.ToString(), expectedError);
        }
        #endregion

        #region DisableUserV1
        [Fact]
        public async Task DisableUserV1_ValidArguments_NoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.DisableUserV1(mockMediatr.Object, userId, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DisableUserV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.DisableUserV1(mockMediatr.Object, userId, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DisableUserV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var userId = Guid.NewGuid();
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.DisableUserV1(mockMediatr.Object, userId, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(unprocessableObject.Value.ToString(), expectedError);
        }
        #endregion

        #region EnableUserV1
        [Fact]
        public async Task EnableUserV1_ValidArguments_NoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.EnableUserV1(mockMediatr.Object, userId, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task EnableUserV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.EnableUserV1(mockMediatr.Object, userId, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task EnableUserV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var userId = Guid.NewGuid();
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new UsersController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.EnableUserV1(mockMediatr.Object, userId, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(unprocessableObject.Value.ToString(), expectedError);
        }
        #endregion

        #region AddUserDTO
        [Fact]
        public void ModelState_ValidAddUserDTO_ValidModalState()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEmail = "userEmail";
            var companyId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var body = new Mock<AddUserDTO>(userId, userEmail, companyId, modelVersion, aggregateId);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(AddUserDTO), typeof(AddUserDTO)), typeof(AddUserDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidAddUserDTOs))]
        public void ModelState_InvalidAddUserDTO_InvalidModalState(Guid userId, string userEmail, Guid companyId, string modalStateError)
        {
            // Arrange
            var body = new AddUserDTO(userId, userEmail, companyId, 0, 0);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(AddUserDTO), typeof(AddUserDTO)), typeof(AddUserDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }

        public static IEnumerable<object[]> InvalidAddUserDTOs
        {
            get
            {
                var validGuid = Guid.NewGuid();
                var invalidGuid = Guid.Empty;
                var validString = "validString";
                var invalidString = "";

                yield return new object[] { invalidGuid, validString, validGuid, "The field Id is invalid." };
                yield return new object[] { validGuid, invalidString, validGuid, "The Email field is required." };
                yield return new object[] { validGuid, validString, invalidGuid, "The field CompanyId is invalid." };
            }
        }
        #endregion
    }
}
