﻿using Newtonsoft.Json;

namespace Dtos
{
    public class EnterPointDto : PointBaseDto
    {
        [JsonProperty("pass_condition")]
        public string PassCondition { get; set; }

        [JsonProperty("pass_condition_api_url")]
        public string PassConditionApiUrl { get; set; }

        [JsonProperty("notify_enter_api_url")]
        public string NotifyEnterApiUrl { get; set; }

        [JsonProperty("notify_leave_api_url")]
        public string NotifyLeaveApiUrl { get; set; }
    }
}
