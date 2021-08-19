using Mavim.Manager.Connect.Write.Commands;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using Microsoft.Azure.ServiceBus;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Test.Commands
{
    public class AddUserTest
    {
        [Fact]
        [Trait("Category", "Connect AddUserCommand Write")]
        public async Task AddUser_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var eventSourcing = new Mock<IEventSourcingGeneric<UserV1>>();
            eventSourcing.Setup(es => es.CreateNewEvent(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UserV1>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(MockData.EmptyMock);
            eventSourcing.Setup(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            eventSourcing.Setup(es => es.SaveEvent(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var handler = new AddUser.Handler(eventSourcing.Object, identity, serviceBus.Object);
            var command = new AddUser.Command("Test@Mavim.nl");
            var cancellationToken = new CancellationToken();

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            eventSourcing.Verify(es => es.CreateNewEvent(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UserV1>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            eventSourcing.Verify(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()), Times.Once);
            eventSourcing.Verify(es => es.SaveEvent(It.IsAny<CancellationToken>()), Times.Once);
            serviceBus.Verify(sb => sb.SendAsync(It.IsAny<Message>()), Times.Once);

            Assert.IsType<Guid>(result);
        }

        [Fact]
        [Trait("Category", "Connect AddUserCommand Write")]
        public async Task AddUser_InvalidDependencyInjection_ArgumentNullException()
        {
            // Arrange
            IEventSourcingGeneric<UserV1> eventSourcing = null;
            var cancellationToken = new CancellationToken();

            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var serviceBus = new Mock<IQueueClient>();

            // Act
            Exception exception = await Record.ExceptionAsync(() => new AddUser.Handler(eventSourcing, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'eventSourcing')", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect AddUserCommand Write")]
        public async Task AddUser_DatabaseFailed_Exception()
        {
            // Arrange
            var eventSourcing = new Mock<IEventSourcingGeneric<UserV1>>();
            eventSourcing.Setup(es => es.CreateNewEvent(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UserV1>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(MockData.EmptyMock);

            eventSourcing.Setup(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            eventSourcing.Setup(es => es.SaveEvent(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(false));

            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var handler = new AddUser.Handler(eventSourcing.Object, identity, serviceBus.Object);
            var command = new AddUser.Command("Test@Mavim.nl");
            var cancellationToken = new CancellationToken();

            // Act
            Exception exception = await Record.ExceptionAsync(() => handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
            Assert.Equal("Database save was not successful.", exception.Message);
        }
    }
}
