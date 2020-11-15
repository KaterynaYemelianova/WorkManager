namespace Exceptions.BusinessLogic
{
    public class LoginDuplicationException : ServerException
    {
        public override int Code => 3;
        public override string Message => $"User with such login already exists";
    }
}
