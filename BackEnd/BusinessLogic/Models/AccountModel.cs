using Newtonsoft.Json;

namespace BusinessLogic.Models
{
    public class AccountModel : ModelBase
    {
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
