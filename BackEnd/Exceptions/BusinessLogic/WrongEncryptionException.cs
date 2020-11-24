namespace Exceptions.BusinessLogic
{
    public class WrongEncryptionException : ServerException
    {
        public string WrongEncryptedField { get; private set; }
        public override int Code => 12;
        public override string Message => $"{WrongEncryptedField} was encrypted incorrectly";

        public WrongEncryptionException(string wrongEncryptedField)
        {
            WrongEncryptedField = wrongEncryptedField;
        }
    }
}
