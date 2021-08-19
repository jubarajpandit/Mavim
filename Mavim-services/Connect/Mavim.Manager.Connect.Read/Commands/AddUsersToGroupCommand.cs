using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Databases.Models;
using Mavim.Manager.Connect.Read.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Commands
{
    public static class AddUsersToGroupCommand
    {
        // Command
        public record Command(Guid GroupId, IEnumerable<Guid> UserIds, int ModelVersion, int AggregateId) : IRequest;

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
                var (groupId, userIds, modelVersion, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                var group = await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
                if (group is null) throw new RequestNotFoundException(string.Format(Logging.GROUP_NOT_FOUND, groupId));

                var expectedAggregateId = group.AggregateId + 1;
                if (aggregateId != expectedAggregateId)
                    throw new UnprocessableEntityException(
                        string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, expectedAggregateId),
                        aggregateId > expectedAggregateId ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                        );

                foreach (Guid userId in userIds)
                    await AddGroupToUser(userId, groupId, cancellationToken);

                AddUsersToGroup(group, userIds, modelVersion, aggregateId);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private void AddUsersToGroup(GroupTable group, IEnumerable<Guid> userIds, int modelVersion, int aggregateId)
            {
                var groupValue = Map<GroupValue>(group?.Value);
                var updatedUsers = groupValue?.Users is null ? userIds?.ToList() : groupValue.Users.Concat(userIds).ToList();
                var updatedGroupValue = groupValue with
                {
                    Users = updatedUsers
                };

                var updatedGroup = group with
                {
                    Value = Map(updatedGroupValue),
                    ModelVersion = modelVersion,
                    AggregateId = aggregateId,
                    LastUpdated = DateTime.Now
                };

                _dbContext.Groups.Update(updatedGroup);
            }

            private async Task AddGroupToUser(Guid userId, Guid groupId, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
                if (user is null) throw new RequestNotFoundException(string.Format(Logging.USER_NOT_FOUND, userId));

                var userValue = Map<UserValue>(user.Value);
                var groups = userValue?.Groups?.ToList() ?? new List<Guid>();
                groups.Add(groupId);

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
