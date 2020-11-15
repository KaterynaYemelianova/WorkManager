using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BusinessLogic.Services
{
    internal class RSAService : IAsymmetricEncryptionService
    {
        private Dictionary<PublicKeyModel, RSA> Codes = new Dictionary<PublicKeyModel, RSA>();

        public string Decrypt(string encryptedText, PublicKeyModel encryptionPublicKey)
        {
            RSA rsa = Codes[encryptionPublicKey];
            byte[] encryptedData = Convert.FromBase64String(encryptedText);
            byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedData);
        }

        public string Encrypt(string text, PublicKeyModel publicKey)
        {
            RSA rsa = Codes[publicKey];
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedData);
        }

        public PublicKeyModel GetNewPublicKey()
        {   
            RSA rsa = RSA.Create();

            PublicKeyModel publicKey = new PublicKeyModel(
                rsa.ToXmlString(false)
            );

            Codes.Add(publicKey, rsa);

            return publicKey;
        }

        public void DestroyKeyPair(PublicKeyModel publicKey)
        {
            Codes[publicKey].Dispose();
            Codes.Remove(publicKey);
        }
    }
}
