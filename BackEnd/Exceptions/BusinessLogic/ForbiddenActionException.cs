namespace Exceptions.BusinessLogic
{
    public class ForbiddenActionException : ServerException
    {
        public override int Code => 18;
        public override string Message => "You are forbidden to do this";
    }
}
