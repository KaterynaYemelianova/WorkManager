using Newtonsoft.Json;

namespace Dtos
{
    public class PointDataDto : IDto
    {
        [JsonProperty("point_id")]
        public int PointId { get; set; }

        [JsonProperty("detector_id")]
        public int DetectorId { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
