using System.IO;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1
{
    public interface IChartRepository
    {
        /// <summary>
        /// Checks the file information.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        Task<ICheckFileInfo> CheckFileInfo(string dcvId);

        /// <summary>
        /// Gets the content of the file.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        Task<Stream> GetFileContent(string dcvId);
    }
}
