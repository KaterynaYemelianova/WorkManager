using BusinessLogic.Models;

namespace BusinessLogic.ServiceContracts
{
    public interface IAsymmetricEncryptionService
    {
        PublicKeyModel GetNewPublicKey();
        string Decrypt(string encryptedText, PublicKeyModel publicKey);
        string Encrypt(string text, PublicKeyModel publicKey);
        void DestroyKeyPair(PublicKeyModel publicKey);
    }
}
