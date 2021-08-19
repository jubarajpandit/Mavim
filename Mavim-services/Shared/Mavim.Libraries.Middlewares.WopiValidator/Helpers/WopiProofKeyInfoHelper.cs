using Mavim.Libraries.Middlewares.WopiValidator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.WopiValidator.Helpers
{
    public class WopiProofKeyInfoHelper
    {
        private const string SHA256 = "SHA256";
        readonly WopiProofKeyInfo _currentKeyInfo;
        readonly WopiProofKeyInfo _oldKeyInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiProofKeyInfoHelper"/> class.
        /// </summary>
        /// <param name="currentKeyInfo">The current key.</param>
        /// <param name="oldKeyInfo">The old key.</param>
        public WopiProofKeyInfoHelper(WopiProofKeyInfo currentKeyInfo, WopiProofKeyInfo oldKeyInfo)
        {
            _currentKeyInfo = currentKeyInfo;
            _oldKeyInfo = oldKeyInfo;
        }

        /// <summary>
        /// Validates the specified proofkey validation input.
        /// </summary>
        /// <param name="proofkeyValidationInput">The proofkey validation input.</param>
        /// <returns></returns>
        public async Task<bool> ValidateWopiProof(WopiProofKeyValidationInput proofkeyValidationInput)
        {
            byte[] accessTokenBytes = Encoding.UTF8.GetBytes(proofkeyValidationInput.AccessToken);
            byte[] hostUrlBytes = Encoding.UTF8.GetBytes(proofkeyValidationInput.Url.ToUpperInvariant());
            byte[] timeStampBytes = EncodeNumber(proofkeyValidationInput.TimeStamp);

            // As described in the WOPI proof key validation technique : https://wopi.readthedocs.io/en/latest/scenarios/proofkeys.html
            // Assemble the data as follows:
            // 4 bytes that represent the length, in bytes, of the access_token on the request.
            // The access_token.
            // 4 bytes that represent the length, in bytes, of the full URL of the WOPI request, including any query string parameters.
            // The WOPI request URL in all uppercase. All query string parameters on the request URL should be included.
            // 4 bytes that represent the length, in bytes, of the X - WOPI - TimeStamp value.
            // The X-WOPI - TimeStamp value.
            List<byte> expectedProof = new List<byte>(
              4 + accessTokenBytes.Length +
              4 + hostUrlBytes.Length +
              4 + timeStampBytes.Length);

            expectedProof.AddRange(EncodeNumber(accessTokenBytes.Length));
            expectedProof.AddRange(accessTokenBytes);
            expectedProof.AddRange(EncodeNumber(hostUrlBytes.Length));
            expectedProof.AddRange(hostUrlBytes);
            expectedProof.AddRange(EncodeNumber(timeStampBytes.Length));
            expectedProof.AddRange(timeStampBytes);

            byte[] expectedProofArray = expectedProof.ToArray();

            bool isValidTimeStamp = IsValidTimeStamp(proofkeyValidationInput.TimeStamp);

            //TODO: Need to log here about expired timestamp for reference.(Wi:19124)
            if (!isValidTimeStamp) return false;

            bool isValidProofKey = await Task.Run(() => TryVerification(expectedProofArray, proofkeyValidationInput.CurrentProof, _currentKeyInfo.CspBlob) ||
                                    TryVerification(expectedProofArray, proofkeyValidationInput.OldProof, _currentKeyInfo.CspBlob) ||
                                    TryVerification(expectedProofArray, proofkeyValidationInput.CurrentProof, _oldKeyInfo.CspBlob));

            return isValidProofKey;
        }

        /// <summary>
        /// Determines whether [is valid time stamp] [the specified proof key time stamp].
        /// </summary>
        /// <param name="proofKeyTimeStamp">The proof key time stamp.</param>
        /// <returns>
        ///   <c>true</c> if [is valid time stamp] [the specified proof key time stamp]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidTimeStamp(long proofKeyTimeStamp)
        {
            DateTime proofKeyDateTime = new DateTime(proofKeyTimeStamp).ToUniversalTime();
            TimeSpan proofKeyTimeSpanDifference = DateTime.UtcNow.Subtract(proofKeyDateTime);

            // Check if TimeStamp of the proofkey is not more than 20 minutes old as given by the guidelines of Microsoft @ https://wopi.readthedocs.io/en/latest/scenarios/proofkeys.html
            return !((int)proofKeyTimeSpanDifference.TotalMinutes > 20);
        }

        /// <summary>
        /// Encodes the number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static byte[] EncodeNumber(int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Encodes the number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static byte[] EncodeNumber(long value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Try to verify if the proof keys are correct using the public keys from the wopi discovery url
        /// </summary>
        /// <param name="expectedProof">The expected proof.</param>
        /// <param name="signedProof">The signed proof.</param>
        /// <param name="publicKeyCspBlob">The public key CSP BLOB.</param>
        /// <returns></returns>
        private static bool TryVerification(byte[] expectedProof, string signedProof, string publicKeyCspBlob)
        {
            using (RSACryptoServiceProvider rsaAlg = new RSACryptoServiceProvider())
            {
                byte[] publicKey = Convert.FromBase64String(publicKeyCspBlob);
                byte[] signedProofBytes = Convert.FromBase64String(signedProof);
                try
                {
                    rsaAlg.ImportCspBlob(publicKey);
                    return rsaAlg.VerifyData(expectedProof, SHA256, signedProofBytes);
                }
                catch (FormatException)
                {
                    return false;
                }
                catch (CryptographicException)
                {
                    return false;
                }
            }
        }
    }
}
