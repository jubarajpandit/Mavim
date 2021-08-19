using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Commands
{
    public static class DisableGroupCommand
    {
        // Command
        public record Command(Guid GroupId, int ModelVersion, int AggregateId) : IRequest;

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
                var (groupId, modelVersion, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                var group = await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
                if (group is null)
                    throw new RequestNotFoundException($"Could not find group with id: {groupId}");

                var expectedAggregateId = group.AggregateId + 1;
                if (aggregateId != expectedAggregateId)
                    throw new UnprocessableEntityException(
                        string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, expectedAggregateId),
                        aggregateId > expectedAggregateId ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                        );

                if (group.Disabled)
                    throw new UnprocessableEntityException($"Group with id {groupId} is disabled.");

                var groupValue = Map<GroupValue>(group.Value);
                foreach (Guid userId in groupValue.Users)
                    await RemoveGroupFromUser(groupId, userId, cancellationToken);

                var updatedGroup = group with
                {
                    Disabled = true,
                    ModelVersion = modelVersion,
                    AggregateId = aggregateId,
                    LastUpdated = DateTime.Now
                };

                _dbContext.Groups.Update(updatedGroup);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private async Task RemoveGroupFromUser(Guid groupId, Guid userId, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
                if (user is null) throw new RequestNotFoundException(string.Format(Logging.USER_NOT_FOUND, userId));

                var userValue = Map<UserValue>(user.Value);
                var groups = userValue.Groups.Where(id => id != groupId).ToList();

                var updatedUserValue = userValue with
                {
                    Groups = groups
                };

                var updatedUser = user with
                {
                    Value = Map(updatedUserValue)
                };

                _dbContext.Users.Update(updatedUser);
            }
        }
    }
}
