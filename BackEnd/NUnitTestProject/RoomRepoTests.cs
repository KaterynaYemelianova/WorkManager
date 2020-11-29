using DataAccess.Entities;

using System;
using System.Threading.Tasks;

namespace NUnitTestProject
{
    public class RoomRepoTests : AbstractTest<RoomEntity>
    {
        public static bool AreEqualCommon(RoomEntity generated, RoomEntity operated, RoomEntity got)
        {
            //TODO
            return true;
        }

        public override bool AreEqual(RoomEntity generated, RoomEntity operated, RoomEntity got)
        {
            return AreEqualCommon(generated, operated, got);
        }

        public override Task<RoomEntity> GetInstering()
        {
            throw new NotImplementedException();
        }

        public override Task<RoomEntity> GetUpdating()
        {
            throw new NotImplementedException();
        }
    }
}
