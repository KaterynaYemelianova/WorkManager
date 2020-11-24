using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class NewRoomDto : IDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "square is required")]
        [JsonProperty("square")]
        public float? Square { get; set; }

        [Required(ErrorMessage = "height is required")]
        [JsonProperty("height")]
        public float? Height { get; set; }

        [JsonProperty("extra_data")]
        public object ExtraData { get; set; }
    }
}
