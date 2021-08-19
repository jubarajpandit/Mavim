using System.Threading.Tasks;
using Mavim.Manager.Authorization.Read.Clients.Models;

namespace Mavim.Manager.Authorization.Read.Clients.Interfaces
{
    public interface IConnectClient
    {
        Task<User> GetConnectMeUser();
    }

}
