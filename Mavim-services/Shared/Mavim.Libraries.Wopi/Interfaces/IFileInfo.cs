namespace Mavim.Libraries.Wopi.Interfaces
{
    public interface IFileInfo
    {
        /// <summary>
        /// Gets or sets the name of the base file.
        /// </summary>
        /// <value>
        /// The name of the base file.
        /// </value>
        string BaseFileName { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        long Size { get; set; }
    }
}
