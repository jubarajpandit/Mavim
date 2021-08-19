using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.WopiFileLock.Model
{
    public class WopiFileLockState : IWopiFileLockState
    {
        /// <summary>
        /// Gets or sets the database identifier. Part of the composite key.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        [Required]
        public Guid DbId { get; set; }

        /// <summary>
        /// Gets or sets the DCV identifier. Part of the composite key.
        /// </summary>
        /// <value>
        /// The DCV identifier.
        /// </value>
        [Required]
        public string DcvId { get; set; }

        /// <summary>
        /// Gets or sets the user object identifier.
        /// </summary>
        /// <value>
        /// The user object identifier.
        /// </value>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>
        /// The expires.
        /// </value>
        [Required]
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Required]

        public string Version { get; set; }
    }
}