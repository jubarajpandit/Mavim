using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.Authorization.Repository.v1.Model;
using Mavim.Manager.Authorization.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel = Mavim.Manager.Authorization.DbModel;

namespace Mavim.Manager.Api.Authorization.Repository.v1
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        public readonly AuthorizationDbContext _dbContext;
        private readonly ILogger<AuthorizationRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// dbContext
        /// or
        /// logger
        /// </exception>
        public AuthorizationRepository(AuthorizationDbContext dbContext, ILogger<AuthorizationRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Determines whether this instance can edit the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<IUser> GetUser(string email, Guid tenantId)
        {
            _logger.LogTrace("Retrieving authorization configuration for user.");
            DbModel.User result = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u =>
                u.Email == email
                && u.TenantId == tenantId
            );

            if (result == null)
            {
                _logger.LogDebug($"No user authorization results found for user with id: {email} and tenantId {tenantId}.");
                return null;
            }
            _logger.LogDebug($"Found authorization for user with id: {email}.");

            return Map(result);
        }


        /// <summary>
        /// Determines whether this instance can edit the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<IUser> GetUser(Guid userId, Guid tenantId)
        {
            _logger.LogTrace("Retrieving authorization configuration for user.");
            DbModel.User result = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u =>
                u.Id == userId
                && u.TenantId == tenantId
            );

            if (result == null)
            {
                _logger.LogDebug($"No user authorization results found for user with id: { userId } and tenantId {tenantId}.");
                return null;
            }
            _logger.LogDebug($"Found authorization for user with id: { userId }.");

            return Map(result); ;
        }

        /// <summary>
        /// Get all users role by tenantId
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IUser>> GetUsers(Guid tenantId)
        {
            var result = await _dbContext.Users.AsNoTracking().Where(u => u.TenantId == tenantId).ToListAsync();
            return result.Select(Map);
        }

        public async Task<IEnumerable<IUser>> AddUsers(IEnumerable<IUser> users)
        {
            if (users is null)
                throw new ArgumentNullException(nameof(users));

            #region BusinessLogic
            Guid tenantID = users.FirstOrDefault().TenantId;

            if (tenantID == Guid.Empty)
                throw new BadRequestException("Invalid TenantId, id is empty");

            users = DistinctUsers(users);

            List<string> userEmails = users.Select(u => u.Email).ToList();

            List<DbModel.User> usersInDb = await GetUsersBasedOnListOfEmails(tenantID, userEmails);

            List<string> userIdsInDb = usersInDb.Select(u => u.Email).ToList();

            List<IUser> usersNotInDb = users.Where(u => !userIdsInDb.Contains(u.Email)).ToList();

            IEnumerable<DbModel.User> databaseModel = usersNotInDb.Select(Map);
            #endregion

            await _dbContext.Users.AddRangeAsync(databaseModel);

            await _dbContext.SaveChangesAsync();

            usersInDb.ForEach(u => _logger.LogWarning($"Email {u.Email} already exists in database"));

            List<string> addedUsersEmail = usersNotInDb.Select(i => i.Email).ToList();

            List<DbModel.User> addedUsers = await GetUsersBasedOnListOfEmails(tenantID, addedUsersEmail);

            return addedUsers.Select(Map);
        }

        public async Task EditUserRole(Guid userId, Guid tenantId, UserRole role)
        {
            DbModel.User usersInDb = await GetUserByIdWithTracking(userId, tenantId);

            usersInDb.Role = Map(role);

            _dbContext.Users.Update(usersInDb);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteUser(Guid userId, Guid tenantId)
        {
            DbModel.User usersInDb = await GetUserByIdWithTracking(userId, tenantId);

            _dbContext.Users.Remove(usersInDb);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<DbModel.User> GetUserByIdWithTracking(Guid userId, Guid tenantId)
        {
            DbModel.User usersInDb = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Id == userId
                && u.TenantId == tenantId
            );

            if (usersInDb == null)
                throw new RequestNotFoundException($"User id: {userId} does not exists.");

            return usersInDb;
        }

        /// <summary>
        /// Maps the specified model DB user to a repo user.
        /// </summary>
        /// <param name="modelUser">The model user.</param>
        /// <returns></returns>
        private static IUser Map(DbModel.User modelUser) => new User
        {
            Id = modelUser.Id,
            Email = modelUser.Email?.ToLower(),
            TenantId = modelUser.TenantId,
            Role = Map(modelUser.Role)
        };

        /// <summary>
        /// Maps the specified model repo user to a Db user.
        /// </summary>
        /// <param name="repoUser"></param>
        /// <returns></returns>
        private static DbModel.User Map(IUser repoUser) => new DbModel.User
        {
            // don't add id because this will auto generate by the database
            Email = repoUser.Email?.ToLower(),
            TenantId = repoUser.TenantId,
            Role = Map(repoUser.Role)
        };

        private static DbModel.UserRole Map(UserRole role) => role switch
        {
            UserRole.Subscriber => DbModel.UserRole.Subscriber,
            UserRole.Contributor => DbModel.UserRole.Contributor,
            UserRole.Administrator => DbModel.UserRole.Administrator,
            _ => throw new Exception("Unknown role"),
        };

        private static UserRole Map(DbModel.UserRole role) => role switch
        {
            DbModel.UserRole.Subscriber => UserRole.Subscriber,
            DbModel.UserRole.Contributor => UserRole.Contributor,
            DbModel.UserRole.Administrator => UserRole.Administrator,
            _ => throw new Exception("Unknown role"),
        };



        private IEnumerable<IUser> DistinctUsers(IEnumerable<IUser> users)
        {
            List<IUser> userList = new List<IUser>();
            users.ToList().ForEach(user =>
            {
                if (userList.FirstOrDefault(ul => ul.Email?.ToLower() == user.Email?.ToLower()) == null)
                    userList.Add(user);
            });

            return userList;
        }

        private async Task<List<DbModel.User>> GetUsersBasedOnListOfEmails(Guid tenantID, List<string> userIds)
        {
            return await _dbContext.Users.AsNoTracking().Where(u =>
                userIds.Contains(u.Email)
                && u.TenantId == tenantID
            ).ToListAsync();
        }

    }
}