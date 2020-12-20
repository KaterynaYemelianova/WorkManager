using Autofac;

using BusinessLogic;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using Dtos;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BackEnd.Controllers.PointControllers
{
    public abstract class PointController<TPointService> : ControllerBase where TPointService : IPointService
    {
        private IPointService PointService = BusinessLogicDependencyHolder.Dependencies.Resolve<TPointService>();

        [HttpPost]
        public async Task<HttpResponseMessage> AddData([FromBody] PointDataDto dto)
        {
            return await Execute(
                d => PointService.AddData(d), dto
            );
        }
    }
}
