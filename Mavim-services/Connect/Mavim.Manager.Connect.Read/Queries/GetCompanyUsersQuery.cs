using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
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
    public static class GetCompanyUsers
    {
        // Command
        public record Query() : IRequest<IReadOnlyList<IUserValue>>;

        // Handler
        public class Handler : BaseHandler, IRequestHandler<Query, IReadOnlyList<IUserValue>>
        {
            private readonly IUserIdentity _userIdentity;
            private readonly IConnectRepository _repository;

            public Handler(IUserIdentity userIdentity, IConnectRepository repository)
            {
                _userIdentity = userIdentity ?? throw new ArgumentNullException(nameof(userIdentity));
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<IReadOnlyList<IUserValue>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _repository.GetUser(_userIdentity.Id);
                // TODO: Check if user is allowed to view company users
                if (user is null || user.Disabled) throw new ForbiddenRequestException(Logging.NOT_ALLOWED);

                var users = await _repository.GetCompanyUsers(user.CompanyId);
                var enabledUsers = users?.Where(user => !user.Disabled);
                var userValues = enabledUsers?.Select(user => Map<UserValue>(user.Value)).ToList();

                return userValues;
            }
        }
    }
}
