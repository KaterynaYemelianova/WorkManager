using Dtos.PointActionDtos;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts.PointServiceContracts
{
    public interface IEnterPointService : IPointService
    {
        Task<bool> CheckEnterAbility(EnterPointActionDto dto);
        Task NotifyEnterAction(EnterPointActionDto dto);
        Task NotifyLeaveAction(EnterPointActionDto dto);
    }
}
