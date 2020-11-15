namespace Exceptions.BusinessLogic
{
    public class SessionNotFoundException : ServerException
    {
        public override int Code => 6;
        public override string Message => "Session not found";
    }
}
