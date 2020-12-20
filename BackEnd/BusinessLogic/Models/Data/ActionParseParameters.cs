using Dtos.PointActionDtos;

namespace BusinessLogic.Models.Data
{
    public class ActionParseParameters
    {
        public AccountModel Account { get; set; }
        public RoleEnum Role { get; set; }
        public PointActionBaseDto Action { get; set; }
        public PointBaseModel Point { get; set; }
        public RoomModel Room { get; set; }
        public CompanyModel Company { get; set; }
    }
}
