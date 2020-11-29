using DataAccess.Attributes;

namespace DataAccess.Entities
{
    public class AccountCompanyRoleEntity : EntityBase
    {
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
    }
}
