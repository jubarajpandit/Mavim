using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Interfaces
{
    public interface IAuthorizationClient
    {
        Task<IAuthorization> GetAuthorization(string requestUri);
    }
}
