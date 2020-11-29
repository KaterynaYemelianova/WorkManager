using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class AccountRoleDto : IdDto, IDto
    {
        [Required(ErrorMessage = "account_id is required")]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }

        [Required(ErrorMessage = "role_id is required")]
        [JsonProperty("role_id")]
        public int? RoleId { get; set; }
    }
}
