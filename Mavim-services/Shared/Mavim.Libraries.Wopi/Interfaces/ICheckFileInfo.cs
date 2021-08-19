namespace Mavim.Libraries.Wopi.Interfaces
{
    public interface ICheckFileInfo
    {
        #region Mandatory Fields
        /// <summary>
        /// Gets or sets the name of the base file.
        /// </summary>
        /// <value>
        /// The name of the base file.
        /// </value>
        string BaseFileName { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        long Size { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        string UserId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        string Version { get; set; }
        #endregion

        #region Non Mandatory fields
        /// <summary>
        /// Gets or sets the sh a256.
        /// </summary>
        /// <value>
        /// The sh a256.
        /// </value>
        string SHA256 { get; set; }

        /// <summary>
        /// Gets or sets the supports update.
        /// </summary>
        /// <value>
        /// The supports update.
        /// </value>
        bool SupportsUpdate { get; set; }

        /// <summary>
        /// Gets or sets the user can write.
        /// </summary>
        /// <value>
        /// The user can write.
        /// </value>
        bool UserCanWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [supports get lock].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports get lock]; otherwise, <c>false</c>.
        /// </value>
        bool SupportsGetLock { get; set; }

        /// <summary>
        /// Gets or sets the supports locks.
        /// </summary>
        /// <value>
        /// The supports locks.
        /// </value>
        bool SupportsLocks { get; set; }

        /// <summary>
        /// Gets or sets the name of the user friendly.
        /// </summary>
        /// <value>
        /// The name of the user friendly.
        /// </value>
        string UserFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is anonymous user.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is anonymous user; otherwise, <c>false</c>.
        /// </value>
        bool IsAnonymousUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [license check for edit is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [license check for edit is enabled]; otherwise, <c>false</c>.
        /// </value>
        bool LicenseCheckForEditIsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the host edit URL.
        /// </summary>
        /// <value>
        /// The host edit URL.
        /// </value>
        string HostEditUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read only]; otherwise, <c>false</c>.
        /// </value>
        bool ReadOnly { get; set; }
        #endregion       
    }
}
