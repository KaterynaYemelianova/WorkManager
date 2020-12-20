using BusinessLogic.Models;
using BusinessLogic.Models.Data;

using DataAccess.Entities;

using Dtos.PointActionDtos;

using Exceptions.BusinessLogic;

using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.PointServices
{
    internal class UserPointBaseService<TPointEntity, TPointModel> : PointBaseService<TPointEntity, TPointModel>
        where TPointEntity : PointBaseEntity, new()
        where TPointModel : PointBaseModel, new()
    {
        protected async Task<ActionEntities<TPointEntity>> GetActionEntities(UserPointActionBaseDto dto)
        {
            AccountEntity account = await AccountRepo.GetById(dto.UserId.Value);

            if (account == null)
                throw new AccountNotFoundException();

            TPointEntity pointEntity = await PointRepo.GetById(dto.PointId.Value);

            if (pointEntity == null)
                throw new PointNotFoundException();

            RoomEntity roomEntity = await RoomRepo.GetById(pointEntity.RoomId);
            CompanyEntity companyEntity = await CompanyRepo.GetById(roomEntity.CompanyId);

            RoleEntity role = companyEntity.Members.First(
                member => member.Key.Id == account.Id
            ).Value;

            return new ActionEntities<TPointEntity>()
            {
                Role = role,
                Account = account,
                Point = pointEntity,
                Room = roomEntity,
                Company = companyEntity
            };
        }
    }
}
