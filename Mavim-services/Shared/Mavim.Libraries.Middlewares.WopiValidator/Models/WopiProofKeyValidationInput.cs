using System;

namespace Mavim.Libraries.Middlewares.WopiValidator.Models
{
    public class WopiProofKeyValidationInput
    {
        readonly string _accessToken;
        readonly long _timeStamp;
        readonly string _url;
        readonly string _currentProof;
        readonly string _oldProof;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiProofKeyValidationInput" /> class.
        /// </summary>
        /// <param name="accessToken">The access toke.</param>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="url">The URL.</param>
        /// <param name="currentProof">The proof.</param>
        /// <param name="oldProof">The old proof.</param>
        public WopiProofKeyValidationInput(string accessToken, long timeStamp, string url, string currentProof, string oldProof)
        {
            _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            _timeStamp = timeStamp;
            _url = url ?? throw new ArgumentNullException(nameof(url));
            _currentProof = currentProof ?? throw new ArgumentNullException(nameof(currentProof));
            _oldProof = oldProof ?? throw new ArgumentNullException(nameof(oldProof));
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken => _accessToken;

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public long TimeStamp => _timeStamp;

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url => _url;

        /// <summary>
        /// Gets the proof.
        /// </summary>
        /// <value>
        /// The proof.
        /// </value>
        public string CurrentProof => _currentProof;

        /// <summary>
        /// Gets the old proof.
        /// </summary>
        /// <value>
        /// The old proof.
        /// </value>
        public string OldProof => _oldProof;
    }
}
