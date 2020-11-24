using Dtos.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class AuthorizedDto<TDto> where TDto : IDto
    {
        [HeaderAutoWired]
        public SessionDto Session { get; set; }
        
        [Required(ErrorMessage = "data is required")]
        [JsonProperty("data")]
        public TDto Data { get; set; }
    }
}
