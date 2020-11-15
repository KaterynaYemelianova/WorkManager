using Dtos.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class SignUpDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "login is required")]
        [RegularExpression("[A-Za-z0-9]{6,32}", ErrorMessage = "login must consist of english letters or digits between 6 and 32 symbols")]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "first_name is required")]
        [RegularExpression("[A-Za-z]{1,32}", ErrorMessage = "first_name must consist of letters between 1 and 32 symbols")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "last_name is required")]
        [RegularExpression("[A-Za-z]{1,32}", ErrorMessage = "last_name must consist of letters between 1 and 32 symbols")]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "password_encrypted is required")]
        [JsonProperty("password_encrypted")]
        public string PasswordEncrypted { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "email is invalid")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "public_key is required")]
        [JsonProperty("public_key")]
        public PublicKeyDto PublicKey { get; set; }
    }
}