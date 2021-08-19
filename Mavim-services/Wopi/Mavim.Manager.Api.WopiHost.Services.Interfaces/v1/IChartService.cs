using System.IO;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Services.Interfaces.v1
{
    public interface IChartService
    {
        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="access_token">The access token.</param>
        /// <returns></returns>
        Task<ICheckFileInfo> GetFileInfo(string id, string access_token, string embeddingPageOrigin, string embeddingPageSessionInfo);

        /// <summary>
        /// Gets the content of the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="access_token">The access token.</param>
        /// <returns></returns>
        Task<Stream> GetFileContent(string id, string access_token);
    }
}
