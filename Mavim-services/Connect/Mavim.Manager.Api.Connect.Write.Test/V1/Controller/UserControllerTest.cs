using Mavim.Manager.Api.Connect.Write.V1.Controllers;
using Mavim.Manager.Api.Connect.Write.V1.DTO;
using Mavim.Manager.Connect.Write.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Connect.Write.Test.V1.Controller
{
    public class UserControllerTest
    {
        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task AddUser_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new UsersController();
            var companyId = Guid.NewGuid();
            var returnId = Guid.NewGuid();
            var command = new AddUserDto("Test");
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<AddUser.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnId);

            // Act
            var actionResult = await controller.AddUser(mediatR.Object, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<AddUser.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(new CreateDto(returnId), okObjectResult.Value);
        }

        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task AddUser_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new UsersController();
            var companyId = Guid.NewGuid();
            var command = new AddUserDto("Test");
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<AddUser.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.AddUser(mediatR.Object, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task DeleteUser_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new UsersController();
            var userId = Guid.NewGuid();
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<DeleteUser.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.DeleteUser(mediatR.Object, userId, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<DeleteUser.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task DeleteUser_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new UsersController();
            var userId = Guid.NewGuid();
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<DeleteUser.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.DeleteUser(mediatR.Object, userId, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
    }
}
