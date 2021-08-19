using Mavim.Libraries.Middlewares.WopiValidator.Helpers;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Services.Interfaces.v1
{
    public interface IWopiActionUrlMetadataService
    {
        Task<IWopiActionUrlMetaData> GetWopiSourceMetadata();
    }
}
