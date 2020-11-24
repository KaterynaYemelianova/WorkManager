using Newtonsoft.Json;
using System.Collections.Generic;

namespace Exceptions.Common
{
    public class ValidationException : ServerException
    {
        public override int Code => 1;
        public override string Message => "Some fields are invalid or required not presented";

        [JsonProperty("invalid_messages")]
        public IEnumerable<string> InvalidMessages { get; private set; }

        public ValidationException(IEnumerable<string> messages)
        {
            InvalidMessages = messages;
        }

        public ValidationException(params string[] messages) : 
            this(messages as IEnumerable<string>) { }
    }
}