using System;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Interfaces
{
    public interface IAzureSqlAccessTokenClient
    {
        Task<string> GetTokenAsync(Guid tenantId, Guid applicationId, string applicationSecret);
    }
}
