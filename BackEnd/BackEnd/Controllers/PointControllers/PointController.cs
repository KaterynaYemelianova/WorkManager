using Autofac;

using BusinessLogic;
using BusinessLogic.ServiceContracts.PointServiceContracts;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BackEnd.Controllers.PointControllers
{
    public abstract class PointController<TPointService> : ControllerBase where TPointService : IPointService
    {
        private IPointService PointService = BusinessLogicDependencyHolder.Dependencies.Resolve<TPointService>();

        [HttpPost]
        public async Task<HttpResponseMessage> AddData([FromBody] object data, int pointId, int detectorId)
        {
            return await Execute(
                dataObj => PointService.AddData(pointId, detectorId, dataObj),
                data
            );
        }
    }
}
