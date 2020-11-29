using Newtonsoft.Json;

using System.Collections.Generic;

namespace BusinessLogic.Models
{
    public class CompanyModel : ModelBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("extra_data")]
        public object ExtraData { get; set; }

        [JsonProperty("members")]
        public IEnumerable<AccountRoleModel> Members { get; set; }

        [JsonProperty("rooms")]
        public IEnumerable<RoomModel> Rooms { get; set; }
    }
}
