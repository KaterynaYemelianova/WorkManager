using DataAccess.Attributes;

using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class CompanyEntity : EntityBase
    {
        public string Name { get; set; }
        public string ExtraData { get; set; }

        [Ignore]
        public IDictionary<AccountEntity, RoleEntity> Members { get; set; }

        [Ignore]
        public ICollection<RoomEntity> Rooms { get; set; }
    }
}