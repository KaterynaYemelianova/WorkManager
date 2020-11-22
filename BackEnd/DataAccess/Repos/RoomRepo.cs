using DataAccess.Entities;
using DataAccess.RepoContracts;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal class RoomRepo : ConnectedRepoBase<RoomEntity>, IRoomRepo
    {
        protected override async Task<IEnumerable<RoomEntity>> LoadDependencies(IEnumerable<RoomEntity> entities)
        {
            return entities;
        }
    }
}
