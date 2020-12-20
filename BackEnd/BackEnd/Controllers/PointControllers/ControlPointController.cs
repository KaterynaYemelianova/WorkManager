using BusinessLogic.ServiceContracts.PointServiceContracts;

using System.Web.Http.Cors;

namespace BackEnd.Controllers.PointControllers
{
    [EnableCors("*", "*", "*")]
    public class ControlPointController : PointController<IControlPointService>
    {
    }
}
