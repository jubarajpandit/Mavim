using FluentAssertions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using Mavim.Manager.Api.Authorization.Services.v1;
using Mavim.Manager.Api.Authorization.Services.v1.Model;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ILibrary = Mavim.Libraries.Authorization.Interfaces;
using IRepo = Mavim.Manager.Api.Authorization.Repository.Interfaces.v1;
using Library = Mavim.Libraries.Authorization.Models;
using Repo = Mavim.Manager.Api.Authorization.Repository.v1.Model;

namespace Mavim.Manager.Api.Authorization.Services.Test.v1
{
    public class AuthorizationServiceTest
    {
        private static readonly Guid AppId = Guid.NewGuid();

        private static readonly Guid UserId1 = Guid.NewGuid();
        private static readonly Guid UserId2 = Guid.NewGuid();
        private static readonly Guid UserId3 = Guid.NewGuid();

        private static readonly string Email1 = "test1@mavim.nl";
        private static readonly string Email2 = "test2@mavim.nl";
        private static readonly string Email3 = "test3@mavim.nl";

        private static readonly Guid TenantId = Guid.NewGuid();

        private ILibrary.IJwtSecurityToken GetSecurityToken(bool isAdmin = false) => new Library.JwtToken
        {
            AppId = AppId,
            Email = isAdmin ? Email3 : Email1,
            Name = "Test User",
            UserID = isAdmin ? UserId3 : UserId1,
            TenantId = TenantId,
            Token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken()
        };

        private List<IRepo.Interface.IUser> GetUsersFromRepo() => new List<IRepo.Interface.IUser>()
            {
                new Repo.User()
                {
                    Id = UserId1,
                    Email = Email1,
                    Role = IRepo.Enum.UserRole.Subscriber,
                    TenantId = TenantId
                },
                new Repo.User()
                {
                    Id = UserId2,
                    Email = Email2,
                    Role = IRepo.Enum.UserRole.Contributor,
                    TenantId = TenantId
                },
                new Repo.User()
                {
                    Id = UserId3,
                    Email = Email3,
                    Role = IRepo.Enum.UserRole.Administrator,
                    TenantId = TenantId
                },

            };

        private List<IUser> GetUsersFromService(bool validEmail = true) => new List<IUser>()
            {
                new User()
                {
                    Id = UserId1,
                    Email = validEmail ? Email1 : Email1.Replace("mavim.nl", "blabla.nl"),
                    Role = Role.Subscriber,
                    TenantId = TenantId
                },
                new User()
                {
                    Id = UserId2,
                    Email = validEmail ? Email2 : Email2.Replace("mavim.nl", "blabla.nl"),
                    Role = Role.Contributor,
                    TenantId = TenantId
                },
                new User()
                {
                    Id = UserId3,
                    Email = validEmail ? Email3 : Email3.Replace("mavim.nl", "blabla.nl"),
                    Role = Role.Administrator,
                    TenantId = TenantId
                },

            };
        public static IEnumerable<object[]> InvalidArgumentAddUsers => new List<object[]> {
                    new object[] { null },
                    new object[] { new List<User>() },
            };

        #region GetUser
        [Fact]
        [Trait("Authorization Service", "GetUsers")]
        public async Task GetUser_ValidArguments_UserObject()
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken());

            //Act
            var result = await service.GetUser();

