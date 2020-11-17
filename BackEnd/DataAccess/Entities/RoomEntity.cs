namespace DataAccess.Entities
{
    public class RoomEntity : EntityBase
    {
        public string Name { get; set; }
        public float Square { get; set; }
        public float Height { get; set; }
        public int CompanyId { get; set; }
        public string ExtraData { get; set; }
    }
}
