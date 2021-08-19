using Mavim.Manager.Connect.Read.Databases.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Databases.Interfaces
{
    public interface IConnectRepository
    {
        Task<IDiscoveryUser> GetDiscoveryUser(string email, Guid tenantId);
        Task<UserTable> GetUser(Guid userId);
        Task<CompanyTable> GetCompany(Guid companyId);
        Task<IReadOnlyList<UserTable>> GetCompanyUsers(Guid companyId);
        Task<IReadOnlyList<GroupTable>> GetCompanyGroups(Guid companyId);
        Task<GroupTable> GetGroup(Guid groupId);
        Task<IReadOnlyList<GroupTable>> GetGroups(IEnumerable<Guid> groupIds);
        Task<IReadOnlyList<UserTable>> GetUsers(IEnumerable<Guid> userIds);
    }
}
