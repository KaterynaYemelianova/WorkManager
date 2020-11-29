namespace Exceptions.BusinessLogic
{
    public class MembershipDuplicationException : ServerException
    {
        public override int Code => 13;

        public override string Message => "Company has already hired member with such id";
    }
}
