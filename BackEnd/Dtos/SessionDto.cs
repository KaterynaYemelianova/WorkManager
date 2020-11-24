using Dtos.Attributes;

namespace Dtos
{
    public class SessionDto : IDto
    {
        [HeaderAutoWired("UID")]
        public int UserId { get; set; }

        [HeaderAutoWired("SESSION_TOKEN_SALTED")]
        public string SessionTokenSalted { get; set; }

        [HeaderAutoWired("SALT")]
        public string Salt { get; set; }
    }
}
