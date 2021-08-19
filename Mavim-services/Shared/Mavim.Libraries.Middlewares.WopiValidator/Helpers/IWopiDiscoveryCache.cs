using Mavim.Libraries.Middlewares.WopiValidator.Models;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.WopiValidator.Helpers
{
    public interface IWopiDiscoveryCache
    {
        Task<WopiProof> GetOrAddWopiProofFromCache(string discoveryUrl, int proofKeyInfoCacheExpirationInMinutes);

        Task<IWopiActionUrlMetaData> GetOrAddWopiSrcFromCache(string discoveryUrl,
            int proofKeyInfoCacheExpirationInMinutes, string supportedWopiQueryParameterValues);
    }
}
