using System.IO;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Services.Interfaces.v1
{
    public interface IDescriptionService
    {
        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<ICheckFileInfo> GetFileInfo(string id, string token);

        /// <summary>
        /// Gets the content of the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Stream> GetFileContent(string id);

        /// <summary>
        /// Updates the description.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="descriptionStream">The description stream.</param>
        Task UpdateDescription(string id, Stream descriptionStream);
    }
}
