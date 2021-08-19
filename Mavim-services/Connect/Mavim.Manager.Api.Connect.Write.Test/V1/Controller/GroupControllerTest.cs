using Mavim.Manager.Api.Connect.Write.V1.Controllers;
using Mavim.Manager.Api.Connect.Write.V1.DTO;
using Mavim.Manager.Connect.Write.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Connect.Write.Test.V1.Controller
{
    public class GroupControllerTest
    {
        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task AddGroup_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var returnId = Guid.NewGuid();
            var command = new AddGroupDto("GroupName", "GroupDescription");
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<AddGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnId);

            // Act
            var actionResult = await controller.AddGroup(mediatR.Object, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<AddGroup.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(new CreateDto(returnId), okObjectResult.Value);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task AddCompany_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var command = new AddGroupDto("GroupName", "GroupDescription");
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<AddGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.AddGroup(mediatR.Object, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task UpdateGroup_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var command = new UpdateGroupDto("GroupName", "GroupDescription");
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<UpdateGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.UpdateGroup(mediatR.Object, id, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<UpdateGroup.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task UpdateGroup_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var command = new UpdateGroupDto("GroupName", "GroupDescription");
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<UpdateGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.UpdateGroup(mediatR.Object, id, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task DeleteGroup_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<DeleteGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.DeleteGroup(mediatR.Object, id, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<DeleteGroup.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task UpdateGroupUser_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var command = new UpdateUserGroupDto(new List<Guid>());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<UpdateUserGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.UpdateGroupUser(mediatR.Object, id, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<UpdateUserGroup.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task UpdateGroupUser_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var command = new UpdateUserGroupDto(new List<Guid>());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<UpdateUserGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.UpdateGroupUser(mediatR.Object, id, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task DeleteGroupUser_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var command = new DeleteUserGroupDto(new List<Guid>());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<DeleteUserGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.DeleteGroupUser(mediatR.Object, id, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<DeleteUserGroup.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect GroupController Write")]
        public async Task DeleteGroupUser_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new GroupsController();
            var id = Guid.NewGuid();
            var command = new DeleteUserGroupDto(new List<Guid>());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<DeleteUserGroup.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.DeleteGroupUser(mediatR.Object, id, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

    }
}
