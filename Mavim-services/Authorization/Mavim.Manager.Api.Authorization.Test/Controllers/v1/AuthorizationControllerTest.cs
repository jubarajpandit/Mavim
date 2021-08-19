using FluentAssertions;
using Mavim.Manager.Api.Authorization.Controllers.v1;
using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Service = Mavim.Manager.Api.Authorization.Services.v1;

namespace Mavim.Manager.Api.Authorization.Test.Controllers.v1
{
    public class AuthorizationControllerTest
    {
        private static readonly Guid UserId1 = Guid.NewGuid();
        private static readonly Guid UserId2 = Guid.NewGuid();
        private static readonly Guid UserId3 = Guid.NewGuid();

        private static readonly string Email1 = "test1@mavim.nl";
        private static readonly string Email2 = "test2@mavim.nl";
        private static readonly string Email3 = "test3@mavim.nl";

        private static readonly Guid TenantId = Guid.NewGuid();

        private static List<User> GetListOfControllerUsers()
        {
            return new List<User>()
            {
                new User
                {
                    Email = Email1,
                    Role = Role.Subscriber
                },
                new User
                {
                    Email = Email2,
                    Role = Role.Contributor
                },
                new User
                {
                    Email = Email3,
                    Role = Role.Administrator
                },

            };
        }

        private static List<IUser> GetListOfControllerUsersMapToService()
        {
            return new List<IUser>()
            {
                new Service.Model.User
                {
                    Email = Email1,
                    Role = Role.Subscriber
                },
                new Service.Model.User
                {
                    Email = Email2,
                    Role = Role.Contributor
                },
                new Service.Model.User
                {
                    Email = Email3,
                    Role = Role.Administrator
                },

            };
        }

        private static List<IUser> GetListOfServiceUsers()
        {
            return new List<IUser>()
            {
                new Service.Model.User
                {
                    Id = UserId1,
                    Email = Email1,
                    TenantId = TenantId,
                    Role = Role.Subscriber
                },
                new Service.Model.User
                {
                    Id = UserId2,
                    Email = Email2,
                    TenantId = TenantId,
                    Role = Role.Contributor
                },
                new Service.Model.User
                {
                    Id = UserId3,
                    Email = Email3,
                    TenantId = TenantId,
                    Role = Role.Administrator
                },

            };
        }

        public static IEnumerable<object[]> EmptyArgumentsAndValidModelStateData => new List<object[]> {
                    new object[] { null },
                    new object[] { new List<User>() },
            };

        [Fact]
        [Trait("Authorization", "GetUsers")]
        public async Task GetUsers_ValidArguments_OkObjectResult()
        {
            //Arrange
            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.GetUsers()).ReturnsAsync(GetListOfServiceUsers);

            var logger = new Mock<ILogger<AuthorizationController>>();

            var controller = new AuthorizationController(service.Object, logger.Object);

            //Act
            var actionResult = await controller.GetAllUsers();

            //Assert
            service.Verify(mock => mock.GetUsers(), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IEnumerable<IUser>;
            Assert.NotNull(fieldsResult);
            Assert.True(fieldsResult.Count() == 3);
        }

        [Fact]
        [Trait("Authorization", "GetUser")]
        public async Task GetUser_ValidArguments_OkObjectResult()
        {
            //Arrange
            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.GetUser()).ReturnsAsync(GetListOfServiceUsers().First);

            var logger = new Mock<ILogger<AuthorizationController>>();

            var controller = new AuthorizationController(service.Object, logger.Object);

            //Act
            var actionResult = await controller.GetUser();

            //Assert
            service.Verify(mock => mock.GetUser(), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IUser;
            Assert.NotNull(fieldsResult);
        }

        [Fact]
        [Trait("Authorization", "AddUser")]
        public async Task AddUsers_ValidArguments_OkObjectResult()
        {
            //Arrange
            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.AddUsers(It.IsAny<IEnumerable<IUser>>())).ReturnsAsync(GetListOfServiceUsers());

            var logger = new Mock<ILogger<AuthorizationController>>();

            var controller = new AuthorizationController(service.Object, logger.Object);

            Func<IEnumerable<IUser>, bool> validate = users =>
            {
                users.ToList().ForEach(i =>
                {
                    var item = GetListOfControllerUsersMapToService().FirstOrDefault(b => b.Email == i.Email);
                    i.Should().BeEquivalentTo(item);
                });
                return true;
            };

            //Act
            var actionResult = await controller.AddUsers(GetListOfControllerUsers());

            //Assert
            service.Verify(mock => mock.AddUsers(It.Is<IEnumerable<IUser>>(x => validate(x))), Times.Once);

            Assert.NotNull(actionResult);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var fieldsResult = okObjectResult.Value as IEnumerable<IUser>;
            Assert.NotNull(fieldsResult);
            Assert.True(fieldsResult.Any());
        }

        [Theory]
        [MemberData(nameof(EmptyArgumentsAndValidModelStateData))]
        [Trait("Authorization", "AddUser")]
        public async Task AddUser_EmptyArgumentsAndValidModelState_BadRequestException(List<User> users)
        {
            //Arrange
            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.GetUser()).ReturnsAsync(GetListOfServiceUsers().First);

