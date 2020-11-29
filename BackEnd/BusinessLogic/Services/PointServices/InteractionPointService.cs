using BusinessLogic.Models;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using DataAccess.Entities;

namespace BusinessLogic.Services.PointServices
{
    internal class InteractionPointService : PointBaseService<InteractionPointEntity, InteractionPointModel>, IInteractionPointService
    {
    }
}
