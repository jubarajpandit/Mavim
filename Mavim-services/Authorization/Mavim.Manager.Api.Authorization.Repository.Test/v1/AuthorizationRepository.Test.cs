using FluentAssertions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.Authorization.Repository.v1;
using Mavim.Manager.Api.Authorization.Repository.v1.Model;
using Mavim.Manager.Authorization.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Authorization.Repository.Test.v1
{
    public class AuthorizationRepositoryTest
    {
        private static readonly Guid UserId1 = Guid.NewGuid();
        private static readonly Guid UserId2 = Guid.NewGuid();
        private static readonly Guid UserId3 = Guid.NewGuid();

        private static readonly string Email1 = "test1@mavim.nl";
        private static readonly string Email2 = "test2@mavim.nl";
        private static readonly string Email3 = "test3@mavim.nl";

        private static readonly Guid TenantId = Guid.NewGuid();

        private List<Manager.Authorization.DbModel.User> GetUsersFromContext() => new List<Manager.Authorization.DbModel.User>()
            {
                new Manager.Authorization.DbModel.User()
                {
                    Id = UserId1,
                    Email = Email1,
                    Role = Manager.Authorization.DbModel.UserRole.Subscriber,
                    TenantId = TenantId
                },
                new Manager.Authorization.DbModel.User()
                {
                    Id = UserId2,
                    Email = Email2,
                    Role = Manager.Authorization.DbModel.UserRole.Contributor,
                    TenantId = TenantId
                },
                new Manager.Authorization.DbModel.User()
                {
                    Id = UserId3,
                    Email = Email3,
                    Role = Manager.Authorization.DbModel.UserRole.Administrator,
                    TenantId = TenantId
                },
            };

        private List<IUser> GetUsersFromRepo() => new List<IUser>()
            {
                new User()
                {
                    Id = UserId1,
                    Email = Email1,
                    Role = UserRole.Subscriber,
                    TenantId = TenantId
                },
                new User()
                {
                    Id = UserId2,
                    Email = Email2,
                    Role = UserRole.Contributor,
                    TenantId = TenantId
                },
                new User()
                {
                    Id = UserId3,
                    Email = Email3,
                    Role = UserRole.Administrator,
                    TenantId = TenantId
                },
            };

        #region GetUser
        [Fact]
        [Trait("Authorization Repository", "GetUser")]
        public async Task GetUserByEmail_ValidArguments_UserObject()
        {
            //Arrange
            var user = GetUsersFromContext().First(u => u.Email == Email1);
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            var result = await repo.GetUser(Email1, TenantId);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(user);
            await context.DisposeAsync();
        }

        [Fact]
        [Trait("Authorization Repository", "GetUser")]
        public async Task GetUserByID_ValidArguments_UserObject()
        {
            //Arrange
            var user = GetUsersFromContext().First(u => u.Email == Email1);
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            var result = await repo.GetUser(UserId1, TenantId);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(user);
            await context.DisposeAsync();
        }

        [Fact]
        [Trait("Authorization Repository", "GetUser")]
        public async Task GetUserByID_NotExistingUser_Null()
        {
            //Arrange
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext(withData: false);
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            var result = await repo.GetUser(Guid.Empty, TenantId);

            //Assert
            Assert.Null(result);
            await context.DisposeAsync();
        }

        [Fact]
        [Trait("Authorization Repository", "GetUser")]
        public async Task GetUser_NotExistingArgument_Null()
        {
            //Arrange
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext(withData: false);
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            var result = await repo.GetUser(Email1, TenantId);

            //Assert
            Assert.Null(result);
            await context.DisposeAsync();
        }
        #endregion

        #region GetUsers
        [Fact]
        [Trait("Authorization Repository", "GetUsers")]
        public async Task GetUsers_ValidArguments_UserList()
        {
            //Arrange
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            var result = await repo.GetUsers(TenantId);

            //Assert
            Assert.NotNull(result);
            result.ToList().ForEach(i =>
            {
                var item = GetUsersFromRepo().FirstOrDefault(b => b.Email == i.Email);
                i.Should().BeEquivalentTo(item);
            });

            await context.DisposeAsync();
        }
        #endregion

        #region AddUsers
        [Fact]
        [Trait("Authorization Repository", "AddUsers")]
        public async Task AddUsers_ValidArguments_UserList()
        {
            //Arrange
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext(withData: false);
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            var result = await repo.AddUsers(GetUsersFromRepo());

            //Assert
            Assert.NotNull(result);
            Assert.True(context.Users.AsNoTracking().Count(i => i.TenantId == TenantId) == 3);
            result.ToList().ForEach(i =>
            {
                var item = GetUsersFromRepo().FirstOrDefault(b => b.Email == i.Email);
                Assert.True(i.Id != item.Id);
                Assert.True(i.TenantId == item.TenantId);
                Assert.True(i.Email == item.Email);
                Assert.True(i.Role == item.Role);
            });

            await context.DisposeAsync();
        }


        [Fact]
        [Trait("Authorization Repository", "AddUsers")]
        public async Task AddUsers_ListWithSameUsers_SingleUser()
        {
            //Arrange
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext(withData: false);
            var repo = new AuthorizationRepository(context, logger.Object);
            var users = new List<IUser>() {
                GetUsersFromRepo().First(),
                GetUsersFromRepo().First(),
                GetUsersFromRepo().First()
            };

            //Act
            var result = await repo.AddUsers(users);

            //Assert
            Assert.NotNull(result);
            Assert.True(context.Users.AsNoTracking().Count(i => i.TenantId == TenantId) == 1);

            await context.DisposeAsync();
        }

        [Fact]
        [Trait("Authorization Repository", "AddUsers")]
        public async Task AddUsers_ExistingUsers_EmptyArray()
        {
            //Arrange
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            var result = await repo.AddUsers(GetUsersFromRepo());

            //Assert
            Assert.Empty(result);

            await context.DisposeAsync();
        }

        [Fact]
        [Trait("Authorization Repository", "AddUsers")]
        public async Task AddUsers_InvalidNullArgument_BadRequestException()
        {
            //Arrange
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repo.AddUsers(null));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);

            await context.DisposeAsync();
        }

        [Fact]
        [Trait("Authorization Repository", "AddUsers")]
        public async Task AddUsers_EmptyGuid_BadRequestException()
        {
            //Arrange
            var users = GetUsersFromRepo().Select(u => { u.TenantId = Guid.Empty; return u; });
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() => repo.AddUsers(users));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<BadRequestException>(exception);

            await context.DisposeAsync();
        }
        #endregion

        #region EditUser
        [Fact]
        [Trait("Authorization Repository", "EditUser")]
        public async Task EditUser_ValidArguments_UserList()
        {
            //Arrange
            var user = GetUsersFromRepo().First();
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            await repo.EditUserRole(user.Id, user.TenantId, UserRole.Contributor);

            //Assert
            var dbUser = await context.Users.AsNoTracking().FirstOrDefaultAsync(u =>
                u.Id == user.Id
            );

            Assert.Equal(user.Id, dbUser.Id);
            Assert.Equal(Manager.Authorization.DbModel.UserRole.Contributor, dbUser.Role);
            await context.DisposeAsync();
        }

        [Fact]
        [Trait("Authorization Repository", "EditUser")]
        public async Task EditUser_NotExistingUSer_RequestNotFoundException()
        {
            //Arrange
            var user = GetUsersFromRepo().First();
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                    repo.EditUserRole(Guid.Empty, Guid.Empty, UserRole.Contributor));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);

            await context.DisposeAsync();
        }
        #endregion

        #region DeleteUser
        [Fact]
        [Trait("Authorization Repository", "DeleteUser")]
        public async Task DeleteUser_ValidArguments_UserList()
        {
            //Arrange
            var user = GetUsersFromRepo().First();
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            await repo.DeleteUser(user.Id, user.TenantId);

            //Assert
            var dbUser = await context.Users.AsNoTracking().FirstOrDefaultAsync(u =>
                u.Id == user.Id
            );

            Assert.Null(dbUser);
            await context.DisposeAsync();
        }
        [Fact]
        [Trait("Authorization Repository", "DeleteUser")]
        public async Task DeleteUser_NotExistingUSer_RequestNotFoundException()
        {
            //Arrange
            var user = GetUsersFromRepo().First();
            var logger = new Mock<ILogger<AuthorizationRepository>>();
            var context = await GetMockContext();
            var repo = new AuthorizationRepository(context, logger.Object);

            //Act
            Exception exception = await Record.ExceptionAsync(() =>
                    repo.DeleteUser(Guid.Empty, Guid.Empty));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<RequestNotFoundException>(exception);

            await context.DisposeAsync();
        }
        #endregion

        private async Task<AuthorizationDbContext> GetMockContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder<AuthorizationDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new AuthorizationDbContext(options);

            if (withData)
            {
                await context.AddRangeAsync(GetUsersFromContext());
                await context.SaveChangesAsync();
            }

            return context;
        }
    }
}
