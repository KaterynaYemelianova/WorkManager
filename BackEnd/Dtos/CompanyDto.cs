using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class CompanyDto : NewCompanyDto
    {
        [Required(ErrorMessage = "id is required")]
        [JsonProperty("id")]
        public int? Id { get; set; }
    }
}
