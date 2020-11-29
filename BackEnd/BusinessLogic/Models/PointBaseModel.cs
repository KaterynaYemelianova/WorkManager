using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class PointBaseModel : ModelBase
    {
        [JsonProperty("room_id")]
        public int? RoomId { get; set; }

        [JsonProperty("extra_data")]
        public object ExtraData { get; set; }
    }
}
