using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mavim.Manager.Authorization.Read.Databases.Interfaces;
using DbRole = Mavim.Manager.Authorization.Read.Databases.Models.Role;
using Mavim.Manager.Authorization.Read.Models;
using MediatR;
using Mavim.Manager.Authorization.Read.Clients;
using Mavim.Manager.Authorization.Read.Clients.Interfaces;

namespace Mavim.Manager.Authorization.Read.Queries
{
    public static class GetMyRoles
    {
        // Command
        public record Query() : IRequest<IReadOnlyList<Role>>;

        // Handler
        public class Handler : IRequestHandler<Query, IReadOnlyList<Role>>
        {
            private readonly IRepository _repository;
            private readonly IConnectClient _connectClient;

            public Handler(IRepository repository, IConnectClient connectClient)
            {
                _repository = repository ?? throw new NullReferenceException(nameof(repository));
                _connectClient = connectClient ?? throw new NullReferenceException(nameof(connectClient));
            }

            public async Task<IReadOnlyList<Role>> Handle(Query request, CancellationToken cancellationToken)
            {
                var myUser = await _connectClient.GetConnectMeUser();

                var roles = await _repository.GetRoles(myUser.CompanyId);

                var activeRoles = roles.Where(r => !r.Disabled);

                var myRoles = activeRoles.Where(r => r.Groups.Any(g => myUser.Groups.Contains(g)));

                return myRoles.Select(Map).ToList();
            }
        }

        private static Role Map(DbRole role)
        {
            (Guid id, int _, string name, Guid[] groups, Guid[] topicPermissions, Guid _, bool _, DateTime _) = role;
            return new Role(id, name, groups.ToList(), topicPermissions.ToList());
        }

    }

}