            var logger = new Mock<ILogger<AuthorizationController>>();

            var controller = new AuthorizationController(service.Object, logger.Object);
            controller.ModelState.Clear();


            //Act
            var actionResult = await controller.AddUsers(users);


            //Assert
            Assert.NotNull(actionResult);
            var BadRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(BadRequestObjectResult);
            Assert.Equal("Empty user list", BadRequestObjectResult.Value.ToString());
        }

        [Theory]
        [Trait("Authorization", "AddUser")]
        [InlineData("[0].role", "Invalid role")]
        [InlineData("[0].email", "Invalid email")]
        [InlineData("unknown", "Invalid user object")]
        public async Task AddUser_InvalidArgumentsAndInValidModelState_BadRequestException(string key, string errorMsg)
        {
            //Arrange
            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.GetUser()).ReturnsAsync(GetListOfServiceUsers().First);

            var logger = new Mock<ILogger<AuthorizationController>>();

            var controller = new AuthorizationController(service.Object, logger.Object);
            controller.ModelState.AddModelError(key, "error");


            //Act
            var actionResult = await controller.AddUsers(null);


            //Assert
            Assert.NotNull(actionResult);
            var BadRequestObjectResult = actionResult.Result as BadRequestObjectResult;
            Assert.NotNull(BadRequestObjectResult);
            Assert.Equal(errorMsg, BadRequestObjectResult.Value.ToString());
        }


        [Fact]
        [Trait("Authorization", "EditUser")]
        public async Task EditUsers_ValidArguments_OkObjectResult()
        {
            //Arrange

            var patchId = UserId1;
            var patchBody = new EmailPatchBody()
            {
                Role = Role.Administrator
            };

            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.EditUserRole(It.IsAny<Guid>(), It.IsAny<Role>()));

            var logger = new Mock<ILogger<AuthorizationController>>();
            var controller = new AuthorizationController(service.Object, logger.Object);

            //Act
            var actionResult = await controller.PatchUser(patchId, patchBody);

            //Assert
            service.Verify(mock => mock.EditUserRole(
                    It.Is<Guid>(u => u == UserId1),
                    It.Is<Role>(r => r == Role.Administrator)
                ),
                Times.Once
            );

            var okObjectResult = actionResult as OkResult;
            Assert.NotNull(okObjectResult);

        }

        [Theory]
        [Trait("Authorization", "EditUser")]
        [InlineData("id", "Invalid UserID")]
        [InlineData("role", "Invalid role")]
        [InlineData("unknown", "Invalid user object")]
        public async Task EditUser_InvalidArgumentsAndInValidModelState_BadRequestException(string key, string errorMsg)
        {
            //Arrange
            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.GetUser()).ReturnsAsync(GetListOfServiceUsers().First);

            var logger = new Mock<ILogger<AuthorizationController>>();

            var controller = new AuthorizationController(service.Object, logger.Object);
            controller.ModelState.AddModelError(key, "error");

            //Act
            var actionResult = await controller.PatchUser(Guid.Empty, null);

            //Assert
            Assert.NotNull(actionResult);
            var BadRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(BadRequestObjectResult);
            Assert.Equal(errorMsg, BadRequestObjectResult.Value.ToString());

        }



        [Fact]
        [Trait("Authorization", "DeleteUser")]
        public async Task DeleteUsers_ValidArguments_OkObjectResult()
        {
            //Arrange

            var patchId = UserId1;
            var patchBody = new EmailPatchBody()
            {
                Role = Role.Administrator
            };

            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.EditUserRole(It.IsAny<Guid>(), It.IsAny<Role>()));

            var logger = new Mock<ILogger<AuthorizationController>>();
            var controller = new AuthorizationController(service.Object, logger.Object);

            //Act
            var actionResult = await controller.DeleteUser(patchId);

            //Assert
            service.Verify(mock => mock.DeleteUser(
                    It.Is<Guid>(u => u == UserId1)
                ),
                Times.Once
            );

            var okObjectResult = actionResult as OkResult;
            Assert.NotNull(okObjectResult);

        }

        [Theory]
        [Trait("Authorization", "DeleteUser")]
        [InlineData("id", "Invalid UserID")]
        [InlineData("unknown", "Invalid user object")]
        public async Task DeleteUser_InvalidArgumentsAndInValidModelState_BadRequestException(string key, string errorMsg)
        {
            //Arrange
            var service = new Mock<IAuthorizationService>();

            service.Setup(service => service.GetUser()).ReturnsAsync(GetListOfServiceUsers().First);

            var logger = new Mock<ILogger<AuthorizationController>>();

            var controller = new AuthorizationController(service.Object, logger.Object);
            controller.ModelState.AddModelError(key, "error");

            //Act
            var actionResult = await controller.DeleteUser(Guid.Empty);

            //Assert
            Assert.NotNull(actionResult);
            var BadRequestObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(BadRequestObjectResult);
            Assert.Equal(errorMsg, BadRequestObjectResult.Value.ToString());

        }

    }

}
