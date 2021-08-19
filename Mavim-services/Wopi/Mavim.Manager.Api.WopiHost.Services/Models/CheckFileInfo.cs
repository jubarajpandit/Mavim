using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;

namespace Mavim.Manager.Api.WopiHost.Services.Models
{
    public class CheckFileInfo : ICheckFileInfo
    {
        // ==============================================================================================================================================================
        // ************************************************************************* IMPORTANT *************************************************************************
        // For detailed information on the properties of the CheckFileInfo look in to : https://wopi.readthedocs.io/projects/wopirest/en/latest/files/CheckFileInfo.html
        // ==============================================================================================================================================================

        #region Mandatory Fields

        /// <summary>
        /// Gets or sets the name of the base file.
        /// The string name of the file, including extension, without a path. Used for display in user interface (UI), and determining the extension of the file.
        /// </summary>
        /// <value>
        /// The name of the base file.
        /// </value>
        public string BaseFileName { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// A string that uniquely identifies the owner of the file. In most cases, the user who uploaded or created the file should be considered the owner.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// The size of the file in bytes, expressed as a long, a 64-bit signed integer.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// A string value uniquely identifying the user currently accessing the file.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// The current version of the file based on the server’s file version schema, as a string. This value must change when the file changes, and version values must never repeat for a given file.
        /// This value must be a string, even if numbers are used to represent versions.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the user friendly.
        /// A string that is the name of the user, suitable for displaying in UI.
        /// </summary>
        /// <value>
        /// The name of the user friendly.
        /// </value>
        public string UserFriendlyName { get; set; }
        #endregion

        #region Non Mandatory fields

        /// <summary>
        /// Gets or sets the SHA256.
        /// A 256 bit SHA-2-encoded [FIPS 180-2] hash of the file contents, as a Base64-encoded string. Used for caching purposes in WOPI clients.
        /// </summary>
        /// <value>
        /// The sh a256.
        /// </value>
        public string SHA256 { get; set; }

        /// <summary>
        /// Gets or sets the supports update.
        /// A Boolean value that indicates that the host supports the following WOPI operations:
        /// PutFile, PutRelativeFile
        /// </summary>
        /// <value>
        /// The supports update.
        /// </value>
        public bool SupportsUpdate { get; set; }

        /// <summary>
        /// Gets or sets the user can write.
        /// A Boolean value that indicates that the user has permission to alter the file. Setting this to true tells the WOPI client that it can call PutFile on behalf of the user.
        /// </summary>
        /// <value>
        /// The user can write.
        /// </value>
        public bool UserCanWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [supports get lock].
        /// A Boolean value that indicates that the host supports the GetLock operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports get lock]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsGetLock { get; set; }

        /// <summary>
        /// Gets or sets the supports locks.
        /// A Boolean value that indicates that the host supports the following WOPI operations:
        /// Lock, Unlock, RefreshLock, UnlockAndRelock operations for this file.
        /// </summary>
        /// <value>
        /// The supports locks.
        /// </value>
        public bool SupportsLocks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [supports extended lock length].
        /// A Boolean value that indicates that the host supports lock IDs up to 1024 ASCII characters long. If not provided, WOPI clients will assume that lock IDs are limited to 256 ASCII characters.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports extended lock length]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsExtendedLockLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [supports rename].
        /// A Boolean value that indicates that the host supports the RenameFile operation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports rename]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsRename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [supports user information].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports user information]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsUserInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [user can not write relative].
        /// A Boolean value that indicates the user does not have sufficient permission to create new files on the WOPI server. Setting this to true tells the WOPI client that calls to PutRelativeFile will fail for this user on the current file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [user can not write relative]; otherwise, <c>false</c>.
        /// </value>
        public bool UserCanNotWriteRelative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is anonymous user.
        /// A Boolean value indicating whether the user is authenticated with the host or not. Hosts should always set this to true for unauthenticated users, so that clients are aware that the user is anonymous.
        /// When setting this to true, hosts can choose to omit the UserId property, but must still set the OwnerId property.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is anonymous user; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnonymousUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [license check for edit is enabled].
        /// A Boolean value indicating whether the user is a business user or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if [license check for edit is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool LicenseCheckForEditIsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the host edit URL.
        /// A URI to a host page that loads the edit WOPI action.
        /// </summary>
        /// <value>
        /// The host edit URL.
        /// </value>
        public string HostEditUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// A Boolean value that indicates that, for this user, the file cannot be changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if [read only]; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// This property is necessary for interacting with embedded visio charts 
        /// </summary>
        public string EmbeddingPageOrigin { get; set; }

        /// <summary>
        /// This property is necessary for interacting with embedded visio charts 
        /// </summary>
        public string EmbeddingPageSessionInfo { get; set; }
        #endregion
    }
}
