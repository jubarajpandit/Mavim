using System;
using System.IO;
using System.Security.Cryptography;

namespace Mavim.Manager.Api.Utils
{
    public static class HashUtils
    {
        public static string ComputeSHA256(Stream stream)
        {
            string sha256 = string.Empty;

            if (stream.Length == 0) return sha256;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] checksum = sha.ComputeHash(stream);
                sha256 = Convert.ToBase64String(checksum);
            }

            return sha256;
        }
    }
}
