using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class RoomDto : NewRoomDto
    {
        [Required(ErrorMessage = "id is required")]
        [JsonProperty("id")]
        public int? Id { get; set; }
    }
}
