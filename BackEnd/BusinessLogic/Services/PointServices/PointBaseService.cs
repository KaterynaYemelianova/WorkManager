using Autofac;

using BusinessLogic.Models;
using BusinessLogic.Models.Data;
using BusinessLogic.ServiceContracts;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;
using Dtos;
using System.Collections.Generic;

namespace BusinessLogic.Services.PointServices
{
    internal abstract class PointBaseService<TPointEntity, TPointModel> : IPointService
        where TPointEntity : PointBaseEntity, new()
        where TPointModel : PointBaseModel, new()
    {
        public Dictionary<int, PointData> Data = new Dictionary<int, PointData>();

        protected IAccountRepo AccountRepo = DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();
        protected IRoomRepo RoomRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRoomRepo>();
        protected ICompanyRepo CompanyRepo = DataAccessDependencyHolder.Dependencies.Resolve<ICompanyRepo>();
        protected IRepo<TPointEntity> PointRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRepo<TPointEntity>>();

        protected IMemberLocationService MemberLocationService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<IMemberLocationService>();

        protected PointExternalApiService PointExternalApiService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<PointExternalApiService>();

        public virtual void AddData(PointDataDto dto)
        {
            if (!Data.ContainsKey(dto.PointId))
                Data.Add(dto.PointId, new PointData());
            Data[dto.PointId].AddData(dto.DetectorId, dto.Data);
        }

        protected virtual ActionModels<TPointModel> GetActionModels(ActionEntities<TPointEntity> actionEntities)
        {
            return new ActionModels<TPointModel>()
            {
                Role = EntityModelMapper.Mapper.Map<RoleModel>(actionEntities.Role),
                Point = EntityModelMapper.Mapper.Map<TPointModel>(actionEntities.Point),
                Account = EntityModelMapper.Mapper.Map<AccountModel>(actionEntities.Account),
                Room = EntityModelMapper.Mapper.Map<RoomModel>(actionEntities.Room),
                Company = EntityModelMapper.Mapper.Map<CompanyModel>(actionEntities.Company)
            };
        }

        protected virtual ActionParseParameters GetActionParseParameters(ActionModels<TPointModel> actionModels)
        { 
            return new ActionParseParameters()
            { 
                Role = (RoleEnum)actionModels.Role.Id,
                Point = actionModels.Point,
                Account = actionModels.Account,
                Room = actionModels.Room,
                Company = actionModels.Company
            };
        }
    }
}
