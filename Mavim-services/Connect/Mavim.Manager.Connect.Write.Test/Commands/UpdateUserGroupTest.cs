using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Write.Commands;
using Mavim.Manager.Connect.Write.Database.Models;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.DomainModel.Interfaces;
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
    public class UpdateUserGroupTest
    {
        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var group = new GroupV1(Guid.NewGuid(), "GroupName", "GroupDescription", identity.CompanyId, new List<Guid> {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            }, IsActive: true);
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", identity.CompanyId, IsActive: true);
            var eventSourcing = InitEventSourcing(group);
            var eventSourcingUser = InitEventSourcing(user);

            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var handler = new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser.Object, identity, serviceBus.Object);
            var command = new UpdateUserGroup.Command(identity.GroupId, listOfUsers);
            var cancellationToken = new CancellationToken();



            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            eventSourcing.Verify(es => es.GetEvents(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            eventSourcing.Verify(es => es.PlayEvents(It.IsAny<IReadOnlyList<EventSourcingModel>>()), Times.Once);
            eventSourcing.Verify(es => es.AddPartialPayload(It.IsAny<GroupV1>(), It.IsAny<GroupV1>()), Times.Once);
            eventSourcing.Verify(es => es.CreateAddPartialEvent(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<GroupV1>(), It.IsAny<int>()), Times.Once);
            eventSourcing.Verify(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()), Times.Once);
            eventSourcing.Verify(es => es.SaveEvent(It.IsAny<CancellationToken>()), Times.Once);
            serviceBus.Verify(sb => sb.SendAsync(It.IsAny<Message>()), Times.Once);

            Assert.IsType<MediatR.Unit>(result);
        }

        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_InvalidGroupEventSourcingDependencyInjection_ArgumentNullException()
        {
            // Arrange
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", identity.CompanyId, IsActive: true);
            EventSourcingGeneric<GroupV1> eventSourcing = null;
            var eventSourcingUser = InitEventSourcing(user);

            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = new UpdateUserGroup.Command(identity.GroupId, listOfUsers);
            var cancellationToken = new CancellationToken();

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing, eventSourcingUser.Object, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'eventSourcing')", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_PayloadDitNotChange_BadRequestException()
        {
            // Arrange
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var group = new GroupV1(Guid.NewGuid(), "GroupName", "GroupDescription", identity.CompanyId, new List<Guid> {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            }, IsActive: true);
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", identity.CompanyId, IsActive: true);
            var eventSourcing = InitEventSourcing(group);
            var eventSourcingUser = InitEventSourcing(user);

            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            var command = new UpdateUserGroup.Command(identity.GroupId, listOfUsers);
            var cancellationToken = new CancellationToken();

            eventSourcing.Setup(es => es.AddPartialPayload(It.IsAny<GroupV1>(), It.IsAny<GroupV1>()))
            .ReturnsAsync(() => new GroupV1() with { UserIds = new List<Guid> { } });

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser.Object, identity, serviceBus.Object)?.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<UnprocessableEntityException>(exception);
            Assert.Equal("Properties to update contains the same results", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_InvalidUserEventSourcingDependencyInjection_ArgumentNullException()
        {
            // Arrange
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", identity.CompanyId, IsActive: true);
            var eventSourcing = InitEventSourcing(new GroupV1());
            IEventSourcingGeneric<UserV1> eventSourcingUser = null;

            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = new UpdateUserGroup.Command(identity.GroupId, listOfUsers);
            var cancellationToken = new CancellationToken();

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'userEventSourcing')", exception.Message);
        }


        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_InvalidIdentityDependencyInjection_ArgumentNullException()
        {
            // Arrange
            IdentityService identity = null;
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", Guid.NewGuid(), IsActive: true);
            var eventSourcing = InitEventSourcing(new GroupV1());
            var eventSourcingUser = InitEventSourcing(user);

            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = new UpdateUserGroup.Command(Guid.NewGuid(), listOfUsers);
            var cancellationToken = new CancellationToken();

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser.Object, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'identity')", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_InvalidCommandParametern_ArgumentNullException()
        {
            // Arrange
            IdentityService identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", Guid.NewGuid(), IsActive: true);
            var eventSourcing = InitEventSourcing(new GroupV1());
            var eventSourcingUser = InitEventSourcing(user);

            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = new UpdateUserGroup.Command(Guid.NewGuid(), listOfUsers);
            var cancellationToken = new CancellationToken();

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser.Object, identity, serviceBus.Object)?.Handle(null, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'request')", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_GroupDoesNotExists_RequestNotFoundException()
        {
            // Arrange
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var group = new GroupV1(Guid.NewGuid(), "GroupName", "GroupDescription", identity.CompanyId, new List<Guid> {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            }, IsActive: false);
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", identity.CompanyId, IsActive: true);
            var eventSourcing = InitEventSourcing(group);
            var eventSourcingUser = InitEventSourcing(user);

            eventSourcing.Setup(es => es.DoesEntitiesExists(It.IsAny<IReadOnlyList<Guid>>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => false);


            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var command = new UpdateUserGroup.Command((Guid)group.Id, listOfUsers);
            var cancellationToken = new CancellationToken();

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser.Object, identity, serviceBus.Object)?.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal($"Group with ID {group.Id} does not exists", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_UserDoesNotExists_RequestNotFoundException()
        {
            // Arrange
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var group = new GroupV1(Guid.NewGuid(), "GroupName", "GroupDescription", identity.CompanyId, new List<Guid> {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            }, IsActive: true);
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", identity.CompanyId, IsActive: true);
            var eventSourcing = InitEventSourcing(group);
            var eventSourcingUser = InitEventSourcing(user);

            eventSourcingUser.Setup(es => es.DoesEntitiesExists(It.IsAny<IReadOnlyList<Guid>>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => false);


            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var command = new UpdateUserGroup.Command(identity.GroupId, listOfUsers);
            var cancellationToken = new CancellationToken();

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser.Object, identity, serviceBus.Object)?.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);
            Assert.Equal("Unable to add an non-existing userId", exception.Message);
        }

        [Fact]
        [Trait("Category", "Connect UpdateUserGroupCommand Write")]
        public async Task UpdateUserGroup_DatabaseFailed_Exception()
        {
            // Arrange
            var identity = new IdentityService(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var group = new GroupV1(Guid.NewGuid(), "GroupName", "GroupDescription", identity.CompanyId, new List<Guid> {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            }, IsActive: true);
            var user = new UserV1(Guid.NewGuid(), "test@mavim.com", identity.CompanyId, IsActive: true);
            var eventSourcing = InitEventSourcing(group);
            eventSourcing.Setup(es => es.SaveEvent(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(false));

            var eventSourcingUser = InitEventSourcing(user);

            var serviceBus = new Mock<IQueueClient>();
            serviceBus.Setup(sb => sb.SendAsync(It.IsAny<Message>()));

            var listOfUsers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = new UpdateUserGroup.Command(identity.GroupId, listOfUsers);
            var cancellationToken = new CancellationToken();

            // Act
            Exception exception = await Record.ExceptionAsync(() => new UpdateUserGroup.Handler(eventSourcing.Object, eventSourcingUser.Object, identity, serviceBus.Object)?.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
            Assert.Equal("Database save was not successful.", exception.Message);
        }

        private static Mock<IEventSourcingGeneric<T>> InitEventSourcing<T>(T entity) where T : IEventModel
        {
            var eventSourcing = new Mock<IEventSourcingGeneric<T>>();

            eventSourcing.Setup(es => es.GetEvents(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<EventSourcingModel>() { MockData.EmptyMock });

            eventSourcing.Setup(es => es.PlayEvents(It.IsAny<IReadOnlyList<EventSourcingModel>>()))
                .ReturnsAsync(() => entity);

            eventSourcing.Setup(es => es.AddPartialPayload(It.IsAny<T>(), It.IsAny<T>()))
                .ReturnsAsync((T entity, T payload) => payload);

            eventSourcing.Setup(es => es.DoesEntitiesExists(It.IsAny<IReadOnlyList<Guid>>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => true);

            eventSourcing.Setup(es => es.CreateAddPartialEvent(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<T>(), It.IsAny<int>()))
                .Returns(MockData.EmptyMock);

            eventSourcing.Setup(es => es.AddEventToDatabase(It.IsAny<EventSourcingModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            eventSourcing.Setup(es => es.SaveEvent(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            return eventSourcing;
        }
    }
}