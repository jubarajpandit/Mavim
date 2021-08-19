using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using DbRole = Mavim.Manager.Authorization.Read.Databases.Models.Role;
using Mavim.Manager.Authorization.Read.Databases;
using Mavim.Manager.Authorization.Read.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Mavim.Manager.Authorization.Read.Constants;

namespace Mavim.Manager.Authorization.Read.Commands
{
    public static class EnableRoleCommand
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

                EntityIsEnabled(roleId, role);

                DbRole dbRole = DisableRole(role);

                await SaveToDatabase(dbRole, cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static DbRole DisableRole(DbRole role) => role with
            {
                Disabled = true
            };
        }
    }
}
