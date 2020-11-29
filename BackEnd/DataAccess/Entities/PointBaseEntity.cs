namespace DataAccess.Entities
{
    public abstract class PointBaseEntity : EntityBase
    {
        public int RoomId { get; set; }
        public string ExtraData { get; set; }
    }
}
