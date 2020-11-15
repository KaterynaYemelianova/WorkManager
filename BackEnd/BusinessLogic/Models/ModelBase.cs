using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class ModelBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
