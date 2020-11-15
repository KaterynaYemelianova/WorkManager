using System.Text;

namespace BusinessLogic.ServiceContracts
{
    internal interface IHashingService
    {
        string GetHashUTF8(string text);
        string GetHash(string text, Encoding encoding);
    }
}
