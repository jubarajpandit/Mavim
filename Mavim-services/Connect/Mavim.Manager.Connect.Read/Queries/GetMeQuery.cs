using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Mavim.Manager.Connect.Read.Models;
using Mavim.Manager.Connect.Read.Models.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Queries
{
    public static class GetMe
    {
        // Command
        public record Query() : IRequest<IUserValue>;

        // Handler
        public class Handler : BaseHandler, IRequestHandler<Query, IUserValue>
        {
            private readonly IUserIdentity _userIdentity;
            private readonly IConnectRepository _repository;

            public Handler(IUserIdentity userIdentity, IConnectRepository repository)
            {
                _userIdentity = userIdentity ?? throw new ArgumentNullException(nameof(userIdentity));
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<IUserValue> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _repository.GetUser(_userIdentity.Id);

                if (user is null || user.Disabled) throw new ForbiddenRequestException(Logging.NOT_ALLOWED);

                var userValue = Map<UserValue>(user.Value);
                return userValue;
            }
        }
    }
}
