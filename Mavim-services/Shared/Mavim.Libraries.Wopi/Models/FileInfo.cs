using Mavim.Libraries.Wopi.Interfaces;

namespace Mavim.Libraries.Wopi.Models
{
    public class FileInfo : IFileInfo
    {
        /// <summary>
        /// Gets or sets the name of the base file.
        /// The string name of the file, including extension, without a path. Used for display in user interface (UI), and determining the extension of the file.
        /// </summary>
        /// <value>
        /// The name of the base file.
        /// </value>
        public string BaseFileName { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// The size of the file in bytes, expressed as a long, a 64-bit signed integer.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public long Size { get; set; }
    }
}
