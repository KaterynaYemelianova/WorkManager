using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class RoleModel : ModelBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
