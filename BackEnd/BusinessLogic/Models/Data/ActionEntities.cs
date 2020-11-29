using DataAccess.Entities;

namespace BusinessLogic.Models.Data
{
    public class ActionEntities<TPointEntity> where TPointEntity : PointBaseEntity
    {
        public AccountEntity Account { get; set; }
        public RoleEntity Role { get; set; }
        public TPointEntity Point { get; set; }
        public RoomEntity Room { get; set; }
        public CompanyEntity Company { get; set; }
    }
}
