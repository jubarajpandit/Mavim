using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Interface
{
    public interface IAuthorizationRepository
    {
        Task<IUser> GetUser(string email, Guid tenantId);
        Task<IUser> GetUser(Guid userId, Guid tenantId);
        Task<IEnumerable<IUser>> GetUsers(Guid tenantId);
        Task<IEnumerable<IUser>> AddUsers(IEnumerable<IUser> users);
        Task EditUserRole(Guid userId, Guid tenantId, UserRole role);
        Task DeleteUser(Guid userId, Guid tenantId);
    }
}