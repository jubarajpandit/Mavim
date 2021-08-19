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
    public static class AddUserCommand
    {
        // Command
        public record Command(Guid UserId, string Email, Guid CompanyId, int ModelVersion, int AggregateId) : IRequest;

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
                var (userId, _, _, _, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                if (aggregateId != 0)
                    throw new UnprocessableEntityException(
                        string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, 0),
                        aggregateId > 0 ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                        );

                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

                if (user is not null) throw new UnprocessableEntityException(string.Format(Logging.USER_ALREADY_EXISTS, userId));

                _dbContext.Users.Add(Map(request));
                await _dbContext.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static UserTable Map(Command command) => new(command.UserId, Map(MapToGroupValue(command)), command.ModelVersion, command.AggregateId, command.CompanyId, false, DateTime.Now);
            private static UserValue MapToGroupValue(Command command) => new(command.UserId, command.Email, command.CompanyId, new List<Guid>());
        }
    }
}
