using DataAccess.Attributes;

namespace DataAccess.Entities
{
    internal class AccountCompanyRoleEntity : EntityBase
    {
        [FKey(typeof(AccountEntity))]
        public int AccountId { get; set; }

        [FKey(typeof(CompanyEntity))]
        public int CompanyId { get; set; }

        [FKey(typeof(RoleEntity))]
        public int RoleId { get; set; }
    }
}
