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
        public IDictionary<AccountModel, RoleModel> Members { get; set; }
    }
}
