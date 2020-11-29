using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class AccountCompanyRoleModel : ModelBase
    {
        [JsonProperty("account")]
        public AccountModel Account { get; set; }

        [JsonProperty("role")]
        public RoleModel Role { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }
    }
}
