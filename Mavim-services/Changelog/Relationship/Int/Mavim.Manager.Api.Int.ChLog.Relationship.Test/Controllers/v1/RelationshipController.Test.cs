using FluentAssertions;
using Mavim.Manager.Api.Int.ChLog.Relationship.Controllers.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Controllers.v1.Models;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Enum;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Service = Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1.Model;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Test.Controllers.v1
{
    public class RelationshipControllerTest
    {
        #region private vars
        private readonly Guid _dbId = new Guid("0d4dea97-487d-460e-898c-2b242432a3bb");
        private readonly Guid _changelogId = new Guid("a711cae7-cd47-493a-a5a9-66ec34549bc1");
        private readonly string _topicId = "d12950883c414v0";
        private readonly string _relationId = "d12950883c414v1";
        private readonly string _fromCategory = "when";
        private readonly string _fromTopicId = "d12950883c414v3";
        private readonly string _toCategory = "who";
        private readonly string _toTopicId = "d12950883c414v5";
        #endregion

        #region GetRelations
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetRelations_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.GetRelationsByTopic(It.IsAny<Guid>(), It.IsAny<string>()))
                       .ReturnsAsync(() => new Mock<IEnumerable<IRelation>>().Object);

            var logger = new Mock<ILogger<RelationshipController>>();
            var controller = new RelationshipController(mockService.Object, logger.Object);

            //Act
            var actionResult = await controller.GetRelationsByTopic(_dbId, _topicId);

            mockService.Verify(mock => mock.GetRelationsByTopic(_dbId, _topicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IRelation> titles = okObjectResult.Value as IEnumerable<IRelation>;
            Assert.NotNull(titles);
        }
        #endregion

        #region GetStatusByTopic
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetStatusByTopic_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.GetRelationStatus(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(ChangeStatus.Approved);

            var logger = new Mock<ILogger<RelationshipController>>();
            var controller = new RelationshipController(mockService.Object, logger.Object);

            //Act
            var actionResult = await controller.GetStatusByTopic(_dbId, _topicId);

            mockService.Verify(mock => mock.GetRelationStatus(_dbId, _topicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.True(Enum.IsDefined(typeof(ChangeStatus), okObjectResult.Value));
        }
        #endregion

        #region GetPendingRelations
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task GetPendingRelations_ValidArguments_OkObjectResult()
        {
            //Arrange
            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.GetPendingRelationsByTopic(It.IsAny<Guid>(), It.IsAny<string>()))
                       .ReturnsAsync(() => new Mock<IEnumerable<IRelation>>().Object);

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);

            //Act
            var actionResult = await controller.GetPendingRelationsByTopic(_dbId, _topicId);

            mockService.Verify(mock => mock.GetPendingRelationsByTopic(_dbId, _topicId), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            IEnumerable<IRelation> titles = okObjectResult.Value as IEnumerable<IRelation>;
            Assert.NotNull(titles);
        }
        #endregion

        #region SaveCreateRelation
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveCreateRelation_ValidArguments_OkObjectResult()
        {
            //Arrange
            var saveRelation = new SaveCreateRelation
            {
                TopicId = _topicId,
                RelationId = _relationId,
                Category = _toCategory,
                ToTopicId = _toTopicId
            };

            var saveRelationService = new Service.SaveRelation
            {
                TopicId = _topicId,
                RelationId = _relationId,
                Category = _toCategory,
                ToTopicId = _toTopicId,
                Action = Services.Interfaces.v1.Enum.Action.Create
            };

            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.SaveRelation(It.IsAny<Guid>(), It.IsAny<Service.SaveRelation>()))
                       .ReturnsAsync(() => new Mock<IRelation>().Object);

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.Clear();

            Func<Service.SaveRelation, bool> validate = relation =>
            {
                relation.Should().BeEquivalentTo(saveRelationService);
                return true;
            };

            //Act
            var actionResult = await controller.CreateRelation(_dbId, saveRelation);

            mockService.Verify(mock => mock.SaveRelation(
                _dbId,
                It.Is<Service.SaveRelation>(s => validate(s))), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveCreateRelation_InvalidArgument_BadRequest()
        {
            var mockService = new Mock<IRelationService>();

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.AddModelError("error", "error");

            //Act
            var actionResult = await controller.CreateRelation(_dbId, null);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
        #endregion

        #region SaveDeleteRelation
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveDeleteRelation_ValidArguments_OkObjectResult()
        {
            //Arrange
            SaveRelation saveRelation = new SaveRelation
            {
                TopicId = _topicId,
                RelationId = _relationId,
            };

            var saveRelationService = new Service.SaveRelation
            {
                TopicId = _topicId,
                RelationId = _relationId,
                Action = Services.Interfaces.v1.Enum.Action.Delete
            };

            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.SaveRelation(It.IsAny<Guid>(), It.IsAny<Service.SaveRelation>()))
                       .ReturnsAsync(() => new Mock<IRelation>().Object);

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.Clear();

            Func<Service.SaveRelation, bool> validate = relation =>
            {
                relation.Should().BeEquivalentTo(saveRelationService);
                return true;
            };

            //Act
            var actionResult = await controller.DeleteRelation(_dbId, saveRelation);

            mockService.Verify(mock => mock.SaveRelation(
                _dbId,
                It.Is<Service.SaveRelation>(s => validate(s))), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveDeleteRelation_InvalidArgument_BadRequest()
        {
            var mockService = new Mock<IRelationService>();

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.AddModelError("error", "error");

            //Act
            var actionResult = await controller.DeleteRelation(_dbId, null);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
        #endregion

        #region SaveEditRelation
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveEditRelation_ValidArguments_OkObjectResult()
        {
            //Arrange
            var saveRelation = new SaveEditRelation
            {
                TopicId = _topicId,
                RelationId = _relationId,
                OldCategory = _fromCategory,
                OldToTopicId = _fromTopicId,
                Category = _toCategory,
                ToTopicId = _toTopicId
            };

            var saveRelationService = new Service.SaveRelation
            {
                TopicId = _topicId,
                RelationId = _relationId,
                Action = Services.Interfaces.v1.Enum.Action.Edit,
                OldCategory = _fromCategory,
                OldTopicId = _fromTopicId,
                Category = _toCategory,
                ToTopicId = _toTopicId
            };

            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.SaveRelation(It.IsAny<Guid>(), It.IsAny<Service.SaveRelation>()))
                       .ReturnsAsync(() => new Mock<IRelation>().Object);

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.Clear();

            Func<Service.SaveRelation, bool> validate = relation =>
            {
                relation.Should().BeEquivalentTo(saveRelationService);
                return true;
            };

            //Act
            var actionResult = await controller.EditRelation(_dbId, saveRelation);

            mockService.Verify(mock => mock.SaveRelation(
                _dbId,
                It.Is<Service.SaveRelation>(s => validate(s))), Times.Once);
            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task SaveEditRelation_InvalidArgument_BadRequest()
        {
            var mockService = new Mock<IRelationService>();

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.AddModelError("error", "error");

            //Act
            var actionResult = await controller.EditRelation(_dbId, null);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
        #endregion

        #region ApproveRelation
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task ApproveRelation_ValidArguments_OkObjectResult()
        {
            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.UpdateRelationStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ChangeStatus>()));

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.Clear();

            //Act
            await controller.ApproveRelation(_dbId, _changelogId);

            mockService.Verify(mock => mock.UpdateRelationStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ChangeStatus>()), Times.Once);
        }
        #endregion

        #region RejectRelation
        [Fact]
        [Trait("Category", "ChangelogRelation")]
        public async Task RejectRelation_ValidArguments_OkObjectResult()
        {
            var mockService = new Mock<IRelationService>();
            mockService.Setup(x => x.UpdateRelationStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ChangeStatus>()));

            var logger = new Mock<ILogger<RelationshipController>>();

            var controller = new RelationshipController(mockService.Object, logger.Object);
            controller.ModelState.Clear();

            //Act
            await controller.RejectRelation(_dbId, _changelogId);

            mockService.Verify(mock => mock.UpdateRelationStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ChangeStatus>()), Times.Once);
        }
        #endregion
    }
}
