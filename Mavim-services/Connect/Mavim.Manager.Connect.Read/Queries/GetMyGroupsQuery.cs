﻿using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Mavim.Manager.Connect.Read.Models;
using Mavim.Manager.Connect.Read.Models.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Queries
{
    public static class GetMyGroups
    {
        // Command
        public record Query() : IRequest<IReadOnlyList<IGroupValue>>;

        // Handler
        public class Handler : BaseHandler, IRequestHandler<Query, IReadOnlyList<IGroupValue>>
        {
            private readonly IUserIdentity _userIdentity;
            private readonly IConnectRepository _repository;

            public Handler(IUserIdentity userIdentity, IConnectRepository repository)
            {
                _userIdentity = userIdentity ?? throw new ArgumentNullException(nameof(userIdentity));
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<IReadOnlyList<IGroupValue>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _repository.GetUser(_userIdentity.Id);
                if (user is null || user.Disabled) throw new ForbiddenRequestException(Logging.NOT_ALLOWED);

                var userValue = Map<UserValue>(user.Value);
                var groups = await _repository.GetGroups(userValue.Groups);
                var enabledGroups = groups?.Where(group => !group.Disabled).ToList();

                var groupValues = enabledGroups?.Select(group => Map<GroupValue>(group.Value)).ToList();

                var groupsWithEnabledUsers = new List<IGroupValue>();
                foreach (IGroupValue group in groupValues)
                {
                    var users = await _repository.GetUsers(group.Users);
                    var enabledUsersGuids = users?.Where(user => !user.Disabled)
                        .Select(user => user.Id)
                        .ToList();

                    var groupWithEnabledUsers = new GroupValue(group.Id, group.Name, group.Description, group.CompanyId, enabledUsersGuids);
                    groupsWithEnabledUsers.Add(groupWithEnabledUsers);
                }

                return groupsWithEnabledUsers;
            }
        }
    }
}