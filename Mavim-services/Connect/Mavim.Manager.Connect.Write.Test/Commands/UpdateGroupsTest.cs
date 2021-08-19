using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Commands;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using Microsoft.Azure.ServiceBus;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Test.Commands
{
    public class UpdateGroupTest
    {
        [Theory]
        [InlineData("NewGroupName", null)]
        [InlineData(null, "NewGroupDescription")]
        [InlineData("NewGroupName", "NewGroupDescription")]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_ValidArguments_ValidModelStateAsync(string Name, string Description)
        {
            // Arrange
            var eventSourcing = new Mock<IEventSourcingGeneric<GroupV1>>();

            eventSourcing.Setup(es => es.GetEvents(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<EventSourcingModel>() { MockData.EmptyMock });

            eventSourcing.Setup(es => es.PlayEvents(It.IsAny<IReadOnlyList<EventSourcingModel>>()))
                .ReturnsAsync(() => new GroupV1() { IsActive = true });

            eventSourcing.Setup(es => es.UpdatePayload(It.IsAny<GroupV1>(), It.IsAny<GroupV1>()))
                .ReturnsAsync((GroupV1 entity, GroupV1 payload) => payload);

            eventSourcing.Setup(es => es.CreateUpdateEvent(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<GroupV1>(), It.IsAny<int>()))
                .Returns(MockData.EmptyMock);

            eventSourcing.Setup(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            eventSourcing.Setup(es => es.SaveEvent(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);


            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var handler = new UpdateGroup.Handler(eventSourcing.Object, identity, serviceBus.Object);
            var command = new UpdateGroup.Command(identity.GroupId, Name, Description);
            var cancellationToken = new CancellationToken();

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            eventSourcing.Verify(es => es.UpdatePayload(It.IsAny<GroupV1>(), It.IsAny<GroupV1>()), Times.Once);
            eventSourcing.Verify(es => es.CreateUpdateEvent(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<GroupV1>(), It.IsAny<int>()), Times.Once);
            eventSourcing.Verify(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()), Times.Once);
            eventSourcing.Verify(es => es.SaveEvent(It.IsAny<CancellationToken>()), Times.Once);
            serviceBus.Verify(sb => sb.SendAsync(It.IsAny<Message>()), Times.Once);

            Assert.IsType<MediatR.Unit>(result);
        }

        [Theory]
        [InlineData(null, null, "Please update at least one property")]
        [InlineData("", null, "Empty name property is not allowed")]
        [InlineData(" ", null, "Empty name property is not allowed")]
        public async Task UpdateGroup_InValidArguments_BadRequestException(string Name, string Description, string errorText)
        {
            // Arrange
            var eventSourcing = new Mock<IEventSourcingGeneric<GroupV1>>();

            eventSourcing.Setup(es => es.GetEvents(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<EventSourcingModel>() { MockData.EmptyMock });

            eventSourcing.Setup(es => es.PlayEvents(It.IsAny<IReadOnlyList<EventSourcingModel>>()))
                .ReturnsAsync(() => new GroupV1() { IsActive = true });

            eventSourcing.Setup(es => es.UpdatePayload(It.IsAny<GroupV1>(), It.IsAny<GroupV1>()))
                .ReturnsAsync((GroupV1 entity, GroupV1 payload) => entity);

            eventSourcing.Setup(es => es.CreateUpdateEvent(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<GroupV1>(), It.IsAny<int>()))
                .Returns(MockData.EmptyMock);

            eventSourcing.Setup(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            eventSourcing.Setup(es => es.SaveEvent(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var handler = new UpdateGroup.Handler(eventSourcing.Object, identity, serviceBus.Object);
            var command = new UpdateGroup.Command(identity.GroupId, Name, Description);
            var cancellationToken = new CancellationToken();

            // Act
            Exception exception = await Record.ExceptionAsync(() => handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
            Assert.Equal(errorText, exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_InvalidEventSourcingGenericDependencyInjection_ArgumentNullException()
        {
            // Arrange
            IEventSourcingGeneric<GroupV1> eventSourcing = null;

            var cancellationToken = new CancellationToken();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateGroup.Handler(eventSourcing, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'eventSourcing')", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_InvalidIdentityDependencyInjection_ArgumentNullException()
        {
            // Arrange
            var eventSourcing = new GroupV1EventSourcing(new Database.ConnectDbContext());

            var cancellationToken = new CancellationToken();
            IdentityService identity = null;

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateGroup.Handler(eventSourcing, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'identity')", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_InvalidCommandParameter_ArgumentNullException()
        {
            // Arrange
            var eventSourcing = new GroupV1EventSourcing(new Database.ConnectDbContext());

            var cancellationToken = new CancellationToken();
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateGroup.Handler(eventSourcing, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'request')", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateGroupCommand Write")]
        public async Task UpdateGroup_DatabaseFailed_Exception()
        {
            // Arrange
            var eventSourcing = new Mock<IEventSourcingGeneric<GroupV1>>();

            eventSourcing.Setup(es => es.GetEvents(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<EventSourcingModel>() { MockData.EmptyMock });

            eventSourcing.Setup(es => es.PlayEvents(It.IsAny<IReadOnlyList<EventSourcingModel>>()))
                .ReturnsAsync(() => new GroupV1() { IsActive = true });

            eventSourcing.Setup(es => es.UpdatePayload(It.IsAny<GroupV1>(), It.IsAny<GroupV1>()))
                .ReturnsAsync((GroupV1 entity, GroupV1 payload) => payload);

            eventSourcing.Setup(es => es.CreateUpdateEvent(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<GroupV1>(), It.IsAny<int>()))
                .Returns(MockData.EmptyMock);

            eventSourcing.Setup(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            eventSourcing.Setup(es => es.SaveEvent(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var handler = new UpdateGroup.Handler(eventSourcing.Object, identity, serviceBus.Object);
            var command = new UpdateGroup.Command(identity.GroupId, "Name", null);
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