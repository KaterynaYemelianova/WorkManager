using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class RoomDto : IdDto, IDto
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

        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("enter_points")]
        public ICollection<EnterPointDto> EnterPoints { get; set; }

        [JsonProperty("check_points")]
        public ICollection<CheckPointDto> CheckPoints { get; set; }

        [JsonProperty("control_points")]
        public ICollection<ControlPointDto> ControlPoints { get; set; }

        [JsonProperty("interaction_points")]
        public ICollection<InteractionPointDto> InteractionPoints { get; set; }
    }
}
