namespace DataAccess.Entities
{
    public class InteractionPointEntity : PointBaseEntity
    {
        public string SuccessCondition { get; set; }
        public string FailureCondition { get; set; }
        public string InteractionApiUrl { get; set; }
        public string NotifySuccessApiUrl { get; set; }
        public string NotifyFailureApiUrl { get; set; }
    }
}
