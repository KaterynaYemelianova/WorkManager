using Newtonsoft.Json;

namespace Dtos
{
    public class PublicKeyDto : IDto
    {
        [JsonProperty("modulus")]
        public string Modulus { get; set; }

        [JsonProperty("exponent")]
        public string Exponent { get; set; }
    }
}