using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class AccountCompanyRoleDto : AccountRoleDto
    {
        [Required(ErrorMessage = "company_id is required")]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }
    }
}
