using Autofac;

using BusinessLogic;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using Dtos.PointActionDtos;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BackEnd.Controllers.PointControllers
{
    public class EnterPointController : PointController<IEnterPointService>
    {
        private IEnterPointService EnterPointService = BusinessLogicDependencyHolder.Dependencies.Resolve<IEnterPointService>();

        [HttpPost]
        public async Task<HttpResponseMessage> CheckEnterAbility([FromBody] EnterPointActionDto dto)
        {
            return await Execute(d => EnterPointService.CheckEnterAbility(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Enter([FromBody] EnterPointActionDto dto)
        {
            return await Execute(d => EnterPointService.NotifyEnterAction(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Leave([FromBody] EnterPointActionDto dto)
        {
            return await Execute(d => EnterPointService.NotifyLeaveAction(d), dto);
        }
    }
}
