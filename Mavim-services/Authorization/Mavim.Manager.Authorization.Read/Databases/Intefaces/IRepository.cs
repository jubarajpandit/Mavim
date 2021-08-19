using Mavim.Manager.Authorization.Read.Databases.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Authorization.Read.Databases.Interfaces
{
    public interface IRepository
    {
        Task<IReadOnlyList<Role>> GetRoles(Guid companyId);
    }
}
