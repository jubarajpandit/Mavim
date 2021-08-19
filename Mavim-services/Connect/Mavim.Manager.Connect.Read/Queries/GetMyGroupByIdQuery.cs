using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Mavim.Manager.Connect.Read.Models;
using Mavim.Manager.Connect.Read.Models.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Queries
{
    public static class GetMyGroupById
    {
        // Command
        public record Query(Guid GroupId) : IRequest<IGroupValue>;

        // Handler
        public class Handler : BaseHandler, IRequestHandler<Query, IGroupValue>
        {
            private readonly IUserIdentity _userIdentity;
            private readonly IConnectRepository _repository;

            public Handler(IUserIdentity userIdentity, IConnectRepository repository)
            {
                _userIdentity = userIdentity ?? throw new ArgumentNullException(nameof(userIdentity));
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<IGroupValue> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));
                if (request.GroupId == Guid.Empty) throw new BadRequestException(string.Format(Logging.INVALID_GROUPID, request.GroupId));

                var user = await _repository.GetUser(_userIdentity.Id);
                if (user is null || user.Disabled) throw new ForbiddenRequestException(Logging.NOT_ALLOWED);

                var userValue = Map<UserValue>(user.Value);

                if (userValue.Groups is null || !userValue.Groups.ToList().Contains(request.GroupId))
                    throw new RequestNotFoundException(string.Format(Logging.GROUP_NOT_FOUND, request.GroupId));

                var group = await _repository.GetGroup(request.GroupId);
                if (group is null || group.Disabled) throw new RequestNotFoundException(string.Format(Logging.GROUP_NOT_FOUND, request.GroupId));

                var groupValue = Map<GroupValue>(group.Value);
                var users = await _repository.GetUsers(groupValue.Users);
                var enabledUsersGuids = users?.Where(user => !user.Disabled)
                    .Select(user => user.Id)
                    .ToList();

                var groupWithEnabledUsers = new GroupValue(groupValue.Id, groupValue.Name, groupValue.Description, groupValue.CompanyId, enabledUsersGuids);

                return groupWithEnabledUsers;
            }
        }
    }
}
