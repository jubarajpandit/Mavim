using Mavim.Manager.Api.Ext.ChLog.Relationship.Controllers.v1;
using Mavim.Manager.Api.Ext.ChLog.Relationship.Models;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Enums;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Ext.ChLog.Tests.Controllers.v1
{
    public class ChangelogRelationshipPublicControllerTests
    {
        #region Private Members
        private Mock<IChangelogRelationshipPublicService> _mockService;
        private ChangelogRelationshipPublicController _relationshipController;
        private Mock<ILogger<ChangelogRelationshipPublicController>> _mockLogger;
        private const string _validTopicId = "d0c2v0";
        #endregion

        #region GetRelations tests
        [Fact]
        public async Task GetRelations_OneOrMoreEmptyArguments_BadRequestException()
        {
            // Arrange
            ArrangeCommonObjects();
            _relationshipController.ModelState.AddModelError("error", "error");
            var routeParams = new GetByTopicRouteParams();

            // Act
            var actionResult = await _relationshipController.GetRelations(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }

        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task GetRelations_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;
            _mockService.Setup(x => x.GetRelations(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(new Mock<IEnumerable<IChangelogRelationship>>().Object));

            // Act
            ActionResult<IEnumerable<IChangelogRelationship>> actionResult = await _relationshipController.GetRelations(routeParams);

            // Assert
            _mockService.Verify(mock => mock.GetRelations(routeParams.DatabaseId, routeParams.TopicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogRelationship> relationships = okObjectResult.Value as IEnumerable<IChangelogRelationship>;
            Assert.NotNull(relationships);
        }
        #endregion

        #region GetPendingRelations tests
        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task GetPendingRelations_OneOrMoreEmptyArguments_BadRequestException()
        {
            // Arrange
            ArrangeCommonObjects();
            _relationshipController.ModelState.AddModelError("error", "error");
            var routeParams = new GetByTopicRouteParams();

            // Act
            var actionResult = await _relationshipController.GetPendingRelations(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }

        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task GetPendingRelations_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;
            _mockService.Setup(x => x.GetPendingRelations(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(new Mock<IEnumerable<IChangelogRelationship>>().Object));

            // Act
            ActionResult<IEnumerable<IChangelogRelationship>> actionResult = await _relationshipController.GetPendingRelations(routeParams);

            // Assert
            _mockService.Verify(mock => mock.GetPendingRelations(routeParams.DatabaseId, routeParams.TopicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogRelationship> relationships = okObjectResult.Value as IEnumerable<IChangelogRelationship>;
            Assert.NotNull(relationships);
        }
        #endregion

        #region GetAllPendingRelations tests
        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task GetAllPendingRelations_OneOrMoreEmptyArguments_BadRequestException()
        {
            // Arrange
            ArrangeCommonObjects();
            _relationshipController.ModelState.AddModelError("error", "error");
            var routeParams = new BaseRouteParam();

            // Act
            var actionResult = await _relationshipController.GetAllPendingRelations(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }

        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task GetAllPendingRelations_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            _mockService.Setup(x => x.GetAllPendingRelations(It.IsAny<Guid>())).Returns(() => Task.FromResult(new Mock<IEnumerable<IChangelogRelationship>>().Object));

            // Act
            ActionResult<IEnumerable<IChangelogRelationship>> actionResult = await _relationshipController.GetAllPendingRelations(routeParams);

            // Assert
            _mockService.Verify(mock => mock.GetAllPendingRelations(routeParams.DatabaseId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IChangelogRelationship> relationships = okObjectResult.Value as IEnumerable<IChangelogRelationship>;
            Assert.NotNull(relationships);
        }
        #endregion

        #region GetRelationStatus tests
        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task GetRelationStatus_OneOrMoreEmptyArguments_BadRequestException()
        {
            // Arrange
            ArrangeCommonObjects();
            _relationshipController.ModelState.AddModelError("error", "error");
            var routeParams = new GetByTopicRouteParams();

            // Act
            var actionResult = await _relationshipController.GetRelationStatus(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }

        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task GetRelationStatus_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;
            Mock<IChangelogRelationship> mock = new Mock<IChangelogRelationship>();
            mock.SetupGet(p => p.Status).Returns(ChangeStatus.Rejected);
            _mockService.Setup(x => x.GetRelationStatus(It.IsAny<Guid>(), It.IsAny<string>())).Returns(() => Task.FromResult(mock.Object.Status));

            // Act
            ActionResult<ChangeStatus> actionResult = await _relationshipController.GetRelationStatus(routeParams);

            // Assert
            _mockService.Verify(mock => mock.GetRelationStatus(routeParams.DatabaseId, routeParams.TopicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            ChangeStatus status = Map(okObjectResult.Value.ToString());
            Assert.Equal(ChangeStatus.Rejected, status);
        }
        #endregion

        #region ApproveRelation tests
        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task ApproveRelation_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.ChangelogId = Guid.NewGuid();

            _mockService.Setup(x => x.ApproveRelation(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(() => Task.FromResult(new Mock<IChangelogRelationship>().Object));

            // Act
            ActionResult<IChangelogRelationship> actionResult = await _relationshipController.ApproveRelation(routeParams);

            // Assert
            _mockService.Verify(mock => mock.ApproveRelation(routeParams.DatabaseId, routeParams.ChangelogId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IChangelogRelationship relationship = okObjectResult.Value as IChangelogRelationship;
            Assert.NotNull(relationship);
        }

        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task ApproveRelation_OneOrMoreEmptyArguments_BadRequestException()
        {
            // Arrange
            ArrangeCommonObjects();
            _relationshipController.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();

            // Act
            var actionResult = await _relationshipController.ApproveRelation(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region RejectRelation tests
        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task RejectRelation_ValidArguments_OkObjectResult()
        {
            // Arrange
            ArrangeCommonObjects();
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.ChangelogId = Guid.NewGuid();

            _mockService.Setup(x => x.RejectRelation(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(() => Task.FromResult(new Mock<IChangelogRelationship>().Object));

            // Act
            ActionResult<IChangelogRelationship> actionResult = await _relationshipController.RejectRelation(routeParams);

            // Assert
            _mockService.Verify(mock => mock.RejectRelation(routeParams.DatabaseId, routeParams.ChangelogId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IChangelogRelationship relationship = okObjectResult.Value as IChangelogRelationship;
            Assert.NotNull(relationship);
        }

        [Fact]
        [Trait("Category", "ChangelogRelationshipPublic")]
        public async Task RejectRelation_OneOrMoreEmptyArguments_BadRequestException()
        {
            // Arrange
            ArrangeCommonObjects();
            _relationshipController.ModelState.AddModelError("error", "error");
            var routeParams = new PatchRouteParams();

            // Act
            var actionResult = await _relationshipController.RejectRelation(routeParams);

            // Assert
            Assert.NotNull(actionResult);
            BadRequestObjectResult badRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
        }
        #endregion

        #region ModelState
        [Fact]
        public void ModelState_ValidGetByTopicRouteParams_ValidModalState()
        {
            // Arrange
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.TopicId = _validTopicId;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfTopicRouteParams))]
        public void ModelState_InvalidGetByTopicRouteParams_InvalidModalState(Guid dbId, string topicId, string modalStateError)
        {
            // Arrange
            var routeParams = new GetByTopicRouteParams();
            routeParams.DatabaseId = dbId;
            routeParams.TopicId = topicId;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }

        [Fact]
        public void ModelState_ValidBaseRouteParam_ValidModalState()
        {
            // Arrange
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.NewGuid();
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Fact]
        public void ModelState_InvalidBaseRouteParam_ValidModalState()
        {
            // Arrange
            var routeParams = new BaseRouteParam();
            routeParams.DatabaseId = Guid.Empty;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == "Cannot use empty Guid");
        }

        [Fact]
        public void ModelState_ValidPatchRouteParams_ValidModalState()
        {
            // Arrange
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = Guid.NewGuid();
            routeParams.ChangelogId = Guid.NewGuid();
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.True(isModelStateValid);
        }

        [Theory, MemberData(nameof(InvalidCombinationOfPatchRouteParams))]
        public void ModelState_InvalidPatchRouteParams_InvalidModalState(Guid dbId, Guid changelogId, string modalStateError)
        {
            // Arrange
            var routeParams = new PatchRouteParams();
            routeParams.DatabaseId = dbId;
            routeParams.ChangelogId = changelogId;
            var context = new ValidationContext(routeParams, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(GetByTopicRouteParams), typeof(GetByTopicRouteParams)), typeof(GetByTopicRouteParams));

            // Act
            var isModelStateValid = Validator.TryValidateObject(routeParams, context, results, true);

            // Assert
            Assert.False(isModelStateValid);
            Assert.Contains(results, err => err.ErrorMessage == modalStateError);
        }
        #endregion

        #region TestData
        public static IEnumerable<object[]> InvalidDbIds
        {
            get
            {
                yield return new object[] { Guid.Empty };
                yield return new object[] { null };
            }
        }

        public static IEnumerable<object[]> InvalidDbIdAndTopicIdCombinations
        {
            get
            {
                yield return new object[] { Guid.Empty, "d12950883c414v0" };
                yield return new object[] { null, "d12950883c414v0" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), "" };
                yield return new object[] { new Guid("0d4dea97-487d-460e-898c-2b242432a3bb"), null };
            }
        }

        public static IEnumerable<object[]> InvalidDbIdAndTopicIdAndRelationIdCombinations
        {
            get
            {
                yield return new object[] { Guid.Empty, Guid.NewGuid() };
                yield return new object[] { Guid.NewGuid(), Guid.Empty };
            }
        }

        public static IEnumerable<object[]> InvalidCombinationOfTopicRouteParams
        {
            get
            {
                yield return new object[] { Guid.Empty, _validTopicId, "Cannot use empty Guid" };
                yield return new object[] { Guid.NewGuid(), "test", "Invalid topicId format" };
                yield return new object[] { Guid.NewGuid(), null, "The TopicId field is required." };
            }
        }

        public static IEnumerable<object[]> InvalidCombinationOfPatchRouteParams
        {
            get
            {
                yield return new object[] { Guid.Empty, Guid.NewGuid(), "Cannot use empty Guid" };
                yield return new object[] { Guid.NewGuid(), Guid.Empty, "Cannot use empty Guid" };
            }
        }
        #endregion

        #region Private Methods
        private static ChangeStatus Map(string titleStatus)
        {
            return titleStatus switch
            {
                "Pending" => ChangeStatus.Pending,
                "Approved" => ChangeStatus.Approved,
                "Rejected" => ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(titleStatus), titleStatus, null)
            };
        }

        private void ArrangeCommonObjects()
        {
            _mockService = new Mock<IChangelogRelationshipPublicService>();
            _mockLogger = new Mock<ILogger<ChangelogRelationshipPublicController>>();
            _relationshipController = new ChangelogRelationshipPublicController(_mockService.Object, _mockLogger.Object);
        }
        #endregion
    }
}
