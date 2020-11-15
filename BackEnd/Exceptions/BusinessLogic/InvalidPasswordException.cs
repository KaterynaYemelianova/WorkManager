namespace Exceptions.BusinessLogic
{
    public class InvalidPasswordException : ServerException
    {
        public override int Code => 4;

        public override string Message => "Password must consist of English letters and digits between 8 and 32 symbols";
    }
}
