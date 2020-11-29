using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Dtos.PointActionDtos
{
    public class UserPointActionBaseDto : PointActionBaseDto
    {
        [Required(ErrorMessage = "user_id is required")]
        [JsonProperty("user_id")]
        public int? UserId { get; set; }
    }
}
