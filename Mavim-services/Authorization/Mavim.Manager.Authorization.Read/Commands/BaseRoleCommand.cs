using System;
using System.Threading;
using System.Threading.Tasks;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Authorization.Read.Constants;
using Mavim.Manager.Authorization.Read.Databases;
using Microsoft.EntityFrameworkCore;
using DbRole = Mavim.Manager.Authorization.Read.Databases.Models.Role;

namespace Mavim.Manager.Authorization.Read.Commands
{
    public abstract class BaseRoleCommand
    {
        private readonly AuthorizationDatabaseContext _dbContext;

        public BaseRoleCommand(AuthorizationDatabaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        protected async Task<DbRole> GetRole(Guid roleId, Guid companyId, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
            if (role is null) throw new RequestNotFoundException(string.Format(Logging.ROLE_NOT_FOUND, roleId));
            return role;
        }

        protected async Task RoleShouldNotExists(Guid roleId, Guid companyId, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
            if (role is not null) throw new UnprocessableEntityException(string.Format(Logging.ROLE_ALREADY_EXISTS, roleId));
        }

        protected async Task SaveToDatabase(DbRole dbRole, CancellationToken cancellationToken)
        {
            _dbContext.Roles.Update(dbRole);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        protected static void ValidateIfAggregateIdIsZero(int aggregateId)
        {
            if (aggregateId != 0)
                throw new UnprocessableEntityException(
                    string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, 0),
                    aggregateId > 0 ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                    );
        }

        protected static void ValidateAggregateId(int aggregateId, DbRole role)
        {
            var expectedAggregateId = role.AggregateId + 1;
            if (aggregateId != expectedAggregateId)
                throw new UnprocessableEntityException(
                    string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, expectedAggregateId),
                    aggregateId > expectedAggregateId ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                    );
        }

        protected static void EntityIsEnabled(Guid roleId, DbRole role)
        {
            if (role.Disabled) throw new UnprocessableEntityException($"Role with id: {roleId} is already disabled");
        }
        protected static void EntityIsDisabled(Guid roleId, DbRole role)
        {
            if (!role.Disabled) throw new UnprocessableEntityException($"Role with id: {roleId} is already enabled");
        }
    }
}
