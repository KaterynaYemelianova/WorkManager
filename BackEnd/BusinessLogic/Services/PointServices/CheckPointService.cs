using BusinessLogic.Models;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using DataAccess.Entities;

namespace BusinessLogic.Services.PointServices
{
    internal class CheckPointService : PointBaseService<CheckPointEntity, CheckPointModel>, ICheckPointService
    {
    }
}
