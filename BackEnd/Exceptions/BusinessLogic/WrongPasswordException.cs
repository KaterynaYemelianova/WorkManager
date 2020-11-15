namespace Exceptions.BusinessLogic
{
    public class WrongPasswordException : ServerException
    {
        public override int Code => 10;
        public override string Message => "Wrong password";
    }
}
