using Newtonsoft.Json;

namespace Dtos
{
    public class PublicKeyDto
    {
        [JsonProperty("modulus")]
        public string Modulus { get; set; }

        [JsonProperty("exponent")]
        public string Exponent { get; set; }
    }
}