using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Commands
{
    public static class UpdateGroupCommand
    {
        // Command
        public record Command(Guid GroupId, string Name, string Description, int ModelVersion, int AggregateId) : IRequest;

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
                var (groupId, name, description, modelVersion, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                var group = await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
                if (group is null) throw new RequestNotFoundException(string.Format(Logging.GROUP_NOT_FOUND, groupId));

                var expectedAggregateId = group.AggregateId + 1;
                if (aggregateId != expectedAggregateId)
                    throw new UnprocessableEntityException(
                        string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, expectedAggregateId),
                        aggregateId > expectedAggregateId ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                        );

                var groupValue = Map<GroupValue>(group.Value);

                var updatedGroupValue = groupValue with
                {
                    Name = name ?? groupValue.Name,
                    Description = description ?? groupValue.Description
                };

                var updatedGroup = group with
                {
                    Value = Map(updatedGroupValue),
                    ModelVersion = modelVersion,
                    AggregateId = aggregateId,
                    LastUpdated = DateTime.Now
                };

                _dbContext.Groups.Update(updatedGroup);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Unit.Value);
            }
        }
    }
}
