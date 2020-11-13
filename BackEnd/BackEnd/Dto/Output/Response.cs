using Newtonsoft.Json;

namespace BackEnd.Dto.Output
{
    public class Response
    {
        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }
}