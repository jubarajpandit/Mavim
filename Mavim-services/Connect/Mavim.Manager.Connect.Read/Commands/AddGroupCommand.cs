using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Databases.Models;
using Mavim.Manager.Connect.Read.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Commands
{
    public static class AddGroupCommand
    {
        // Command
        public record Command(Guid GroupId, string Name, string Description, Guid CompanyId, int ModelVersion, int AggregateId) : IRequest;

        // Handler
        public class Handler : BaseHandler, IRequestHandler<Command, Unit>
        {
            private readonly ConnectDatabaseContext _dbContext;

            public Handler(ConnectDatabaseContext dbContext)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var (groupId, _, _, _, _, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                if (aggregateId != 0)
                    throw new UnprocessableEntityException(
                        string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, 0),
                        aggregateId > 0 ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                        );

                var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
                if (group is not null) throw new UnprocessableEntityException(string.Format(Logging.GROUP_ALREADY_EXISTS, groupId));

                _dbContext.Groups.Add(Map(request));
                await _dbContext.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static GroupTable Map(Command command) => new(command.GroupId, Map(MapToGroupValue(command)), command.ModelVersion, command.AggregateId, command.CompanyId, false, DateTime.Now);
            private static GroupValue MapToGroupValue(Command command) => new(command.GroupId, command.Name, command.Description, command.CompanyId, new List<Guid>());
        }
    }
}
