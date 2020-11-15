using Newtonsoft.Json;

namespace Exceptions.DataAccess
{
    public class NotFoundException : ServerException
    {
        [JsonProperty("what")]
        public string What { get; private set; }
        public override int Code => 2;
        public override string Message => $"{What} not found";

        public NotFoundException(string what) : base(what) { }
    }
}
