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
    public static class AddRoleCommand
    {
        // Command
        public record Command(Guid Id, string Name, IReadOnlyList<Guid> Groups, IReadOnlyList<Guid> TopicPermissions, Guid CompanyId, int AggregateId) : IRequest;

        // Handler
        public class Handler : BaseRoleCommand, IRequestHandler<Command, Unit>
        {
            public Handler(AuthorizationDatabaseContext dbContext) : base(dbContext)
            { }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var (roleId, _, _, _, companyId, aggregateId) = request;

                ValidateIfAggregateIdIsZero(aggregateId);

                await RoleShouldNotExists(roleId, companyId, cancellationToken);

                var dbRole = Map(request);

                await SaveToDatabase(dbRole, cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static DbRole Map(Command role)
            {
                (Guid id, string name, IReadOnlyList<Guid> groups, IReadOnlyList<Guid> topicPermissions, Guid companyId, int aggregateId) = role;
                return new DbRole(id, aggregateId, name, groups.ToArray(), topicPermissions.ToArray(), companyId, false, DateTime.Now);
            }

        }
    }
}
