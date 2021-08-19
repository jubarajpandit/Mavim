using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Authorization.Services.Interfaces.v1
{
    public interface IAuthorizationService
    {
        Task<IUser> GetUser();
        Task<IEnumerable<IUser>> GetUsers();
        Task<IEnumerable<IUser>> AddUsers(IEnumerable<IUser> users);
        Task EditUserRole(Guid userId, Role role);
        Task DeleteUser(Guid userId);
    }
}