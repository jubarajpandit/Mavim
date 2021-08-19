
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
    public class GroupsControllerTest
    {
        #region AddGroupV1
        [Fact]
        public async Task AddGroupV1_ValidArguments_NoContent()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var tenantId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<AddGroupDTO>(groupId, groupName, groupDescription, tenantId, modelVersion, aggregateId);

            // Act
            var result = await controller.AddGroupV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddGroupV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var tenantId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<AddGroupDTO>(groupId, groupName, groupDescription, tenantId, modelVersion, aggregateId);

            // Act
            var result = await controller.AddGroupV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddGroupV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var tenantId = Guid.NewGuid();
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<AddGroupDTO>(groupId, groupName, groupDescription, tenantId, modelVersion, aggregateId);

            // Act
            var result = await controller.AddGroupV1(mockMediatr.Object, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(unprocessableObject.Value.ToString(), expectedError);
        }
        #endregion

        #region UpdateGroupV1
        [Fact]
        public async Task UpdateGroupV1_ValidArguments_NoContent()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<UpdateGroupDTO>(groupName, groupDescription, modelVersion, aggregateId);

            // Act
            var result = await controller.UpdateGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateGroupV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<UpdateGroupDTO>(groupName, groupDescription, modelVersion, aggregateId);

            // Act
            var result = await controller.UpdateGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateGroupV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var groupId = Guid.NewGuid();
            var groupName = "groupName";
            var groupDescription = "groupDescription";
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<UpdateGroupDTO>(groupName, groupDescription, modelVersion, aggregateId);

            // Act
            var result = await controller.UpdateGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(expectedError, unprocessableObject.Value.ToString());
        }
        #endregion

        #region AddUsersToGroupV1
        [Fact]
        public async Task AddUsersToGroupV1_ValidArguments_NoContent()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<UserIdsDTO>(modelVersion, aggregateId, userIds);

            // Act
            var result = await controller.AddUsersToGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddUsersToGroupV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<UserIdsDTO>(modelVersion, aggregateId, userIds);

            // Act
            var result = await controller.AddUsersToGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddUsersToGroupV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var groupId = Guid.NewGuid();
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<UserIdsDTO>(modelVersion, aggregateId, userIds);

            // Act
            var result = await controller.AddUsersToGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(expectedError, unprocessableObject.Value.ToString());
        }
        #endregion

        #region DeleteUsersFromGroupV1
        [Fact]
        public async Task DeleteUsersFromGroupV1_ValidArguments_NoContent()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<UserIdsDTO>(modelVersion, aggregateId, userIds);

            // Act
            var result = await controller.DeleteUsersFromGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUsersFromGroupV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<UserIdsDTO>(modelVersion, aggregateId, userIds);

            // Act
            var result = await controller.DeleteUsersFromGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUsersFromGroupV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var groupId = Guid.NewGuid();
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<UserIdsDTO>(modelVersion, aggregateId, userIds);

            // Act
            var result = await controller.DeleteUsersFromGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(expectedError, unprocessableObject.Value.ToString());
        }
        #endregion

        #region DisableGroupV1
        [Fact]
        public async Task DisableGroupV1_ValidArguments_NoContent()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.DisableGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DisableGroupV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.DisableGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DisableGroupV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var groupId = Guid.NewGuid();
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.DisableGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(expectedError, unprocessableObject.Value.ToString());
        }
        #endregion

        #region EnableGroupV1
        [Fact]
        public async Task EnableGroupV1_ValidArguments_NoContent()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.EnableGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task EnableGroupV1_InvalidModelState_BadRequest()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            controller.ModelState.AddModelError("error", "error");
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.EnableGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task EnableGroupV1_InvalidModelVersion_UnprocessableEntity()
        {
            // Arrange
            var expectedError = "{ Error = Supplied object contains an invalid modelversion: 0, expected modelversion: 1 }";
            var groupId = Guid.NewGuid();
            var modelVersion = 0;
            var aggregateId = 1;
            var mockMediatr = new Mock<IMediator>();
            var controller = new GroupsController();
            var body = new Mock<MetaDTO>(modelVersion, aggregateId);

            // Act
            var result = await controller.EnableGroupV1(mockMediatr.Object, groupId, body.Object);

            // Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
            var unprocessableObject = result as UnprocessableEntityObjectResult;
            Assert.Equal(expectedError, unprocessableObject.Value.ToString());
        }
        #endregion

        #region AddGroupDTO
        [Fact]
        public void ModelState_ValidAddGroupDTO_ValidModalState()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = "companyName";
            var companyDomain = "companyDomain";
            var tenantid = Guid.NewGuid();
            var modelVersion = 1;
            var aggregateId = 1;
            var body = new AddGroupDTO(companyId, companyName, companyDomain, tenantid, modelVersion, aggregateId);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(AddCompanyDTO), typeof(AddCompanyDTO)), typeof(AddCompanyDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidAddGroupDTOs))]
        public void ModelState_InvalidAddGroupDTO_InvalidModalState(Guid companyId, string companyName, string companyDomain, Guid tenantId, string modalStateError)
        {
            // Arrange
            var body = new AddGroupDTO(companyId, companyName, companyDomain, tenantId, 0, 0);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(AddCompanyDTO), typeof(AddCompanyDTO)), typeof(AddCompanyDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }
        #endregion

        #region UpdateGroupDTO
        [Theory, MemberData(nameof(ValidUpdateGroupDTOs))]
        public void ModelState_ValidUpdateGroupDTO_ValidModalState(string groupName, string groupDescription)
        {
            // Arrange
            var modelVersion = 1;
            var aggregateId = 1;
            var body = new Mock<UpdateGroupDTO>(groupName, groupDescription, modelVersion, aggregateId);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UpdateGroupDTO), typeof(UpdateGroupDTO)), typeof(UpdateGroupDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }
        #endregion

        #region UserIdsDTO
        [Fact]
        public void ModelState_ValidUserIdsDTO_ValidModalState()
        {
            // Arrange
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var modelVersion = 1;
            var aggregateId = 1;
            var body = new UserIdsDTO(modelVersion, aggregateId, userIds);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UserIdsDTO), typeof(UserIdsDTO)), typeof(UserIdsDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidUserIdsDTOs))]
        public void ModelState_InvalidUserIdsDTO_InvalidModalState(IReadOnlyList<Guid> userIds, string modalStateError)
        {
            // Arrange
            var body = new UserIdsDTO(0, 0, userIds);
            var context = new ValidationContext(body, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UserIdsDTO), typeof(UserIdsDTO)), typeof(UserIdsDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(body, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }
        #endregion

        #region Data
        public static IEnumerable<object[]> InvalidAddGroupDTOs
        {
            get
            {
                var validGuid = Guid.NewGuid();
                var invalidGuid = Guid.Empty;
                var validString = "validString";
                var invalidString = "";

                yield return new object[] { invalidGuid, validString, validString, validGuid, "The field Id is invalid." };
                yield return new object[] { validGuid, invalidString, validString, validGuid, "The Name field is required." };
                yield return new object[] { validGuid, validString, validString, invalidGuid, "The field CompanyId is invalid." };
            }
        }
        public static IEnumerable<object[]> ValidUpdateGroupDTOs
        {
            get
            {
                var filledString = "validString";
                var emptyString = "";

                yield return new object[] { filledString, filledString };
                yield return new object[] { filledString, emptyString };
                yield return new object[] { emptyString, filledString };
                yield return new object[] { emptyString, emptyString };
            }
        }

        public static IEnumerable<object[]> InvalidUserIdsDTOs
        {
            get
            {
                var validGuid = Guid.NewGuid();
                var invalidGuid = Guid.Empty;

                yield return new object[] { new List<Guid> { invalidGuid, validGuid }, "The field Ids is invalid." };
                yield return new object[] { new List<Guid> { validGuid, invalidGuid }, "The field Ids is invalid." };
                yield return new object[] { new List<Guid> { invalidGuid, invalidGuid }, "The field Ids is invalid." };
            }
        }
        #endregion
    }
}
