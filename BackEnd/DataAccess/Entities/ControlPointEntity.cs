namespace DataAccess.Entities
{
    public class ControlPointEntity : PointBaseEntity
    {
        public string ViolationCondition { get; set; }
        public string ViolationApiUrl { get; set; }
        public string NotifyViolatoinApiUrl { get; set; }
    }
}
