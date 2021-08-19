using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using Mavim.Manager.Api.Authorization.Services.v1.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using IRepo = Mavim.Manager.Api.Authorization.Repository.Interfaces.v1;
using Repo = Mavim.Manager.Api.Authorization.Repository.v1;

namespace Mavim.Manager.Api.Authorization.Services.v1
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepo.Interface.IAuthorizationRepository _repository;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly IJwtSecurityToken _token;

        public AuthorizationService(
            IRepo.Interface.IAuthorizationRepository repository,
            ILogger<AuthorizationService> logger,
            IJwtSecurityToken token)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// Determines whether user [can edit] based on the authentication token.
        /// </summary>
        /// <returns></returns>
        public async Task<IUser> GetUser()
        {
            IRepo.Interface.IUser user = await _repository.GetUser(_token.Email, _token.TenantId);
            if (user == null)
                _logger.LogWarning($"User with email {_token.Email} is not found.");
            else
                _logger.LogDebug($"User with email {_token.Email} has role {user.Role.ToString()}.");

            return Map(user);
        }

        /// <summary>
        /// Get a list of all users based on tenantId
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IUser>> GetUsers()
        {
            await ValidateAdminRole();

            IEnumerable<IRepo.Interface.IUser> users = await _repository.GetUsers(_token.TenantId);

            return users.Select(Map);
        }

        /// <summary>
        /// Add users with specific roles
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IUser>> AddUsers(IEnumerable<IUser> users)
        {
            await ValidateAdminRole();

            if (users == null || !users.Any())
                throw new BadRequestException("Empty user list");
            List<IUser> userList = users.ToList();
            string domain = new MailAddress(_token.Email).Host;
            userList.ForEach(u =>
            {
                ValidateDomain(u.Email, domain);
            });

            userList.ForEach(EnrichWithTenant);
            IEnumerable<IRepo.Interface.IUser> addUsers = await _repository.AddUsers(userList.Select(Map));
            return addUsers.Select(Map);
        }

        /// <summary>
        /// Edit users role
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="role">The role.</param>
        public async Task EditUserRole(Guid userId, Role role)
        {
            await ValidateAdminRole(userId);

            await _repository.EditUserRole(userId, _token.TenantId, Map(role));
        }

        public async Task DeleteUser(Guid userId)
        {
            await ValidateAdminRole(userId);

            await _repository.DeleteUser(userId, _token.TenantId);
        }

        private async Task<IUser> ValidateAdminRole()
        {
            IUser authorizedUser = await GetUser();
            if (authorizedUser.Role != Role.Administrator)
                throw new ForbiddenRequestException($"Forbidden request for user roll {authorizedUser.Role}");

            return authorizedUser;
        }

        private async Task ValidateAdminRole(Guid userId)
        {
            IUser authorizedUser = await ValidateAdminRole();
            if (authorizedUser.Id == userId)
                throw new ForbiddenRequestException("Not allowed to edit or delete yourself");
        }

        private void ValidateDomain(string email, string domain)
        {
            if (!email.EndsWith(domain, StringComparison.OrdinalIgnoreCase))
                throw new BadRequestException($"{email} domain is not allowed. Please provide email addresses only with domain {domain}");
        }

        private void EnrichWithTenant(IUser user)
        {
            user.TenantId = _token.TenantId;
        }

        private static IUser Map(IRepo.Interface.IUser user) =>
            user == null ? null :
                new User()
                {
                    Id = user.Id,
                    Email = user.Email?.ToLower(),
                    TenantId = user.TenantId,
                    Role = Map(user.Role)
                };

        private static IRepo.Interface.IUser Map(IUser user) => new Repo.Model.User()
        {
            Id = user.Id,
            Email = user.Email?.ToLower(),
            TenantId = user.TenantId,
            Role = Map(user.Role)
        };

        private static Role Map(IRepo.Enum.UserRole role) => role switch
        {
            IRepo.Enum.UserRole.Subscriber => Role.Subscriber,
            IRepo.Enum.UserRole.Contributor => Role.Contributor,
            IRepo.Enum.UserRole.Administrator => Role.Administrator,
            _ => throw new Exception("Unknown role"),
        };

        private static IRepo.Enum.UserRole Map(Role role) => role switch
        {
            Role.Subscriber => IRepo.Enum.UserRole.Subscriber,
            Role.Contributor => IRepo.Enum.UserRole.Contributor,
            Role.Administrator => IRepo.Enum.UserRole.Administrator,
            _ => throw new Exception("Unknown role"),
        };

    }
}