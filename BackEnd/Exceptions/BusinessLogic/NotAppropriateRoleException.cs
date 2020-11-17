namespace Exceptions.BusinessLogic
{
    public class NotAppropriateRoleException : ServerException
    {
        private string RequiredRoleName { get; set; }

        public override int Code => 11;

        public override string Message => $"Your role is not appropriate for this action. You have to be {RequiredRoleName}!";

        public NotAppropriateRoleException(string requiresRoleName)
        {
            RequiredRoleName = requiresRoleName;
        }
    }
}
