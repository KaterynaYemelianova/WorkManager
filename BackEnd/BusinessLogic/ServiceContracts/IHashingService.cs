using System.Text;

namespace BusinessLogic.ServiceContracts
{
    internal interface IHashingService
    {
        string GetHashHex(string text, Encoding encoding = null);
        string GetHashBase64(string text, Encoding encoding = null);
    }
}
