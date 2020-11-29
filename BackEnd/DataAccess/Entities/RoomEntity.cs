using DataAccess.Attributes;

using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class RoomEntity : EntityBase
    {
        public string Name { get; set; }
        public float Square { get; set; }
        public float Height { get; set; }
        public int CompanyId { get; set; }
        public string ExtraData { get; set; }

        [Ignore]
        public ICollection<EnterPointEntity> EnterPoints { get; set; }

        [Ignore]
        public ICollection<CheckPointEntity> CheckPoints { get; set; }

        [Ignore]
        public ICollection<ControlPointEntity> ControlPoints { get; set; }

        [Ignore]
        public ICollection<InteractionPointEntity> InteractionPoints { get; set; }
    }
}
