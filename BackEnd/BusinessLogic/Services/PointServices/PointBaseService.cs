using Autofac;

using BusinessLogic.Models;
using BusinessLogic.Models.Data;
using BusinessLogic.ServiceContracts;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using System.Collections.Generic;

namespace BusinessLogic.Services.PointServices
{
    internal abstract class PointBaseService<TPointEntity, TPointModel> : ServiceBase, IPointService
        where TPointEntity : PointBaseEntity, new()
        where TPointModel : PointBaseModel, new()
    {
        public Dictionary<int, PointData> Data = new Dictionary<int, PointData>();

        protected MemberLocationService MemberLocationService = 
            BusinessLogicDependencyHolder.Dependencies.Resolve<MemberLocationService>();

        protected IConditionParseService ConditionParseService = 
            BusinessLogicDependencyHolder.Dependencies.Resolve<IConditionParseService>();

        protected PointExternalApiService PointExternalApiService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<PointExternalApiService>();

        protected IAccountRepo AccountRepo = DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();
        protected IRoomRepo RoomRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRoomRepo>();
        protected ICompanyRepo CompanyRepo = DataAccessDependencyHolder.Dependencies.Resolve<ICompanyRepo>();
        protected IRepo<TPointEntity> PointRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRepo<TPointEntity>>();

        public virtual void AddData(int pointId, int detectorId, object data)
        {
            if (!Data.ContainsKey(pointId))
                Data.Add(pointId, new PointData());
            Data[pointId].AddData(detectorId, data);
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
                RoomExtraData = actionModels.Room.ExtraData,
                CompanyExtraData = actionModels.Company.ExtraData
            };
        }
    }
}
