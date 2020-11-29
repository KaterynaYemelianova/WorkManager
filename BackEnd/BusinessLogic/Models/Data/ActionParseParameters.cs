using Dtos.PointActionDtos;

namespace BusinessLogic.Models.Data
{
    public class ActionParseParameters
    {
        public AccountModel Account { get; set; }
        public RoleEnum Role { get; set; }
        public PointActionBaseDto Action { get; set; }
        public PointBaseModel Point { get; set; }
        public object RoomExtraData { get; set; }
        public object CompanyExtraData { get; set; }
    }
}
