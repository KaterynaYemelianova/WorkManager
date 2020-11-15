using Exceptions;
using Newtonsoft.Json;

namespace BackEnd.Output
{
    public class Response
    {
        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("error")]
        public ServerException Error { get; set; }
    }
}