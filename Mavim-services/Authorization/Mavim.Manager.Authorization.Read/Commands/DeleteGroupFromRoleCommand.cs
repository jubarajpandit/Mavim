using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using DbRole = Mavim.Manager.Authorization.Read.Databases.Models.Role;
using Mavim.Manager.Authorization.Read.Constants;
using Mavim.Manager.Authorization.Read.Databases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Authorization.Read.Commands
{
    public static class DeleteGroupFromRoleCommand
    {
        // Command
        public record Command(Guid Id, IReadOnlyList<Guid> Groups, Guid CompanyId, int AggregateId) : IRequest;

        // Handler
        public class Handler : BaseRoleCommand, IRequestHandler<Command, Unit>
        {
            public Handler(AuthorizationDatabaseContext dbContext) : base(dbContext)
            { }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var (roleId, groupIds, companyId, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                DbRole role = await GetRole(roleId, companyId, cancellationToken);

                ValidateAggregateId(aggregateId, role);

                var dbRole = RemoveGroupsFromRole(role, groupIds, aggregateId);

                await SaveToDatabase(dbRole, cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static DbRole RemoveGroupsFromRole(DbRole role, IEnumerable<Guid> groupIds, int aggregateId)
            {
                var updatedGroups = role.Groups.Except(groupIds).ToArray();
                return role with
                {
                    Groups = updatedGroups,
                    AggregateId = aggregateId,
                    LastUpdated = DateTime.Now
                };
            }
        }
    }
}
