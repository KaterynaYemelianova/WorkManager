namespace DataAccess.Entities
{
    public class AccountEntity : EntityBase
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsSuperadmin { get; set; }
    }
}
