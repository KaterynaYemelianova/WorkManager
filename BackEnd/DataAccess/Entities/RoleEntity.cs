using DataAccess.Attributes;

namespace DataAccess.Entities
{
    [Table("roles")]
    public class RoleEntity : EntityBase
    {
        public string Name { get; set; }
    }
}
