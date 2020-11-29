using Newtonsoft.Json;

namespace Dtos
{
    public class ControlPointDto : PointBaseDto
    {
        [JsonProperty("violation_condition")]
        public string ViolationCondition { get; set; }

        [JsonProperty("violation_api_url")]
        public string ViolationApiUrl { get; set; }

        [JsonProperty("notify_violation_api_url")]
        public string NotifyViolatoinApiUrl { get; set; }
    }
}
