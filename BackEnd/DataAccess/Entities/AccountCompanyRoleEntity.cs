using DataAccess.Attributes;

namespace DataAccess.Entities
{
    internal class AccountCompanyRoleEntity : EntityBase
    {
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
    }
}
