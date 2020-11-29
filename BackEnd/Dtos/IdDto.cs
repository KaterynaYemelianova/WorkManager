using Newtonsoft.Json;

namespace Dtos
{
    public class IdDto : IDto
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
    }
}
