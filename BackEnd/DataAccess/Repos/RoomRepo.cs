using DataAccess.Entities;
using DataAccess.RepoContracts;

using System;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal class RoomRepo : ConnectedRepoBase<RoomEntity>, IRoomRepo
    {
        protected override async Task<RoomEntity> LoadDependencies(RoomEntity entity)
        {
            return entity;
        }
    }
}
