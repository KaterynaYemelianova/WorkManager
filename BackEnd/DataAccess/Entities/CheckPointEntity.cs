namespace DataAccess.Entities
{
    public class CheckPointEntity : PointBaseEntity
    {
        public int RoomOtherId { get; set; }
        public string PassCondition { get; set; }
        public string PassConditionApiUrl { get; set; }
        public string NotifyCheckApiUrl { get; set; }
    }
}
