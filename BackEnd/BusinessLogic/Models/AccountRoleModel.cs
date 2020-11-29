using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class AccountRoleModel
    {
        [JsonProperty("account")]
        public AccountModel Account { get; set; }

        [JsonProperty("role")]
        public RoleModel Role { get; set; }
    }
}
