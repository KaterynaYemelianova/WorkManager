using Newtonsoft.Json;

using System;

namespace Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ServerException : Exception
    {
        [JsonProperty("code")]
        public abstract int Code { get; }

        [JsonProperty("message")]
        public abstract override string Message { get; }

        public ServerException() { }
        public ServerException(string message) : base(message) { }
    }
}
