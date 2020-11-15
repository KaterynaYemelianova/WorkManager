using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class LogInDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "login is required")]
        [RegularExpression("[A-Za-z0-9]{6,32}", ErrorMessage = "login must consist of english letters or digits between 6 and 32 symbols")]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "password_salted is required")]
        [JsonProperty("password_salted")]
        public string PasswordSalted { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "salt is required")]
        [JsonProperty("salt")]
        public string Salt { get; set; }
    }
}
