namespace Exceptions.BusinessLogic
{
    public class SessionExpiredException : ServerException
    {
        public override int Code => 8;
        public override string Message => "Session expired";
    }
}
