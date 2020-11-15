using BusinessLogic.ServiceContracts;

using System;
using System.Text;
using System.Security.Cryptography;

namespace BusinessLogic.Services
{
    internal class SHA256HashingService : IHashingService
    {
        private SHA256 SHA256 = SHA256.Create();

        public string GetHash(string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            Encoding usedEncoding = encoding ?? Encoding.UTF8;
            byte[] bytes = usedEncoding.GetBytes(text);
            byte[] hash = SHA256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public string GetHashUTF8(string text)
        {
            return GetHash(text, Encoding.UTF8);
        }
    }
}
