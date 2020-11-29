using BusinessLogic.Models;
using BusinessLogic.Models.Data;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using DataAccess.Entities;

using Dtos.PointActionDtos;

using System.Threading.Tasks;

namespace BusinessLogic.Services.PointServices
{
    internal class EnterPointService : UserPointBaseService<EnterPointEntity, EnterPointModel>, IEnterPointService
    {
        public async Task<bool> CheckEnterAbility(EnterPointActionDto dto)
        {
            ActionEntities<EnterPointEntity> entities = await GetActionEntities(dto);
            ActionModels<EnterPointModel> models = GetActionModels(entities);
            ActionParseParameters parameters = GetActionParseParameters(models);
            parameters.Action = dto;

            bool localCondition = entities.Point.PassCondition != null && 
                ConditionParseService.ParseCondition(entities.Point.PassCondition, parameters);

            if (localCondition)
                return true;

            bool externalCondition = entities.Point.PassConditionApiUrl != null && 
                await PointExternalApiService.CheckConditionByApi(entities.Point.PassConditionApiUrl, parameters);

            return externalCondition;
        }

        public async Task NotifyEnterAction(EnterPointActionDto dto)
        {
            ActionEntities<EnterPointEntity> entities = await GetActionEntities(dto);
            ActionModels<EnterPointModel> models = GetActionModels(entities);

            MemberLocationService.NotifyEnterAction(models.Account, models.Room);
        }

        public async Task NotifyLeaveAction(EnterPointActionDto dto)
        {
            ActionEntities<EnterPointEntity> entities = await GetActionEntities(dto);
            ActionModels<EnterPointModel> models = GetActionModels(entities);

            MemberLocationService.NotifyLeaveAction(models.Account, models.Room);
        }
    }
}
