namespace Exceptions.BusinessLogic
{
    public class AccountNotFoundException : ServerException
    {
        public override int Code => 9;
        public override string Message => "Account not found";
    }
}
