using System;

namespace Mavim.Libraries.Middlewares.WopiValidator.Models
{
    public class WopiProofKeyInfo
    {
        /// <summary>
        /// Gets the CSP BLOB.
        /// </summary>
        /// <value>
        /// The CSP BLOB.
        /// </value>
        public string CspBlob { get; private set; }
        /// <summary>
        /// Gets the exponent.
        /// </summary>
        /// <value>
        /// The exponent.
        /// </value>
        public string Exponent { get; private set; }
        /// <summary>
        /// Gets the modulus.
        /// </summary>
        /// <value>
        /// The modulus.
        /// </value>
        public string Modulus { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiProofKeyInfo" /> class.
        /// </summary>
        /// <param name="cspBlob">The CSP BLOB.</param>
        /// <param name="exponent">The exponent.</param>
        /// <param name="modulus">The modulus.</param>
        /// <exception cref="ArgumentNullException">
        /// cspBlob
        /// or
        /// exponent
        /// or
        /// modulus
        /// </exception>
        public WopiProofKeyInfo(string cspBlob, string exponent, string modulus)
        {
            CspBlob = cspBlob ?? throw new ArgumentNullException(nameof(cspBlob));
            Exponent = exponent ?? throw new ArgumentNullException(nameof(exponent));
            Modulus = modulus ?? throw new ArgumentNullException(nameof(modulus));
        }
    }
}
