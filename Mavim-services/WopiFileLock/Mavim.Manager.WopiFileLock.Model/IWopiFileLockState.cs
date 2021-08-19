using System;

namespace Mavim.Manager.WopiFileLock.Model
{
    public interface IWopiFileLockState
    {
        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        Guid DbId { get; set; }

        /// <summary>
        /// Gets or sets the DCV identifier.
        /// </summary>
        /// <value>
        /// The DCV identifier.
        /// </value>
        string DcvId { get; set; }

        /// <summary>
        /// Gets or sets the user object identifier.
        /// </summary>
        /// <value>
        /// The user object identifier.
        /// </value>
        string UserId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        string Value { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>
        /// The expires.
        /// </value>
        DateTime Expires { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        string Version { get; set; }
    }
}
