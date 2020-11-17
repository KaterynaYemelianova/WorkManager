using DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class CompanyEntity : EntityBase
    {
        public string Name { get; set; }
        public string ExtraData { get; set; }

        [Ignore]
        public IDictionary<AccountEntity, RoleEntity> Members { get; set; }

        [Ignore]
        public IEnumerable<RoomEntity> Rooms { get; set; }
    }
}