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
    public static class UpdateRoleCommand
    {
        // Command
        public record Command(Guid Id, string Name, Guid CompanyId, int AggregateId) : IRequest;

        // Handler
        public class Handler : BaseRoleCommand, IRequestHandler<Command, Unit>
        {
            public Handler(AuthorizationDatabaseContext dbContext) : base(dbContext)
            { }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var (roleId, name, companyId, aggregateId) = request;

                var role = await GetRole(roleId, companyId, cancellationToken);

                ValidateAggregateId(aggregateId, role);

                DbRole dbRole = UpdateRole(role, name);

                await SaveToDatabase(dbRole, cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static DbRole UpdateRole(DbRole role, string name) => role with
            {
                Name = name
            };
        }
    }
}
