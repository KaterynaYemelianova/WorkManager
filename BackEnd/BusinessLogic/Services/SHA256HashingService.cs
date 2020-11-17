using BusinessLogic.ServiceContracts;

using System;
using System.Text;
using System.Security.Cryptography;

namespace BusinessLogic.Services
{
    internal class SHA256HashingService : IHashingService
    {
        private SHA256 SHA256 = SHA256.Create();

        public string GetHashBase64(string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            byte[] hash = GetHash(text, encoding);

            return Convert.ToBase64String(hash);
        }

        public string GetHashHex(string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            byte[] hash = GetHash(text, encoding);

            return BitConverter.ToString(hash).Replace("-", "").ToUpper();
        }

        private byte[] GetHash(string text, Encoding encoding = null)
        {
            Encoding usedEncoding = encoding ?? Encoding.UTF8;

            byte[] bytes = usedEncoding.GetBytes(text);
            byte[] hash = SHA256.ComputeHash(bytes);

            return hash;
        }
    }
}
