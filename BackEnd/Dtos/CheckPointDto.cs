using Newtonsoft.Json;

namespace Dtos
{
    public class CheckPointDto : PointBaseDto
    {
        [JsonProperty("room_other_id")]
        public int RoomOtherId { get; set; }

        [JsonProperty("pass_condition")]
        public string PassCondition { get; set; }

        [JsonProperty("pass_condition_api_url")]
        public string PassConditionApiUrl { get; set; }

        [JsonProperty("notify_check_api_url")]
        public string NotifyCheckApiUrl { get; set; }
    }
}
