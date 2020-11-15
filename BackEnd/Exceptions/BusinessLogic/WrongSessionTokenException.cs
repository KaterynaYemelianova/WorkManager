namespace Exceptions.BusinessLogic
{
    public class WrongSessionTokenException : ServerException
    {
        public override int Code => 7;
        public override string Message => "Wrong session token";
    }
}
