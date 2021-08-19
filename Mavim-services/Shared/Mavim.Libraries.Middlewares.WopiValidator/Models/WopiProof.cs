using System;

namespace Mavim.Libraries.Middlewares.WopiValidator.Models
{
    public class WopiProof
    {
        /// <summary>
        /// Gets the current proof key information.
        /// </summary>
        /// <value>
        /// The current proof key information.
        /// </value>
        public WopiProofKeyInfo CurrentWopiProofKeyInfo { get; }

        /// <summary>
        /// Gets the old wopi proof key information.
        /// </summary>
        /// <value>
        /// The old wopi proof key information.
        /// </value>
        public WopiProofKeyInfo OldWopiProofKeyInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiProof" /> class.
        /// </summary>
        /// <param name="currentProofKeyInfo">The current proof key information.</param>
        /// <param name="oldWopiProofKeyInfo">The old wopi proof key information.</param>
        /// <exception cref="ArgumentNullException">
        /// currentProofKeyInfo
        /// or
        /// oldWopiProofKeyInfo
        /// </exception>
        public WopiProof(WopiProofKeyInfo currentProofKeyInfo, WopiProofKeyInfo oldWopiProofKeyInfo)
        {
            CurrentWopiProofKeyInfo = currentProofKeyInfo ?? throw new ArgumentNullException(nameof(currentProofKeyInfo));
            OldWopiProofKeyInfo = oldWopiProofKeyInfo ?? throw new ArgumentNullException(nameof(oldWopiProofKeyInfo));
        }
    }
}
