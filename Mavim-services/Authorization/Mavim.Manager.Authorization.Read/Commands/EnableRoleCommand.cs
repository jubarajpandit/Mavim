using DbRole = Mavim.Manager.Authorization.Read.Databases.Models.Role;
using Mavim.Manager.Authorization.Read.Databases;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Authorization.Read.Commands
{
    public static class DisableRoleCommand
    {
        // Command
        public record Command(Guid Id, Guid CompanyId, int AggregateId) : IRequest;

        // Handler
        public class Handler : BaseRoleCommand, IRequestHandler<Command, Unit>
        {
            public Handler(AuthorizationDatabaseContext dbContext) : base(dbContext)
            { }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var (roleId, companyId, aggregateId) = request;

                var role = await GetRole(roleId, companyId, cancellationToken);

                ValidateAggregateId(aggregateId, role);

                EntityIsDisabled(roleId, role);

                DbRole dbRole = EnableRole(role);

                await SaveToDatabase(dbRole, cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static DbRole EnableRole(DbRole role) => role with
            {
                Disabled = false
            };
        }
    }
}
