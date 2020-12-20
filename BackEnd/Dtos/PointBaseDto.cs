using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class PointBaseDto : IdDto, IDto
    {
        [JsonProperty("room_id")]
        public int? RoomId { get; set; }

        [JsonProperty("extra_data")]
        public object ExtraData { get; set; }
    }
}
