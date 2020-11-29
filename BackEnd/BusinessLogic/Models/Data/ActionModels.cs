namespace BusinessLogic.Models.Data
{
    public class ActionModels<TPointModel> where TPointModel : PointBaseModel
    {
        public AccountModel Account { get; set; }
        public RoleModel Role { get; set; }
        public TPointModel Point { get; set; }
        public RoomModel Room { get; set; }
        public CompanyModel Company { get; set; }
    }
}
