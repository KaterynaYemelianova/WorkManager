using Newtonsoft.Json;

using System;

namespace BusinessLogic.Models
{
    public class SessionModel
    {
        [JsonProperty("session_token")]
        public string Token { get; set; }

        [JsonProperty("expired_at")]
        public DateTime ExpiredAt { get; set; }
    }
}
