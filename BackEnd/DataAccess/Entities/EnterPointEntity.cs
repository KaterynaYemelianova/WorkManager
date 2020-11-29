namespace DataAccess.Entities
{
    public class EnterPointEntity : PointBaseEntity
    {
        public string PassCondition { get; set; }
        public string PassConditionApiUrl { get; set; }
        public string NotifyEnterApiUrl { get; set; }
        public string NotifyLeaveApiUrl { get; set; }
    }
}