            //Assert
            repo.Verify(mock => mock.GetUser(It.Is<string>(e => e == Email1), It.Is<Guid>(i => i == TenantId)), Times.Once);

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(GetUsersFromService().First());
        }

        [Fact]
        [Trait("Authorization Service", "GetUsers")]
        public async Task GetUser_ValidArguments_UserIsNull()
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => null);

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken());

            //Act
            var result = await service.GetUser();

            //Assert
            repo.Verify(mock => mock.GetUser(It.Is<string>(e => e == Email1), It.Is<Guid>(i => i == TenantId)), Times.Once);

            Assert.Null(result);
            result.Should().BeNull();
        }
        #endregion

        #region GetUsers
        [Fact]
        [Trait("Authorization Service", "GetUsers")]
        public async Task GetUsers_WithAdminUser_ListofUsers()
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.GetUsers(It.IsAny<Guid>())).ReturnsAsync(GetUsersFromRepo());
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();
            var requestedUser = GetSecurityToken(isAdmin: true);
            var service = new AuthorizationService(repo.Object, logger.Object, requestedUser);

            //Act
            var result = await service.GetUsers();

            //Assert
            repo.Verify(mock => mock.GetUsers(It.Is<Guid>(i => i == TenantId)), Times.Once);
            repo.Verify(mock => mock.GetUser(It.Is<string>(i => i == requestedUser.Email), It.Is<Guid>(i => i == TenantId)), Times.Once);

            Assert.NotNull(result);

            result.ToList().ForEach(i =>
            {
                var item = GetUsersFromService().FirstOrDefault(b => b.Email == i.Email);
                i.Should().BeEquivalentTo(item);
            });
        }

        [Fact]
        [Trait("Authorization Service", "GetUsers")]
        public async Task GetUsers_SubscriberUser_ForbiddenRequestException()
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.GetUsers(It.IsAny<Guid>())).ReturnsAsync(GetUsersFromRepo());
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();
            var requestedUser = GetSecurityToken(isAdmin: false);
            var service = new AuthorizationService(repo.Object, logger.Object, requestedUser);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.GetUsers());

            //Assert
            repo.Verify(mock => mock.GetUser(It.Is<string>(i => i == requestedUser.Email), It.Is<Guid>(i => i == TenantId)), Times.Once);

            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }
        #endregion

        #region AddUsers
        [Fact]
        [Trait("Authorization Service", "AddUsers")]
        public async Task AddUsers_ValidArgumentsWithAdminUser_ListofUsers()
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.AddUsers(It.IsAny<IEnumerable<IRepo.Interface.IUser>>())).ReturnsAsync(GetUsersFromRepo());
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                    .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: true));
            Func<IEnumerable<IRepo.Interface.IUser>, bool> validate = users =>
            {
                users.ToList().ForEach(i =>
                {
                    var item = GetUsersFromRepo().FirstOrDefault(b => b.Email == i.Email);
                    i.Should().BeEquivalentTo(item);
                });
                return true;
            };


            //Act
            var result = await service.AddUsers(GetUsersFromService());

            //Assert
            repo.Verify(mock => mock.AddUsers(It.Is<IEnumerable<IRepo.Interface.IUser>>(i => validate(i))), Times.Once);
            repo.Verify(mock => mock.GetUser(It.Is<string>(e => e == Email3), It.Is<Guid>(i => i == TenantId)), Times.Once);

            Assert.NotNull(result);

            result.ToList().ForEach(i =>
            {
                var item = GetUsersFromService().FirstOrDefault(b => b.Email == i.Email);
                i.Should().BeEquivalentTo(item);
            });
        }

        [Theory]
        [Trait("Authorization Service", "AddUsers")]
        [MemberData(nameof(InvalidArgumentAddUsers))]
        public async Task AddUsers_InValidArgumentsWithAdminUser_BadRequestException(List<User> users)
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.AddUsers(It.IsAny<IEnumerable<IRepo.Interface.IUser>>())).ReturnsAsync(GetUsersFromRepo());
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                    .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: true));


            //Act
            Exception exception = await Record.ExceptionAsync(() => service.AddUsers(users));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Fact]
        [Trait("Authorization Service", "AddUsers")]
        public async Task AddUsers_InValidEmailAddressWithAdminUser_BadRequestException()
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.AddUsers(It.IsAny<IEnumerable<IRepo.Interface.IUser>>())).ReturnsAsync(GetUsersFromRepo());
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
    .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: true));


            //Act
            Exception exception = await Record.ExceptionAsync(() => service.AddUsers(GetUsersFromService(validEmail: false)));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);
        }

        [Fact]
        [Trait("Authorization Service", "EditUsers")]
        public async Task AddUsers_UpdateWithSubscriber_ForbiddenRequestException()
        {
            //Arrange
            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));
            var logger = new Mock<ILogger<AuthorizationService>>();
            var requestedUser = GetSecurityToken(isAdmin: false);
            var service = new AuthorizationService(repo.Object, logger.Object, requestedUser);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.AddUsers(GetUsersFromService()));


            //Assert
            repo.Verify(mock => mock.GetUser(It.Is<string>(i => i == requestedUser.Email), It.Is<Guid>(i => i == TenantId)), Times.Once);

            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }
        #endregion

        #region EditUsers
        [Fact]
        [Trait("Authorization Service", "EditUsers")]
        public async Task EditUsers_ValidArgumentWithAdminUser_TaskComplete()
        {
            //Arrange
            var patchId = UserId1;
            var patchRole = Role.Administrator;
            var repoRole = IRepo.Enum.UserRole.Administrator;

            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.EditUserRole(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IRepo.Enum.UserRole>()));
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: true));

            //Act
            await service.EditUserRole(patchId, patchRole);

            //Assert
            repo.Verify(mock => mock.EditUserRole(
                It.Is<Guid>(i => i == patchId),
                It.Is<Guid>(t => t == TenantId),
                It.Is<IRepo.Enum.UserRole>(r => r == repoRole)),
               Times.Once);
        }

        [Fact]
        [Trait("Authorization Service", "EditUsers")]
        public async Task EditUsers_UpdateYourSelfWithAdminUser_ForbiddenRequestException()
        {
            //Arrange
            var patchId = UserId3;
            var patchRole = Role.Administrator;

            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));
            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: true));

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.EditUserRole(patchId, patchRole));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }

        [Fact]
        [Trait("Authorization Service", "EditUsers")]
        public async Task EditUsers_UpdateWithSubscriberUser_ForbiddenRequestException()
        {
            //Arrange
            var patchId = UserId1;
            var patchRole = Role.Administrator;

            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));
            var logger = new Mock<ILogger<AuthorizationService>>();
            var requestedUser = GetSecurityToken(isAdmin: false);
            var service = new AuthorizationService(repo.Object, logger.Object, requestedUser);

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.EditUserRole(patchId, patchRole));


            //Assert
            repo.Verify(mock => mock.GetUser(It.Is<string>(i => i == requestedUser.Email), It.Is<Guid>(i => i == TenantId)), Times.Once);

            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
        }
        #endregion

        #region DeleteUsers
        [Fact]
        [Trait("Authorization Service", "EditUsers")]
        public async Task DeleteUsers_ValidArgumentWithAdminUser_TaskComplete()
        {
            //Arrange
            var deleteId = UserId1;

            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();

            repo.Setup(r => r.DeleteUser(It.IsAny<Guid>(), It.IsAny<Guid>()));
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: true));

            //Act
            await service.DeleteUser(deleteId);

            //Assert
            repo.Verify(mock => mock.DeleteUser(
                It.Is<Guid>(i => i == deleteId),
                It.Is<Guid>(t => t == TenantId)),
               Times.Once);
        }

        [Fact]
        [Trait("Authorization Service", "EditUsers")]
        public async Task DeleteUsers_DeleteYourSelfWithAdminUser_ForbiddenRequestException()
        {
            //Arrange
            var deleteId = UserId3;

            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: true));

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.DeleteUser(deleteId));


            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
            Assert.Contains("Not allowed to edit or delete yourself", exception.Message);
        }

        [Fact]
        [Trait("Authorization Service", "EditUsers")]
        public async Task DeleteUsers_DeleteWithSubscriberUser_ForbiddenRequestException()
        {
            //Arrange
            var deleteId = UserId1;

            var repo = new Mock<IRepo.Interface.IAuthorizationRepository>();
            repo.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((string email, Guid tenantId) => GetUsersFromRepo().First(u => u.Email == email));

            var logger = new Mock<ILogger<AuthorizationService>>();

            var service = new AuthorizationService(repo.Object, logger.Object, GetSecurityToken(isAdmin: false));

            //Act
            Exception exception = await Record.ExceptionAsync(() => service.DeleteUser(deleteId));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ForbiddenRequestException>(exception);
            Assert.Contains($"Forbidden request for user roll {Role.Subscriber}", exception.Message);
        }
        #endregion
    }
}
