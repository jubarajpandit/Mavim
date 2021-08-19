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
    public class CompanyControllerTest
    {
        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task AddCompany_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new CompanyController();
            var companyId = Guid.NewGuid();
            var returnId = Guid.NewGuid();
            var command = new AddCompanyDto("Test", "test.nl", Guid.NewGuid());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<AddCompany.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnId);

            // Act
            var actionResult = await controller.AddCompany(mediatR.Object, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<AddCompany.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(new CreateDto(returnId), okObjectResult.Value);
        }

        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task AddCompany_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new CompanyController();
            var companyId = Guid.NewGuid();
            var command = new AddCompanyDto("Test", "test.nl", Guid.NewGuid());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<AddCompany.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.AddCompany(mediatR.Object, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task UndoEventSourcing_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new CompanyController();
            var command = new UndoEventSourcingDto(DateTime.Now);
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<UndoEventSourcing.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.UndoEventSourcing(mediatR.Object, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<UndoEventSourcing.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task UndoEventSourcing_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new CompanyController();
            var companyId = Guid.NewGuid();
            var command = new UndoEventSourcingDto(DateTime.Now);
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<UndoEventSourcing.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.UndoEventSourcing(mediatR.Object, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        #region Resend
        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task ResendEventSourcing_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var controller = new CompanyController();
            var command = new ResendEventSourcingDto(0, Guid.NewGuid());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<ResendEventSourcing.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.ResendEventSourcing(mediatR.Object, command, new CancellationToken());

            // Assert
            mediatR.Verify(mock => mock.Send(It.IsAny<ResendEventSourcing.Command>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        [Trait("Category", "Connect CompanyController Write")]
        public async Task ResendEventSourcing_InValidArguments_InValidModelStateAsync()
        {
            // Arrange
            var controller = new CompanyController();
            var companyId = Guid.NewGuid();
            var command = new ResendEventSourcingDto(0, Guid.NewGuid());
            var mediatR = new Mock<IMediator>();
            mediatR.Setup(mediatR => mediatR.Send(It.IsAny<ResendEventSourcing.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var actionResult = await controller.ResendEventSourcing(mediatR.Object, command, new CancellationToken());

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        #endregion
    }
}
