namespace Exceptions.BusinessLogic
{
    public class MembershipNotFoundException : ServerException
    {
        public override int Code => 14;

        public override string Message => "Membership not found";
    }
}
