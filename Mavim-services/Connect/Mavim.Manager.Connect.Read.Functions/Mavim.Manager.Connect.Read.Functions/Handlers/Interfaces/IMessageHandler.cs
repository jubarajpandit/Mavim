using System.Net.Http;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Functions.Handlers.Interfaces
{
    public interface IMessageHandler
    {
        Task<HttpResponseMessage> ExecuteAsync();
    }
}
