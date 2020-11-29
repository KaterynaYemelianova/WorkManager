using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class InteractionPointModel : PointBaseModel
    {
        [JsonProperty("success_condition")]
        public string SuccessCondition { get; set; }
        
        [JsonProperty("failure_condition")]
        public string FailureCondition { get; set; }

        [JsonProperty("interaction_api_url")]
        public string InteractionApiUrl { get; set; }

        [JsonProperty("notify_success_api_url")]
        public string NotifySuccessApiUrl { get; set; }

        [JsonProperty("notify_failure_api_url")]
        public string NotifyFailureApiUrl { get; set; }
    }
}
