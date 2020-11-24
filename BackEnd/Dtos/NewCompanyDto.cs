using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class NewCompanyDto : IDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("extra_data")]
        public object ExtraData { get; set; }

        [JsonProperty("members")]
        public IEnumerable<AccountRoleDto> Members { get; set; }

        [JsonProperty("rooms")]
        public IEnumerable<NewRoomDto> Rooms { get; set; }
    }
}
