using Autofac;

using DataAccess.Entities;
using DataAccess.RepoContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal class RoomRepo : ConnectedRepoBase<RoomEntity>, IRoomRepo
    {
        private static IRepo<EnterPointEntity> EnterPointsRepo = 
            DataAccessDependencyHolder.Dependencies.Resolve<IRepo<EnterPointEntity>>();

        private static IRepo<CheckPointEntity> CheckPointsRepo =
            DataAccessDependencyHolder.Dependencies.Resolve<IRepo<CheckPointEntity>>();

        private static IRepo<InteractionPointEntity> InteractionPointsRepo =
            DataAccessDependencyHolder.Dependencies.Resolve<IRepo<InteractionPointEntity>>();

        private static IRepo<ControlPointEntity> ControlPointsRepo =
            DataAccessDependencyHolder.Dependencies.Resolve<IRepo<ControlPointEntity>>();

        protected override async Task<IEnumerable<RoomEntity>> LoadDependencies(IEnumerable<RoomEntity> entities)
        {
            int[] roomIds = entities.Select(entity => entity.Id).ToArray();

            IDictionary<int, List<EnterPointEntity>> enterPoints = 
                await EnterPointsRepo.Get(enterPoint => enterPoint.RoomId, roomIds);

            IDictionary<int, List<CheckPointEntity>> checkPoints =
                await CheckPointsRepo.Get(checkPoint => checkPoint.RoomId, roomIds);

            IDictionary<int, List<InteractionPointEntity>> interactionPoints =
                await InteractionPointsRepo.Get(interactionPoint => interactionPoint.RoomId, roomIds);

            IDictionary<int, List<ControlPointEntity>> controlPoints =
                await ControlPointsRepo.Get(controlPoint => controlPoint.RoomId, roomIds);

            foreach(RoomEntity room in entities)
            {
                if (enterPoints.ContainsKey(room.Id))
                    room.EnterPoints = enterPoints[room.Id];

                if (checkPoints.ContainsKey(room.Id))
                    room.CheckPoints = checkPoints[room.Id];

                if (interactionPoints.ContainsKey(room.Id))
                    room.InteractionPoints = interactionPoints[room.Id];

                if (controlPoints.ContainsKey(room.Id))
                    room.ControlPoints = controlPoints[room.Id];
            }

            return entities;
        }

        public override async Task<RoomEntity> Update(RoomEntity entity)
        {
            RoomEntity updated = await base.Update(entity);

            await UpdateCollection(
                updated.EnterPoints, entity.EnterPoints, 
                enterPoint => enterPoint.RoomId, updated
            );

            await UpdateCollection(
                updated.CheckPoints, entity.CheckPoints, 
                checkPoint => checkPoint.RoomId, updated
            );

            await UpdateCollection(
                updated.InteractionPoints, entity.InteractionPoints, 
                interactionPoint => interactionPoint.RoomId, updated
            );

            await UpdateCollection(
                updated.ControlPoints, entity.ControlPoints,
                controlPoint => controlPoint.RoomId, updated
            );

            return await GetById(updated.Id);
        }
    }
}
