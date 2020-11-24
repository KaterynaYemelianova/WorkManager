using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Models
{
    public class RoomModel : ModelBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("square")]
        public float Square { get; set; }
        
        [JsonProperty("height")]
        public float Height { get; set; }
        
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }
        
        [JsonProperty("extra_data")]
        public object ExtraData { get; set; }
    }
}
