using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class ControlPointModel : PointBaseModel
    {
        [JsonProperty("violation_condition")]
        public string ViolationCondition { get; set; }

        [JsonProperty("violation_api_url")]
        public string ViolationApiUrl { get; set; }

        [JsonProperty("notify_violation_api_url")]
        public string NotifyViolatoinApiUrl { get; set; }
    }
}
