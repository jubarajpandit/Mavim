using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Commands
{
    public static class EnableUserCommand
    {
        // Command
        public record Command(Guid UserId, int ModelVersion, int AggregateId) : IRequest;

        // Handler
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ConnectDatabaseContext _dbContext;

            public Handler(ConnectDatabaseContext dbContext)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var (userId, modelVersion, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
                if (user is null)
                    throw new RequestNotFoundException($"Could not find user with id: {userId}");

                var expectedAggregateId = user.AggregateId + 1;
                if (aggregateId != expectedAggregateId)
                    throw new UnprocessableEntityException(
                        string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, expectedAggregateId),
                        aggregateId > expectedAggregateId ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                        );

                if (!user.Disabled)
                    throw new ConflictException($"User with id: {userId} is already enabled");

                var updatedUser = user with
                {
                    Disabled = false,
                    ModelVersion = modelVersion,
                    AggregateId = aggregateId,
                    LastUpdated = DateTime.Now
                };

                _dbContext.Users.Update(updatedUser);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Unit.Value);
            }
        }
    }
}
