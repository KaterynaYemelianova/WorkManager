using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Dtos.PointActionDtos
{
    public class PointActionBaseDto : IDto
    {
        [Required(ErrorMessage = "point_id is required")]
        [JsonProperty("point_id")]
        public int? PointId { get; set; }

        [JsonProperty("extra_data")]
        public object ExtraData { get; set; }
    }
}
